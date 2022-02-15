using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NodeGame : NodeControlBase
{
    public CanvasGroupExtend PL_History;
    public CanvasGroupExtend PL_Book;
    public TMP_InputField INP_Chat;

    public Button BTN_History;
    public Button BTN_Book;


    void Start()
    {
        PL_History.CloseSelf();
        PL_Book.CloseSelf();

        BTN_History.onClick.AddListener(() => {
            PL_History.OpenSelf();
        });

        BTN_Book.onClick.AddListener(() => {
            PL_Book.OpenSelf();
        });

        INP_Chat.onEndEdit.AddListener(onChatSend);
    }

    void onChatSend(string msg){
        
    }

    public override void OnShowTodo(){
        UIManager.instance.BackgroundCanvas.CloseSelf();
    }

    // public override void OnShowFinTodo(){}

    public override void OnHideTodo(){
        UIManager.instance.BackgroundCanvas.OpenSelf();
    }

    // public override void OnHideFinTodo(){}
}
