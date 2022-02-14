using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.UI;
using Doozy.Engine.Soundy;


[RequireComponent(typeof(UIView))]
public class NodeControlBase : MonoBehaviour
{
    protected UIView view;

    public virtual void Awake(){
        view = GetComponent<UIView>();
        RegistEvents();
    }
    
    public virtual void RegistEvents()
    {
        view.ShowBehavior.OnStart.Event.AddListener(OnShowTodo);
        view.ShowBehavior.OnFinished.Event.AddListener(OnShowFinTodo);
        view.HideBehavior.OnStart.Event.AddListener(OnHideTodo);
        view.HideBehavior.OnFinished.Event.AddListener(OnHideFinTodo);
    }

    public virtual void OnShowTodo(){}
    public virtual void OnShowFinTodo(){}

    public virtual void OnHideTodo(){}
    public virtual void OnHideFinTodo(){}

    // public override void OnShowTodo(){}
    // public override void OnShowFinTodo(){}

    // public override void OnHideTodo(){}
    // public override void OnHideFinTodo(){}
}
