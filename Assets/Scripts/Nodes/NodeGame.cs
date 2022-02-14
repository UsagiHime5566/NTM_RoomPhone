using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodeGame : NodeControlBase
{
    public CanvasGroupExtend PL_History;
    public CanvasGroupExtend PL_Book;
    public TMP_InputField INP_Chat;


    void Start()
    {
        PL_History.CloseSelf();
        PL_Book.CloseSelf();
    }

    // public override void OnShowTodo(){}
    // public override void OnShowFinTodo(){}

    // public override void OnHideTodo(){}
    // public override void OnHideFinTodo(){}
}
