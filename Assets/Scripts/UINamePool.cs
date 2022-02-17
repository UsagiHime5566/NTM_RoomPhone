using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UINamePool : HimeLib.SingletonMono<UINamePool>
{
    public Camera FromCamera;
    public FlowNameUI Prefab_Name;
    public List<FlowNameUI> nameUIs;

    public Vector2 GetAnchPos(Vector3 worldPoint){
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(FromCamera, worldPoint);
        return transform.InverseTransformPoint(screenPoint);
    }

    public void CreateName(string msg, Transform attach){
        var n = Instantiate(Prefab_Name, transform);
        n.SetText(msg, attach);

        nameUIs.Add(n);
    }
}
