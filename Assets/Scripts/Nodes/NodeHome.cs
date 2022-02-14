using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;


public class NodeHome : NodeControlBase
{
    public VideoPlayer backgroundPlayer;
    public Button EnterGame;
    public string nullName = "null";
    public string GoNew = "GoNew";
    public string GoGame = "GoGame";
    void Start()
    {
        EnterGame.onClick.AddListener(ToEnterGame);
    }

    void ToEnterGame(){
        Debug.Log("EnterGame");

        string username = SystemConfig.Instance.GetData<string>(SaveKeys.Username, nullName);

        if(username == nullName){
            UIManager.NodeMessage(GoNew);
        }
    }



    public override void OnShowTodo(){
        backgroundPlayer.Play();
    }
    //public override void OnShowFinTodo(){}

    public override void OnHideTodo(){
        backgroundPlayer.Pause();
    }
    public override void OnHideFinTodo(){
        backgroundPlayer.Pause();
    }
}
