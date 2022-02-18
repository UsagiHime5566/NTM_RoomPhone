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
    public string Lyric_pageID;
    public string Post_pageID;


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

        PunChatManager.instance.OnNewMessageComing += PostMessageToGoogle;

        DownloadManager.GoogleGetCSV(ImportPOIData, webService, sheetID, Lyric_pageID);

        yield return new WaitForSeconds(2.0f);

        DownloadManager.GoogleGetCSV(GetInfos, webService, sheetID, Infos_PageID);
    }

    public void ImportPOIData(string csvFile)
    {
        //讀入 CSV 檔案，使其分為 string 二維陣列
        CsvParser csvParser = new CsvParser();
        string[][] csvTable = csvParser.Parse(csvFile);

        lyric_zhtw = new List<string>();
        lyric_ja = new List<string>();
        for (int i = 1; i < csvTable.Length; i++)
        {
            string index = csvTable[i][(int)CSVIndex.INDEX];
            string zh = csvTable[i][(int)CSVIndex.ZHTW];
            string ja = csvTable[i][(int)CSVIndex.JA];

            lyric_zhtw.Add(zh);
            lyric_ja.Add(ja);
        }

        LyricInvoke();
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
        string api_upload_video = csvTable[1][5];
        string api_upload_image = csvTable[1][6];
        string api_get_video = csvTable[1][7];
        string api_get_image = csvTable[1][8];


        NetworkManager.instance.serverURL = serverUrl;
        NetworkManager.instance.api_upload_Video = api_upload_video;
        NetworkManager.instance.api_upload_Image = api_upload_image;
        NetworkManager.instance.api_get_Video = api_get_video;
        NetworkManager.instance.api_get_Image = api_get_image;
        Debug.Log($"Use Server URL : {serverUrl} , with API: {api_upload_video} , {api_upload_image} , {api_get_video} , {api_get_image}");

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


    void PostMessageToGoogle(string msg){
        DownloadManager.GoogleWriteLastLine(webService, sheetID, Post_pageID, DateTime.Now.ToString(), msg);
    }

    public enum CSVIndex
    {
        INDEX = 0,
        ZHTW = 1,
        JA = 2,
    }
}
