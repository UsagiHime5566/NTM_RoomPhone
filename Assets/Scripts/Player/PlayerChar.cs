using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using DG.Tweening;
using Photon.Pun;
using UnityEngine.Networking;
using System.IO;

public class PlayerChar : MonoBehaviourPun
{
    public Transform namePosition;
    public MeshRenderer [] HeadView;
    public VideoPlayer videoPlayer;

    [Header("Anim")]
    public Animator playerAnimator;
    public string keyOfMove = "IsMove";
    public float moveSpeed = 1;

    [Header("Runtime")]
    public string playerName;
    public string uuid;
    Tweener currentMove;

    RenderTexture rt;

    void Awake(){
        videoPlayer.prepareCompleted += PrepareFinished;
    }

    public void PlayFace(){
        string url = NetworkManager.instance.API_GetMedia($"{uuid}.mp4");
        StartCoroutine(this.LoadVideoFromThisURL(url));
        Debug.Log($"Start Play Video. {url}");
    }

    void PrepareFinished(VideoPlayer vp){
        vp.Play();
    }

    async void SetTexture(){
        await System.Threading.Tasks.Task.Delay(5000);
        
        Debug.Log("assign tex");
    }

    void Start()
    {
        gameObject.name = playerName = photonView.Owner.NickName;
        uuid = (string)photonView.Owner.CustomProperties["UUID"];
        
        if(UINamePool.instance){
            UINamePool.instance.CreateName(playerName, namePosition);
        }

        if(NetworkManager.instance)
            PlayFace();

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


    private IEnumerator LoadVideoFromThisURL(string _url)
    {
        string _pathToFile = Path.Combine (PlayerManager.instance.PathFolder, $"{uuid}.mp4");

        if(IsPathFileExist(_pathToFile)){
            PrepareThisURLInVideo (_pathToFile);
            yield break;
        }

        UnityWebRequest _videoRequest = UnityWebRequest.Get (_url);

        var asyncOp = _videoRequest.SendWebRequest();

        while(!asyncOp.isDone){
            yield return null;
        }

        if (_videoRequest.isDone == false || _videoRequest.error != null)
        {
            Debug.Log ("Request >> " + _videoRequest.error );
            yield break;
        }

        Debug.Log ("Video Done >> " + _videoRequest.isDone);

        byte[] _videoBytes = _videoRequest.downloadHandler.data;

        File.WriteAllBytes (_pathToFile, _videoBytes);
        
        Debug.Log ($"Save video to {_pathToFile}");

        PrepareThisURLInVideo (_pathToFile);
        yield return null;
    }


    void PrepareThisURLInVideo(string _url)
    {
        videoPlayer.source = UnityEngine.Video.VideoSource.Url;
        videoPlayer.url = _url;
        videoPlayer.Prepare();

        RenderTexture rt = new RenderTexture(216, 384, 0);
        videoPlayer.targetTexture = rt;

        foreach (var item in HeadView)
        {
            item.material.mainTexture = rt;
        }
    }

    bool IsPathFileExist(string path){
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Debug.LogWarning($"Directory does not exist : {path}, try to download.");
            return false;
        }

        if (!File.Exists(path))
        {
            Debug.Log($"File does not exist : {path}, try to download.");
            return false;
        }

        Debug.Log($"Read video from catch : {path}");
        return true;
    }
}
