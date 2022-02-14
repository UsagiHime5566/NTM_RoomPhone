using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class NodeRecordVideo : NodeControlBase
{
    public Button BTN_StartRecord;
    public Image IMG_RemainTime;
    public CanvasGroupExtend PL_Tip;
    public CanvasGroupExtend PL_Uploading;
    void Start()
    {
        PL_Tip.OpenSelf();
        PL_Uploading.CloseSelfImmediate();
    }

    public override void OnShowTodo(){
        
    }
    //public override void OnShowFinTodo(){}

    public override void OnHideTodo(){
        
    }
    // public override void OnHideFinTodo(){}
}
