using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Chat;
using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;

public class PunChatManager : MonoBehaviour, IChatClientListener
{
    public static PunChatManager instance;
    public string chatRoomName = "world";
    public float reconnectTime = 15;

    public System.Action<string> OnNewMessageComing;

    ChatClient chatClient;
    bool isInGarentee = false;

    void Awake() => instance = this;
    void Start()
    {
        chatClient = new ChatClient(this);
    }

    public void GarenteeConnect(){
        if(isInGarentee)
            return;

        StartCoroutine(GarenteeConnectWork());
    }

    IEnumerator GarenteeConnectWork(){
        isInGarentee = true;
        while(true){
            if(chatClient == null || !chatClient.CanChat)
                DoConnect();
            
            yield return new WaitForSeconds(reconnectTime);
        }
    }

    void Update()
    {
        if(chatClient != null)
            chatClient.Service();
    }

    public void OnDestroy(){
        if (this.chatClient != null)
        {
            this.chatClient.Disconnect();
        }
    }

    public void OnApplicationQuit()
    {
        if (this.chatClient != null)
        {
            this.chatClient.Disconnect();
        }
    }

    void DoConnect(){
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, 
        PhotonNetwork.AppVersion, new AuthenticationValues(PlayerManager.instance.playerName));
    }

    public void SendMessagePUN(string msg){

        if(chatClient != null && chatClient.CanChat)
            chatClient.PublishMessage(chatRoomName, msg);
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
        {
            Debug.LogError(message);
        }
        else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
        {
            Debug.LogWarning(message);
        }
        else
        {
            Debug.Log(message);
        }
    }

    public void OnChatStateChange(ChatState state)
    {
        //Debug.Log("OnChatStateChange");
        //TXT_Status.text = state.ToString();
    }

    public void OnConnected()
    {
        //this.chatClient.Subscribe(this.ChannelsToJoinOnConnect, this.HistoryLengthToFetch);
        chatClient.Subscribe(new string[] {chatRoomName});
        chatClient.SetOnlineStatus(ChatUserStatus.Online);
    }

    public void OnDisconnected()
    {
        //throw new System.NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < senders.Length; i++)
        {
            OnNewMessageComing?.Invoke($"{senders[i]} : {messages[i]}");
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        //throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        //throw new System.NotImplementedException();
        Debug.Log("Status update");
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        // foreach (var item in channels)
        // {
        //     chatClient.PublishMessage(item, "joined");
        // }

        Debug.Log("Connected !");
    }

    public void OnUnsubscribed(string[] channels)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        //throw new System.NotImplementedException();
    }
}
