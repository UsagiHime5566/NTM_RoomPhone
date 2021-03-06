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

    public TextMeshProUGUI TXT_NetStats;

    public HistoryControl historyControl;
    public BookControl bookControl;


    void Start()
    {
        PL_History.CloseSelf();
        PL_Book.CloseSelf();

        BTN_History.onClick.AddListener(() => {
            PL_History.OpenSelf();
        });

        BTN_Book.onClick.AddListener(() => {
            PL_Book.OpenSelf();
            bookControl.RefreshBook();
        });

        INP_Chat.onEndEdit.AddListener(onChatSend);

        PunManager.instance.OnNetworkChanged += UpdateNetworkStats;
    }

    void onChatSend(string msg){
        if(string.IsNullOrEmpty(msg))
            return;
            
        PunChatManager.instance.SendMessagePUN(msg);
        INP_Chat.text = "";
    }

    void UpdateNetworkStats(string msg){
        TXT_NetStats.text = msg;
    }

    public override void OnShowTodo(){
        UIManager.instance.BackgroundCanvas.CloseSelf();

        PunManager.instance.GarenteeConnect();
        PunChatManager.instance.GarenteeConnect();

    }

    // public override void OnShowFinTodo(){}

    public override void OnHideTodo(){
        UIManager.instance.BackgroundCanvas.OpenSelf();
    }

    // public override void OnHideFinTodo(){}
}
