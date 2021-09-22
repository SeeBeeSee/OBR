using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using PlayMaker;

public class MasterBeamParams : MonoBehaviour
{
    public Koreographer k;
    public float warning8ths;
    float eighthNoteDuration;
    PlayMakerFSM fsm;

    // Start is called before the first frame update
    void Start()
    {
        eighthNoteDuration = (float)(30f / k.GetMusicBPM());
        fsm = GetComponent<PlayMakerFSM>();
        // HARDCODED
        fsm.FsmVariables.GetFsmFloat("eighthNoteDuration").Value = eighthNoteDuration;
        fsm.FsmVariables.GetFsmFloat("warningTime").Value = eighthNoteDuration * warning8ths;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
