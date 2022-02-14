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
    void Start()
    {
        
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
