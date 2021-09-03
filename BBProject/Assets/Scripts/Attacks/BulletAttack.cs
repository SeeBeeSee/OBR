using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using SonicBloom.Koreo;

public class BulletAttack : MonoBehaviour
{
    PlayMakerFSM fsm;

    // Start is called before the first frame update
    void Start()
    {
        fsm = GetComponent<PlayMakerFSM>();
        Koreographer.Instance.RegisterForEvents("BulletAttackInit", FireBulletAttackInit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FireBulletAttackInit(KoreographyEvent e)
    {
        if (e.Payload != null)
        {
            var eventText = (e.Payload as TextPayload).TextVal;
            //fsm.SendEvent(eventText);
            Debug.Log(eventText);
            ParseBulletAttackPayload(eventText);
        }
        else
        {
            Debug.LogError("Received empty bullet attack event.");
        }
    }

    void ParseBulletAttackPayload(string payload)
    {
        var payloadSplit = payload.Split('#');
        if (payloadSplit.Length != 7)
        {
            Debug.LogError("Expected 7 params for bullet attack, got " + payloadSplit.Length);
        }
        else
        {
            //foreach(string s in payloadSplit) Debug.Log(s);

            // Attack Type - controls which SpawnPattern gets activated
            var attackType = payloadSplit[0];
            // Movement Type
            var moveType = payloadSplit[1];
            // Start target
            var startTarget = payloadSplit[2];
            var startCoords = startTarget.Split(',');
            var startVec = new Vector3(float.Parse(startCoords[0]), 0, float.Parse(startCoords[1]));
            // End target
            var endTarget = payloadSplit[3];
            var endCoords = endTarget.Split(',');
            var endVec = new Vector3(float.Parse(endCoords[0]), 0, float.Parse(endCoords[1]));
            // Start rotation
            var startRot = payloadSplit[4];
            // End rotation/rotate by
            var endRotBy = payloadSplit[5];
            // Duration of attack - determines length of movement/deactivation time for SpawnPattern
            var attackDuration = payloadSplit[6];

            // Set control variables on FSM
           
            fsm.FsmVariables.GetFsmString("moveType").Value = moveType;
            fsm.FsmVariables.GetFsmVector3("startXZ").Value = startVec;
            fsm.FsmVariables.GetFsmVector3("endXZ").Value = endVec;
            fsm.FsmVariables.GetFsmVector3("startEulerangles").Value = new Vector3(0, float.Parse(startRot), 0);
            fsm.FsmVariables.GetFsmVector3("endEulerangles").Value = new Vector3(0, float.Parse(endRotBy), 0);
            fsm.FsmVariables.GetFsmFloat("rotBy").Value = float.Parse(endRotBy);
            fsm.FsmVariables.GetFsmFloat("attackDuration").Value = float.Parse(attackDuration);

            fsm.SendEvent(attackType);
        }
    }
}
