using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using SonicBloom.Koreo;

public class SpawnPattern : MonoBehaviour
{
    // Event string this SpawnPattern is listening for
    public string spawnTrigger;
    public Transform singleBulletT;
    PlayMakerFSM fsm;

    // Start is called before the first frame update
    void Start()
    {
        fsm = GetComponent<PlayMakerFSM>();
        Koreographer.Instance.RegisterForEvents(spawnTrigger, Spawn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawn(KoreographyEvent e)
    {
        if (gameObject.activeSelf)
        {
            var newBullet = BulletPoolManager.instance.RequestSimpleBullet();
            if (newBullet != null)
            {
                newBullet.transform.position = singleBulletT.position;
                newBullet.transform.rotation = singleBulletT.rotation;
            }

            fsm.SendEvent("Spawn");
        }
    }
}
