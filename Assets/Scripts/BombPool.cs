using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPool : HimeLib.SingletonMono<BombPool>
{
    public BombMessage Prefab_Bomb;
    void Start()
    {
        PunChatManager.instance.OnNewMessageComing += NewBomb;
    }

    void NewBomb(string msg){
        var b = Instantiate(Prefab_Bomb, transform);
        b.SetText(msg);
    }
}
