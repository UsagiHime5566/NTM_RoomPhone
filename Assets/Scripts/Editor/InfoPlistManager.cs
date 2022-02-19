using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using System.IO;
using System.Collections.Generic;

public class InfoPlistManager : MonoBehaviour
{

#if UNITY_IOS
    [PostProcessBuild]
    static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    {
        // Read plist
        var plistPath = Path.Combine(path, "Info.plist");
        var plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        // Update value
        PlistElementDict rootDict = plist.root;
        rootDict.SetString("NSPhotoLibraryUsageDescription", "開啟後，此APP可以將目前的畫面保存到你的相簿之中");
	    rootDict.SetString("NSPhotoLibraryAddUsageDescription", "開啟後，此APP可以將目前的畫面保存到你的相簿之中");
        //rootDict.SetString("NSLocationAlwaysAndWhenInUseUsageDescription", "開啟後，APP中的地圖將定位您的位置，並引導您至此APP中的AR觀看點");
        //rootDict.SetString("NSLocationWhenInUseUsageDescription", "開啟後，APP中的地圖將定位您的位置，並引導您至此APP中的AR觀看點");
        rootDict.SetString("NSCameraUsageDescription", "開啟後，您才能於APP中正常使用相機錄製影片。");
        rootDict.SetString("NSMicrophoneUsageDescription", "開啟後，能夠在APP中同時錄製聲音。");
        
        var rootDicVal = rootDict.values;
        rootDicVal.Remove("UIApplicationExitsOnSuspend");

        // Write plist
        File.WriteAllText(plistPath, plist.WriteToString());
    }
#endif
}