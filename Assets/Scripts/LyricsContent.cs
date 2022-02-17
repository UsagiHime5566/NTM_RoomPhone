using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LyricsContent : MonoBehaviour
{
    public TextMeshProUGUI TXT_Unlock;
    public TextMeshProUGUI TXT_Content;
    public int ContentIndex;

    void Start()
    {
        TXT_Content.text = $"宝物_{ContentIndex + 1}";
        RefManager.instance.OnLyricsDownloaded += LyricUpdate;

        if(ContentIndex < PlayerManager.instance.userGetsLyrics.Count)
            SetStats(PlayerManager.instance.userGetsLyrics[ContentIndex] == 1);
    }

    void LyricUpdate(List<string> lyric){
        TXT_Content.text = lyric[ContentIndex];
    }

    public void SetStats(bool val){
        TXT_Unlock.gameObject.SetActive(!val);
        TXT_Content.gameObject.SetActive(val);
    }

    public int UpdateSelf(){
        if(ContentIndex < PlayerManager.instance.userGetsLyrics.Count){
            if(PlayerManager.instance.userGetsLyrics[ContentIndex] == 1){
                SetStats(true);
                return 1;
            }
        }
        return 0;
    }
}
