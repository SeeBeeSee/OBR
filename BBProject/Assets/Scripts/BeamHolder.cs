using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayMaker;

public class BeamHolder : MonoBehaviour
{
    public GameObject spawn;
    public float spawnRadius;
    public int spawnCount;
    GameObject[] beams;
    PlayMakerFSM fsm;

    private void Awake()
    {
        // Setup before everything gets going
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        // Fun stuff from SonicBloom to address event duping
        SonicBloom.Koreo.Players.AudioVisor.EstimationWindowBufferCount = 2d;
        SonicBloom.Koreo.Players.AudioVisor.OverEstimationPercent = 0.5d;
    }

    // Start is called before the first frame update
    void Start()
    {

        beams = new GameObject[spawnCount];

        for (int i=0; i<spawnCount; i++)
        {
            var angle = i * (2*Mathf.PI) / spawnCount;
            var x = Mathf.Cos(angle) * spawnRadius;
            var z = Mathf.Sin(angle) * spawnRadius;
            Vector3 spawnPos = transform.position + new Vector3(x, 0, z);
            float degrees = (-angle * Mathf.Rad2Deg) - 90;
            Quaternion rotation = Quaternion.Euler(0, degrees, 0);
            var newSpawn = Instantiate(spawn, spawnPos, rotation);
            newSpawn.transform.parent = transform;
            beams[i] = newSpawn;
        }

        fsm = GetComponent<PlayMakerFSM>();
        fsm.FsmVariables.GetFsmArray("beams").Values = beams;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
