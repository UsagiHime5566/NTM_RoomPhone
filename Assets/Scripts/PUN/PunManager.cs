using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PunManager : MonoBehaviourPunCallbacks
{
    // 玩家離開遊戲室時, 把他帶回到遊戲場入口
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        } 
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
}
