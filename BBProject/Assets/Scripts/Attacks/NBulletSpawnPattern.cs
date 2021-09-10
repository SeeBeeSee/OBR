using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class NBulletSpawnPattern : MonoBehaviour
{
    // Event string this SpawnPattern is listening for
    public string spawnTrigger;
    public List<Transform> bulletLocations;
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
        if (gameObject.activeSelf && bulletLocations.Count > 0)
        {
            foreach (Transform t in bulletLocations)
            {
                var newBullet = BulletPoolManager.instance.RequestSimpleBullet();
                if (newBullet != null)
                {
                    newBullet.transform.position = t.position;
                    //newBullet.transform.rotation = t.rotation;
                    var rot = t.eulerAngles;
                    newBullet.transform.rotation = Quaternion.Euler(rot);
                    Debug.Log(t.rotation.eulerAngles + " " + newBullet.transform.rotation.eulerAngles);
                }

            }
            fsm.SendEvent("Spawn");
        }
    }
}
