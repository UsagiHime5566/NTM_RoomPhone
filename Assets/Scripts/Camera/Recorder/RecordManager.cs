using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net.NetworkInformation;

public class RecordManager : HimeLib.SingletonMono<RecordManager>
{
    public RecordHelper recordHelper;

    [Header(@"Uploading")]
    public string ImageFilePath;
    public string MediaFilePath;
}
