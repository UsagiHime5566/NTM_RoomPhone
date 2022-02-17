using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : HimeLib.SingletonMono<NetworkManager>
{
    [Header(@"API URL")]
    public string serverURL = "https://media.iottalktw.com";
    public string api_upload_Video = "/api/face/upload/video";
    public string api_upload_Image = "/api/face/upload/image";
    public string api_get_Video = "/api/face/video";
    public string api_get_Image = "/api/face/image";


    public System.Action<float> OnProgressUpdate;
    bool toAbort = false;

    public void StartUploadImage(System.Action callback){
        StartCoroutine(UploadImageProgress(callback));
    }

    public void StartUploadMedia(System.Action callback){
        StartCoroutine(UploadMediaProgress(callback));
    }

    IEnumerator UploadImageProgress(System.Action callback){

        string toUploadFileName = $"{PlayerManager.instance.uuid}.jpg";

        //Step1. 上傳檔案
        toAbort = false;
        yield return HttpPostFile(serverURL + api_upload_Video, RecordManager.instance.ImageFilePath, toUploadFileName);
        
        callback?.Invoke();
    }

    IEnumerator UploadMediaProgress(System.Action callback){

        string toUploadFileName = $"{PlayerManager.instance.uuid}.mp4";

        //Step1. 上傳檔案
        toAbort = false;
        yield return HttpPostFile(serverURL + api_upload_Video, RecordManager.instance.MediaFilePath, toUploadFileName);
        
        callback?.Invoke();
    }

    public void API_Other_UploadFile(string filePath, string fileID){
        StartCoroutine(HttpPostFile(serverURL + api_upload_Video, filePath, fileID));
    }

    public string API_GetImage(string fileID){
        return serverURL + api_get_Image + "/" + fileID;
    }

    public string API_GetMedia(string fileID){
        return serverURL + api_get_Video + "/" + fileID;
    }

    public IEnumerator HttpPostJSON(string url, string json, System.Action<string> callback)
    {
        // 這個方法會把json裡的文字編碼成url code , 例如 { 變成 %7B
        // var request = UnityWebRequest.Post(url, json);
        // request.SetRequestHeader("Content-Type", "application/json");

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError){
            Debug.Log("Network error has occured: " + request.GetResponseHeader(""));
        } else {
            Debug.Log("Success: " + request.downloadHandler.text);
            
            callback?.Invoke(request.downloadHandler.text);
        }

        // byte[] results = request.downloadHandler.data;
        // Debug.Log("Data: " + System.String.Join(" ", results));
    }

    public IEnumerator HttpPostFile(string url, string filePath, string fileName){

        if(string.IsNullOrEmpty(url) || string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(fileName)){
            Debug.LogError($"Post file param Error. {url} / {filePath} / {fileName}");
            yield break;
        }
        
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", File.ReadAllBytes(filePath), fileName);
        UnityWebRequest www = UnityWebRequest.Post(url, form);

        var asyncOp = www.SendWebRequest();
        while (!asyncOp.isDone)
        {
            if(toAbort){
                www.Abort();
                break;
            }
            OnProgressUpdate?.Invoke(asyncOp.progress);
            yield return null;
        }

        if (www.isNetworkError || www.isHttpError){
            Debug.Log(www.error);
        } else {
            Debug.Log("Form upload complete! >> :" + www.downloadHandler.text);
        }
    }

    public void AbortUploading(){
        toAbort = true;
    }
}
