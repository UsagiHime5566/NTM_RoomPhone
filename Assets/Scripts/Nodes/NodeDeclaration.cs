using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using I2.Loc;

public class NodeDeclaration : NodeControlBase
{
    public Button BTN_Disagree;
    public Button BTN_zhTW;
    public Button BTN_Japan;
    void Start()
    {
        BTN_Disagree.onClick.AddListener(() => {
            Application.Quit();
        });

        BTN_zhTW.onClick.AddListener(() => {
            LocalizationManager.CurrentLanguage = "Chinese (Traditional)";
        });

        BTN_Japan.onClick.AddListener(() => {
            LocalizationManager.CurrentLanguage = "Japanese";
        });
    }

    // public override void OnShowTodo(){}
    public override void OnShowFinTodo(){
        
    }

    // public override void OnHideTodo(){}
    // public override void OnHideFinTodo(){}
}
