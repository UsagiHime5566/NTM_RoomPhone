using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class BombMessage : MonoBehaviour
{
    public TextMeshProUGUI TXT_BombMessage;
    public float duration = 7;
    public int space = 200;
    void Start()
    {
        //Debug.Log($"show {Screen.width}/{Screen.height}");
        TXT_BombMessage.rectTransform.anchoredPosition = new Vector2(Screen.width/2, Random.Range(-Screen.height/2 + space, Screen.height/2 - space));
        TXT_BombMessage.rectTransform.DOAnchorPosX(-Screen.width*2, duration).SetEase(Ease.Linear).OnComplete(() => {
            Destroy(gameObject, 1);
        });
    }

    public void SetText(string msg){
        TXT_BombMessage.text = msg;
    }
}
