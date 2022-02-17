using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ideafixxxer.CsvParser;
using System.Linq;
using UnityEngine.Networking;

public class RefManager : HimeLib.SingletonMono<RefManager>
{
    public Action OnAppInfosDownloaded;
    public Action<List<string>> OnLyricsDownloaded;

    public string webService;
    public string sheetID;
    public string Infos_PageID;


    public int UseLyric { get => useLyric; set {
        useLyric = value;
        LyricInvoke();
    }}

    public List<string> lyric_zhtw;
    public List<string> lyric_ja;
    int useLyric = 0;

    void LyricInvoke(){
        if(useLyric == 0)
            OnLyricsDownloaded?.Invoke(lyric_zhtw);
        if(useLyric == 1)
            OnLyricsDownloaded?.Invoke(lyric_ja);
    }

    // [ContextMenu("Hit")]
    // void SetContentA(){
    //     for (int i = 0; i < 20; i++)
    //     {
    //         lyric_zhtw.Add($"展覽寶物_{i+1}");
    //         lyric_ja.Add($"宝物_{i+1}");
    //     }
    // }

    IEnumerator Start()
    {
        // while(CheckIntenetConnection.instance.InternetStats == false){
        //     yield return null;
        // }

        yield return null;

        DownloadManager.GoogleGetCSV(GetInfos, webService, sheetID, Infos_PageID);
    }

    public void GetInfos(string csvFile){
        //讀入 CSV 檔案，使其分為 string 二維陣列
        CsvParser csvParser = new CsvParser();
        string[][] csvTable = csvParser.Parse(csvFile);

        if(csvTable.Length == 0 || csvTable[0].Length == 0){
            Debug.LogError("Online info is error format");
            return;
        }

        string serverUrl = csvTable[1][0];
        string ver = csvTable[1][1];
        string about = csvTable[1][2];
        string contact = csvTable[1][3];
        string staff = csvTable[1][4];
        string initPosition = csvTable[1][5];
        string api_getUploadAccess = csvTable[1][6];
        string api_doUpload = csvTable[1][7];
        string api_getBoxList = csvTable[1][8];
        string api_getMedia = csvTable[1][9];


        NetworkManager.instance.serverURL = serverUrl;
        NetworkManager.instance.api_getUploadAccess = api_getUploadAccess;
        NetworkManager.instance.api_doUpload = api_doUpload;
        NetworkManager.instance.api_getBoxList = api_getBoxList;
        NetworkManager.instance.api_getMedia = api_getMedia;
        Debug.Log($"Use Server URL : {serverUrl} , with API: {api_getUploadAccess} , {api_doUpload} , {api_getBoxList} , {api_getMedia}");

        //AboutMeLayout.instance.UpdateAboutMe(about_title, about_content);
        OnAppInfosDownloaded?.Invoke();
    }

    IEnumerator PingConnect()
    {
        bool result = false;
        WaitForSeconds wait = new WaitForSeconds(5);
        while (!result)
        {
            UnityWebRequest request = new UnityWebRequest("http://google.com");
            yield return request.SendWebRequest();
            if (request.error != null)
            {
                Debug.Log("Have no Internet, retry after 5 seconds...");
            }
            else
            {
                result = true;
            }
            yield return wait;
        }

        Debug.Log("Network access success.");
    }
}
