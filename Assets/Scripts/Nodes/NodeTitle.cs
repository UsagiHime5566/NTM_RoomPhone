using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class NodeTitle : NodeControlBase
{
    public VideoPlayer backgroundPlayer;
    void Start()
    {
        
    }

    public override void OnShowTodo(){
        backgroundPlayer.Play();
    }
    //public override void OnShowFinTodo(){}

    public override void OnHideTodo(){
        backgroundPlayer.Stop();
    }
    // public override void OnHideFinTodo(){}
}
