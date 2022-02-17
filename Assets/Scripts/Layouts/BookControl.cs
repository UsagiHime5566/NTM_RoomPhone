using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BookControl : MonoBehaviour
{
    public TextMeshProUGUI TXT_Total;
    public LyricsContent Prefab_Lyric;
    public Transform contentParent;

    List<LyricsContent> lyrics;
    void Start()
    {
        lyrics = new List<LyricsContent>();
        
        for (int i = 0; i < PlayerManager.instance.totalBookCount; i++)
        {
            var temp = Instantiate(Prefab_Lyric, contentParent);
            temp.ContentIndex = i;

            lyrics.Add(temp);
        }
    }

    public void RefreshBook(){
        int total = 0;
        foreach (var item in lyrics)
        {
            total += item.UpdateSelf();
        }

        TXT_Total.text = $"{total}/{PlayerManager.instance.totalBookCount}";
    }
}
