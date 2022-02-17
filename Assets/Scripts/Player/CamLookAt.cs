using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamLookAt : HimeLib.SingletonMono<CamLookAt>
{
    public Camera targetCamera;
    public Transform targetCharacter;

    public MeshCollider targetGround;
    public float speed = 15;

    
    protected override void OnSingletonAwake(){
        if(targetCamera == null)
            targetCamera = GetComponent<Camera>();
    }

    void Start()
    {

    }
    


    // Update is called once per frame
    void Update()
    {
        if(targetCharacter == null)
            return;

        Vector3 targetDir = targetCharacter.position - transform.position;
        targetDir.y = 0.0f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDir), Time.deltaTime * speed);
    }

    public bool IsHitNPC(){
        Ray mouseray = targetCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        if(Physics.Raycast(mouseray, out hitData, 100, LayerMask.GetMask("GameNPC"))){
            return true;
        }
        if(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != null)
            return true;

        return false;
    }

    public Vector3 GetMouseOnGround()
    {// calculates the intersection of a ray through the mouse pointer with a static x/z plane for example for movement etc, source: http://unifycommunity.com/wiki/index.php?title=Click_To_Move
        Ray mouseray = targetCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        if (targetGround.Raycast(mouseray, out hitData, 100))
        {// check for the intersection point between ray and plane
            return new Vector3(hitData.point.x, 0, hitData.point.z);
        }
        Debug.Log("ExtensionMethods_Camera.MouseOnPlane: plane is behind camera or ray is parallel to plane! " + hitData);       // both are parallel or plane is behind camera so write a log and return zero vector
        return Vector3.zero;
    }
}
