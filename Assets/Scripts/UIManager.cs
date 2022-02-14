using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : HimeLib.SingletonMono<UIManager>
{
    public RenderTexture mainRenderTexture;
    public Canvas MasterCanvas;
    public CanvasGroupExtend BackgroundCanvas;
    void Start()
    {
        MasterCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

        
    }

}
