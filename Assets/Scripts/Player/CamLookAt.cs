using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CamLookAt : HimeLib.SingletonMono<CamLookAt>
{
    public Transform targetCharacter;


    public MeshCollider targetGround;
    public float speed = 15;

    Camera selfCamera;
    protected override void OnSingletonAwake(){
        selfCamera = GetComponent<Camera>();
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
        Ray mouseray = selfCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        if(Physics.Raycast(mouseray, out hitData, 100, LayerMask.GetMask("GameNPC"))){
            return true;
        }
        return false;
    }

    public Vector3 GetMouseOnGround()
    {// calculates the intersection of a ray through the mouse pointer with a static x/z plane for example for movement etc, source: http://unifycommunity.com/wiki/index.php?title=Click_To_Move
        Ray mouseray = selfCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        if (targetGround.Raycast(mouseray, out hitData, 100))
        {// check for the intersection point between ray and plane
            return hitData.point;
        }
        Debug.Log("ExtensionMethods_Camera.MouseOnPlane: plane is behind camera or ray is parallel to plane! " + hitData);       // both are parallel or plane is behind camera so write a log and return zero vector
        return Vector3.zero;
    }
}
