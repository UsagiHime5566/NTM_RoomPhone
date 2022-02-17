using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlowNameUI : MonoBehaviour
{
    public TextMeshProUGUI target;
    Transform ownerAttach;

    void Update()
    {
        if(ownerAttach == null){
            Destroy(gameObject);
            return;
        }

        Vector2 anchoredPosition = UINamePool.instance.GetAnchPos(ownerAttach.position);
        target.rectTransform.anchoredPosition = anchoredPosition;
    }

    public void SetText(string msg, Transform attach){
        target.text = msg;
        ownerAttach = attach;
    }
}
