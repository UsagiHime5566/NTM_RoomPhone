using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Threading.Tasks;
using TMPro;

public class NpcObject : MonoBehaviour , IPointerDownHandler
{
    [Header("Data")]
    public int bookIndex;

    [Header("UI Control")]
    public SpriteRenderer spriteObject;
    public float spriteMoveY = 0.25f;
    public float spriteDuration = 0.7f;

    public CanvasGroupExtend MessageObject;
    public float FadeTime = 0.25f;
    public int MessageDelay = 3000;

    public TextMeshProUGUI TXT_Message;

    bool isMessageShowing = false;

    void Start(){
        spriteObject.transform.DOLocalMoveY(-spriteMoveY, spriteDuration).SetLoops(-1, LoopType.Yoyo);
        
        MessageObject.CloseSelfImmediate();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"{name} clicked!");

        

        GetMessage();
    }

    async void GetMessage(){
        if(isMessageShowing)    
            return;

        
        MessageObject.OpenSelf();
        spriteObject.DOFade(0, FadeTime);
        
        isMessageShowing = true;
        await Task.Delay(MessageDelay);
        if(this == null) return;
        
        isMessageShowing = false;

        MessageObject.CloseSelf();
        spriteObject.DOFade(1, FadeTime);
    }
}
