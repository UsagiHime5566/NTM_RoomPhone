using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;


public class NodeHome : NodeControlBase
{
    public VideoPlayer backgroundPlayer;
    public Button EnterGame;
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
