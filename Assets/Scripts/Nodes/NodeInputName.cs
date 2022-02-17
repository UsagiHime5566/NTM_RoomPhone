using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;

public class NodeInputName : NodeControlBase
{
    public TMP_InputField INP_UserName;
    public Button BTN_Confirm;
    void Start()
    {
        BTN_Confirm.onClick.AddListener(ConfirmName);
    }

    void ConfirmName(){
        if(string.IsNullOrEmpty(INP_UserName.text))
            return;

        string myGUID = System.Guid.NewGuid().ToString().Replace("-", "");
        Debug.Log($"generated id is : {myGUID}");
        
        PlayerManager.instance.UpdatePlayerInfo(INP_UserName.text, myGUID);

        UIManager.NodeMessage("Go");
    }

    //public override void OnShowTodo(){}
    //public override void OnShowFinTodo(){}
    //public override void OnHideTodo(){}
    //public override void OnHideFinTodo(){}
}
