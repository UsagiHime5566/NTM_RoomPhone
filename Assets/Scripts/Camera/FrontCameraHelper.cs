using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NatCamera))]
public class FrontCameraHelper : CameraHelperBase
{
    [Header("Events")]
    public System.Action OnStartCamera;
    public System.Action OnPause;

    NatCamera scriptsCamera;

    protected override void Awake(){
        base.Awake();
        scriptsCamera = GetComponent<NatCamera>();
    }

    public override void StartCamera(System.Action callback){
        scriptsCamera.StartCamera(delegate {
            VisibleCamera(true);

            OnStartCamera?.Invoke();
            callback?.Invoke();
        });
    }

    public override void ResumeCamera(System.Action callback){
        VisibleCamera(true);
        
        OnStartCamera?.Invoke();
        callback?.Invoke();
    }

    public override void PauseCamera(){
        VisibleCamera(false);

        scriptsCamera.PauseCamera();
        OnPause?.Invoke();
    }

    public override void StopCamera(){
        VisibleCamera(false);

        scriptsCamera.StopCamera();
    }
}
