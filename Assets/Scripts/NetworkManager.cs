using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : HimeLib.SingletonMono<NetworkManager>
{
    [Header(@"API URL")]
    public string serverURL = "https://media.iottalktw.com";
    public string api_getUploadAccess = "/api/getUploadAccess";
    public string api_doUpload = "/api/upload";
    public string api_getBoxList = "/api/getBoxList";
    public string api_getMedia = "/api/media";


    [Header(@"Runtimes")]
    public string currentUUID;
    public string uploadUUID;
    public PoiBoxList Poi_Box;

    public System.Action<float> OnProgressUpdate;
    public System.Action<string,double,double,string,string,string> OnNewPosUploaded;
    bool toAbort = false;

    void Start()
    {
        currentUUID = System.Guid.NewGuid().ToString().Replace("-", "");
    }

    public void StartUploadMedia(System.Action callback){
        StartCoroutine(UploadMediaProgress(callback));
    }

    IEnumerator UploadMediaProgress(System.Action callback){

        DataUUID boxData = null;
        string toUploadFileName = "";

        //Step1. 取得當前位置座標
        // Vector2 gps = OnlineMapsLocationService.instance.position;
        // Debug.Log($"current gps : {gps}");

        //Step2. 取得座標對應的uuid
        // yield return HttpPostJSON(serverURL + api_getUploadAccess, GetLocationJSON(gps.y, gps.x), json => {
        //     boxData = GetDataUUID(json);
        //     toUploadFileName = $"{boxData.uuid}.mp4";
        //     Debug.Log($"Get fileName: {toUploadFileName}");
        // });

        //Step3. 上傳檔案, 並取名uuid
        toAbort = false;
        yield return HttpPostFile(serverURL + api_doUpload, RecordManager.instance.FilePath, toUploadFileName);

        //Step4. 上傳座標, 標題資訊至Google  => 地點名稱	座標 Lat	座標 Lon	標題	內文	media
        //OnlineDataManager.instance.PostDataToSheet(boxData.uuid.Substring(0, 4), boxData.latitude.ToString(), boxData.longitude.ToString(), RecordManager.instance.ComingTitle, RecordManager.instance.ComingContent, toUploadFileName);
        
        OnNewPosUploaded?.Invoke(boxData.uuid.Substring(0, 4), boxData.latitude, boxData.longitude, RecordManager.instance.ComingTitle, RecordManager.instance.ComingContent, toUploadFileName);
        callback?.Invoke();
    }

    public void API_UploadAccess(double lat, double lon)
    {
        StartCoroutine(HttpPostJSON(serverURL + api_getUploadAccess, GetLocationJSON(lat, lon), json => {
            DataUUID data = GetDataUUID(json);
            uploadUUID = data.uuid;
        }));
    }

    public void API_UploadFile(string filePath, string fileID){
        StartCoroutine(HttpPostFile(serverURL + api_doUpload, filePath, fileID));
    }

    public void API_GetBoxList(double lat, double lon, System.Action<PoiBoxList> callback)
    {
        StartCoroutine(HttpPostJSON(serverURL + api_getBoxList, GetLocationUUIDJSON(lat, lon, currentUUID), json => {
            try {
                var fullJson = "{\"box\":" + json + "}";
                PoiBoxList box = JsonUtility.FromJson<PoiBoxList>(fullJson);
                Poi_Box = box;

                callback?.Invoke(box);

            } catch(System.Exception e){
                Debug.Log(e.Message);
            }
        }));
    }

    public string API_GetFileUrl(string fileID){
        return serverURL + api_getMedia + "/" + fileID;
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
            Debug.LogError("Post file param Error.");
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

    public string GetLocationJSON(double lat, double lon)
    {
        LocationJSON json = new LocationJSON() { latitude = lat, longitude = lon };
        return JsonUtility.ToJson(json);
    }

    public string GetLocationUUIDJSON(double lat, double lon, string uuid)
    {
        LocationUUIDJSON json = new LocationUUIDJSON() { latitude = lat, longitude = lon, uid = uuid };
        return JsonUtility.ToJson(json);
    }

    public DataUUID GetDataUUID(string json){
        return JsonUtility.FromJson<DataUUID>(json);
    }

    [System.Serializable]
    public class LocationJSON
    {
        public double latitude;
        public double longitude;
    }

    [System.Serializable]
    public class LocationUUIDJSON
    {
        public double latitude;
        public double longitude;
        public string uid;
    }

    [System.Serializable]
    public class DataUUID
    {
        public string expire;
        public double latitude;
        public double longitude;
        public string timestamp;
        public string uuid;
    }

    [System.Serializable]
    public class PoiBoxList
    {
        public List<PoiBox> box;
    }

    [System.Serializable]
    public class PoiBox
    {
        public int id;
        public double latitude;
        public double longitude;
        public string timestamp;
        public string media_filename;
    }
}
