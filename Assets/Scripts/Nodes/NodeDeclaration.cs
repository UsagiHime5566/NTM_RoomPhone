using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeDeclaration : NodeControlBase
{
    public Button BTN_Disagree;
    void Start()
    {
        BTN_Disagree.onClick.AddListener(() => {
            Application.Quit();
        });
    }

    // public override void OnShowTodo(){}
    // public override void OnShowFinTodo(){}

    // public override void OnHideTodo(){}
    // public override void OnHideFinTodo(){}
}
