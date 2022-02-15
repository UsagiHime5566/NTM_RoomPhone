using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class NodeRecordVideo : NodeControlBase
{
    public CameraHelperBase RecordCamera;

    [Header(@"UI")]
    public Button BTN_StartRecord;
    public Button BTN_CancelUpload;
    public Button BTN_Back;
    public Image IMG_RemainTime;
    public CanvasGroupExtend PL_Tip;
    public CanvasGroupExtend PL_Uploading;

    [Header(@"Settings"), Range(5f, 60f), Tooltip(@"Maximum duration that button can be pressed.")]
    public float maxDuration = 10f;

    bool isRecording = false;
    bool normalEnd = false;

    void Start()
    {
        PL_Tip.OpenSelf();
        PL_Uploading.CloseSelfImmediate();

        BTN_StartRecord.onClick.AddListener(StartRecord);
        BTN_CancelUpload.onClick.AddListener(CancelUpload);
        BTN_Back.onClick.AddListener(StopRecord);
    }

    public void StartRecord()
    {
        isRecording = true;
        normalEnd = false;
        BTN_StartRecord.gameObject.SetActive(false);
        StartCoroutine(Countdown());
    }

    public void StopRecord()
    {
        isRecording = false;
    }

    private IEnumerator Countdown()
    {
        // Start recording
        RecordManager.instance.recordHelper.StartRecording();

        // Animate the countdown
        var startTime = Time.time;
        while (isRecording)
        {
            var ratio = 1 - ((Time.time - startTime) / maxDuration);
            if(ratio <= 0f){
                isRecording = false;
                normalEnd = true;
            }
            IMG_RemainTime.fillAmount = ratio;
            yield return null;
        }
        
        // Stop recording
        RecordManager.instance.recordHelper.StopRecording(normalEnd);

        //Goto Upload
        if(normalEnd){
            StartUpload();
        }
    }

    private void UIReset()
    {
        IMG_RemainTime.fillAmount = 1.0f;
        BTN_StartRecord.gameObject.SetActive(true);
    }



    void StartUpload(){
        PL_Uploading.OpenSelf();
    }

    public void CancelUpload(){
        UIReset();
    }

    private void OnApplicationPause(bool pauseStatus) {
        // true 代表已經暫停
        if(pauseStatus == true){
            isRecording = false;
        } else {
            normalEnd = false;
            UIReset();
        }
    }

    public override void OnShowTodo(){
        UIReset();
        RecordCamera.StartCamera(null);
        UIManager.instance.BackgroundCanvas.CloseSelf();
    }

    //public override void OnShowFinTodo(){}

    public override void OnHideTodo(){
        RecordCamera.StopCamera();
        UIManager.instance.BackgroundCanvas.OpenSelf();
    }
    // public override void OnHideFinTodo(){}
}
