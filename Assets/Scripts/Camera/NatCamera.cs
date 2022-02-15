using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NatCam;
using UnityEngine.UI;

public class NatCamera : MonoBehaviour
{
    [Header("UI")]
    public RawImage rawImage;
    public AspectRatioFitter aspectFitter;      //Mode is "Height Control Width" , RectTransform Stretch to Screen

    CameraDevice[] cameras;
    int activeCamera = -1;
    Texture previewTexture;
    Action StartedCallback;

    bool isReady = false;

    #if UNITY_EDITOR
        void Start(){}
        public void StartCamera(Action callback){
            Debug.LogError("NatCam doesn't support PC platform.");
            callback?.Invoke();
        }
        public void PauseCamera(){}
        public void StopCamera(){}
    
    #else

    void Start()
    {
        // Check permission
        cameras = CameraDevice.GetDevices();

        if (cameras == null)
        {
            Debug.Log("User has not granted camera permission");
            return;
        }

        // Pick camera
        for (var i = 0; i < cameras.Length; i++)
            if (cameras[i].IsFrontFacing)
            {
                activeCamera = i;
                break;
            }
        if (activeCamera == -1)
        {
            Debug.LogError("Camera is null.");
            return;
        }

        rawImage.enabled = false;
        isReady = true;
    }

    public void StartCamera(Action callback){
        if(!isReady)
            return;

        if(!cameras[activeCamera].IsRunning){
            cameras[activeCamera].StartPreview(OnStart);

            StartedCallback = null;
            StartedCallback += callback;

            Debug.Log($"Prepare to Start :{activeCamera}");
            return;
        }

        rawImage.enabled = true;
        callback?.Invoke();
    }

    public void OnStart(Texture preview){
        // Display the preview
        previewTexture = preview;
        rawImage.texture = preview;
        aspectFitter.aspectRatio = preview.width / (float)preview.height;

        rawImage.enabled = true;
        StartedCallback?.Invoke();
        StartedCallback = null;
    }

    public void PauseCamera(){
        Debug.Log($"pause camera :{activeCamera}/{cameras}");
        
        rawImage.enabled = false;

        if(!cameras[activeCamera].IsRunning)
            return;

        cameras[activeCamera].StopPreview();
    }

    public void StopCamera(){
        Debug.Log($"stop camera :{activeCamera}/{cameras}");

        rawImage.enabled = false;

        if(!cameras[activeCamera].IsRunning)
            return;

        cameras[activeCamera].StopPreview();
    }

    #endif
}
