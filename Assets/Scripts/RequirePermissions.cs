using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_2018_3 && UNITY_ANDROID
using UnityEngine.Android;
#endif

public class RequirePermissions : MonoBehaviour
{
    void JustForPermission(){
        WebCamTexture webCam = new WebCamTexture();
        webCam.Play();
    }
    
    IEnumerator Start(){

        #if UNITY_2018_3_OR_NEWER && UNITY_ANDROID
        while(true){
            int totalRequest = 0;

            if(!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.ExternalStorageWrite)){
                UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.ExternalStorageWrite);
            } else totalRequest++;
            if(!UnityEngine.Android.Permission.HasUserAuthorizedPermission("android.permission.INTERNET")){
                UnityEngine.Android.Permission.RequestUserPermission("android.permission.INTERNET");
            } else totalRequest++;
            if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Camera)) {
                UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.Camera);
            } else totalRequest++;

            yield return new WaitForSeconds(1.0f);

            if(totalRequest >= 3)
                break;
        }
        #endif
        yield return null;
    }
}