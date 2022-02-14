using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;

public class NodeInputName : NodeControlBase
{
    public VideoPlayer backgroundPlayer;
    public TMP_InputField INP_UserName;
    public Button BTN_Confirm;
    void Start()
    {
        BTN_Confirm.onClick.AddListener(ConfirmName);
    }

    void ConfirmName(){
        if(string.IsNullOrEmpty(INP_UserName.text))
            return;

        //SystemConfig.Instance.SaveData(SaveKeys.Username, INP_UserName.text);
        System.Guid myGUID = System.Guid.NewGuid();
        Debug.Log($"my id is : {myGUID}");

        UIManager.NodeMessage("Go");

    }

    public override void OnShowTodo(){
        backgroundPlayer.Play();
    }
    //public override void OnShowFinTodo(){}

    public override void OnHideTodo(){
        backgroundPlayer.Pause();
    }
    // public override void OnHideFinTodo(){}
}
