using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;

public class PlayerChar : MonoBehaviourPun
{
    public Transform namePosition;
    public MeshRenderer HeadView;
    public Animator playerAnimator;
    public string keyOfMove = "IsMove";
    public float moveSpeed = 1;

    public string playerName;
    Tweener currentMove;

    void Start()
    {
        gameObject.name = playerName = photonView.Owner.NickName;
        
        if(UINamePool.instance){
            UINamePool.instance.CreateName(playerName, namePosition);
        }

        if(!photonView.IsMine)
            return;

        CamLookAt.instance.targetCharacter = transform;
    }

    void MoveTo(Vector3 pos){
        if(currentMove != null){
            currentMove.Kill();
        }

        playerAnimator.SetBool(keyOfMove, true);
        //var direction = pos - transform.position;
        //transform.LookAt(pos);
        transform.DOLookAt(pos, 0.2f);

        currentMove = transform.DOMove(pos, moveSpeed).OnComplete(() => {
            playerAnimator.SetBool(keyOfMove, false);
        });
    }
    
    void Update()
    {
        if(!photonView.IsMine)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            var pos = Input.mousePosition;

            if(CamLookAt.instance.IsHitNPC())
                return;

            var target = CamLookAt.instance.GetMouseOnGround();
            if(target != Vector3.zero)
                MoveTo(target);
        }
    }
}
