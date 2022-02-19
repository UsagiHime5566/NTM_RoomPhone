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
        rootDict.SetString("NSPhotoLibraryUsageDescription", "該APP會將其他線上使用者的頭像資源暫存至儲存裝置中，以避免再次下載相同資源");
	    rootDict.SetString("NSPhotoLibraryAddUsageDescription", "該APP會將其他線上使用者的頭像資源暫存至儲存裝置中，以避免再次下載相同資源");
        //rootDict.SetString("NSLocationAlwaysAndWhenInUseUsageDescription", "開啟後，APP中的地圖將定位您的位置，並引導您至此APP中的AR觀看點");
        //rootDict.SetString("NSLocationWhenInUseUsageDescription", "開啟後，APP中的地圖將定位您的位置，並引導您至此APP中的AR觀看點");
        rootDict.SetString("NSCameraUsageDescription", "該APP初次使用時，會錄製您的個人影片，以此來客製化您的虛擬角色");
        rootDict.SetString("NSMicrophoneUsageDescription", "該APP初次使用時，會錄製您的聲音，以此來客製化您的虛擬角色");
        
        var rootDicVal = rootDict.values;
        rootDicVal.Remove("UIApplicationExitsOnSuspend");

        // Write plist
        File.WriteAllText(plistPath, plist.WriteToString());
    }
#endif
}