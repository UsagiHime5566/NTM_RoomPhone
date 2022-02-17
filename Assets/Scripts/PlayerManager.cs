using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : HimeLib.SingletonMono<PlayerManager>
{
    public int totalBookCount = 20;
    public List<int> userGetsLyrics;
    void Start()
    {
        userGetsLyrics = SystemConfig.Instance.GetData<List<int>>(SaveKeys.UserBook, InitBook());
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
