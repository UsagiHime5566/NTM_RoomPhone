using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : HimeLib.SingletonMono<PlayerManager>
{
    [Header("Game Const")]
    public int totalBookCount = 20;
    public string nullName = "[null]";

    [Header("Player Data")]
    public string playerName;
    public string uuid;
    public List<int> userGetsLyrics;
    void Start()
    {
        userGetsLyrics = SystemConfig.Instance.GetData<List<int>>(SaveKeys.UserBook, InitBook());

        playerName = SystemConfig.Instance.GetData<string>(SaveKeys.Username, nullName);
        uuid = SystemConfig.Instance.GetData<string>(SaveKeys.UUID, "[uid]");
    }

    public bool IsNewUser(){
        return playerName == nullName;
    }

    public void UpdatePlayerInfo(string n, string u){
        playerName = n;
        uuid = u;

        SystemConfig.Instance.SaveData(SaveKeys.Username, playerName);
        SystemConfig.Instance.SaveData(SaveKeys.UUID, uuid);
    }

    List<int> InitBook(){
        List<int> result = new List<int>();
        for (int i = 0; i < totalBookCount; i++)
        {
            result.Add(0);
        }
        return result;
    }

    public void GetNewLyric(int index){
        if(userGetsLyrics.Count < totalBookCount){
            userGetsLyrics = InitBook();
        }

        userGetsLyrics[index] = 1;

        SystemConfig.Instance.SaveData(SaveKeys.UserBook, userGetsLyrics);
        Debug.Log($"Get new Lyric :{index}");
    }
}
