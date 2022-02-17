using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    public NpcObject Prefab_Npc;
    public List<Transform> NpcPositions;

    public Transform npcPool;

    int totalIndex;
    int lastHour = -1;
    WaitForSeconds wait = new WaitForSeconds(10);
    void Start()
    {
        totalIndex = NpcPositions.Count;
        StartCoroutine(NpcSet());
    }

    IEnumerator NpcSet(){
        while(true){
            var time = System.DateTime.UtcNow;

            if(lastHour != time.Hour){
                CreateNewNpcs(time);
            }

            yield return wait;
        }
    }

    void CreateNewNpcs(System.DateTime time){
        Debug.Log("Create new Npcs!");

        foreach (Transform item in npcPool)
        {
            Destroy(item.gameObject);
        }

        lastHour = time.Hour;
        int createdIndex1 = time.Hour % totalIndex;
        int createdIndex2 = (time.Hour + 4) % totalIndex;
        int createdIndex3 = (time.Hour + 7) % totalIndex;

        var npc1 = Instantiate(Prefab_Npc, NpcPositions[createdIndex1].position, Quaternion.identity, npcPool);
        var npc2 = Instantiate(Prefab_Npc, NpcPositions[createdIndex2].position, Quaternion.identity, npcPool);
        var npc3 = Instantiate(Prefab_Npc, NpcPositions[createdIndex3].position, Quaternion.identity, npcPool);

        npc1.bookIndex = createdIndex1;
        npc2.bookIndex = createdIndex2;
        npc3.bookIndex = createdIndex3;

        npc1.transform.LookAt(Vector3.zero);
        npc2.transform.LookAt(Vector3.zero);
        npc3.transform.LookAt(Vector3.zero);
    }
}
