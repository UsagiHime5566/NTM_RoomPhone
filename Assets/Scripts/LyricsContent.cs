using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LyricsContent : MonoBehaviour
{
    public TextMeshProUGUI TXT_Unlock;
    public TextMeshProUGUI TXT_Content;

    void Start()
    {
        
    }

    public void SetStats(bool val){
        TXT_Unlock.gameObject.SetActive(!val);
        TXT_Content.gameObject.SetActive(val);
    }
}
