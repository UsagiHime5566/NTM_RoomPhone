using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public abstract class CameraHelperBase : MonoBehaviour
{
    protected Camera _ARCamera;

    protected virtual void Awake() {
        _ARCamera = GetComponent<Camera>();
    }

    public void VisibleCamera(bool visible){
        if(visible)
            _ARCamera.depth = 5;
        else
            _ARCamera.depth = -5;
    }

    
    public abstract void StartCamera(System.Action callback);   // callback is for inherit method to use
    
    public abstract void ResumeCamera(System.Action callback);

    public abstract void PauseCamera();

    public abstract void StopCamera();
    
}
