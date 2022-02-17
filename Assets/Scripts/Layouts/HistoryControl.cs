using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HistoryControl : MonoBehaviour
{
    public TMP_InputField INP_Histroy;
    public int maxChatCount = 25;

    Queue<string> queueMessage;


    void Awake(){
        queueMessage = new Queue<string>();
    }

    void Start()
    {
        PunChatManager.instance.OnNewMessageComing += OnNewMessage;
    }

    void OnNewMessage(string msg){
        if(string.IsNullOrEmpty(msg))
            return;
            
        queueMessage.Enqueue(msg);
        
        if(queueMessage.Count > maxChatCount)
            queueMessage.Dequeue();

        INP_Histroy.text = "";
        foreach (var item in queueMessage)
        {
            INP_Histroy.text += $"{item}\n";
        }
    }
}
