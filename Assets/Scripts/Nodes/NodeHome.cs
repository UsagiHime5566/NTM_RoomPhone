using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using I2.Loc;

public class NodeHome : NodeControlBase
{
    public VideoPlayer backgroundPlayer;
    public Button BTN_zhTW;
    public Button BTN_Japan;
    void Start()
    {
        BTN_zhTW.onClick.AddListener(() => {
            LocalizationManager.CurrentLanguage = "Chinese (Traditional)";
        });

        BTN_Japan.onClick.AddListener(() => {
            LocalizationManager.CurrentLanguage = "Japanese";
        });
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
