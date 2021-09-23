using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamParamParser : MonoBehaviour
{
    PlayMakerFSM fsm;

    // Start is called before the first frame update
    void Start()
    {
        // For beam cannons with multiple FSMs, the one we want to modify should always be first.
        fsm = GetComponents<PlayMakerFSM>()[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ParseBeamParamString(string paramString)
    {
        // Expected string split char: ,

        // Expected string param format O-type:
        //  <type (O,B,R: string)>, <location (1-N: int)>, 
        //  <cannon base color (3 floats)>, <beam base color (3 floats)>,
        //  <beam emission color (3 floats)>, <cannon light color (3 floats)>

        // Expected string param format B-type:
        //  <type (O,B,R: string)>, <locations (int[])>,
        //  <cannon base color (3 floats)>, <beam base color (3 floats)>,
        //  <beam emission color (3 floats)>, <cannon light color (3 floats)>

        // Expected string param format R-type:
        //  <type (O,B,R: string)>, <location (1-N: int)>, 
        //  <rotation direction (L/R: string)>, <rotation duration in 8th notes (1-N: int)>, <rotation amount in degrees (1-N: float)>,
        //  <cannon base color (3 floats)>, <beam base color (3 floats)>,
        //  <beam emission color (3 floats)>, <cannon light color (3 floats)>

        // randomized colors for testing
        //var randomHSV = Random.ColorHSV();
        //var baseColor = Color.HSVToRGB(randomHSV.r, randomHSV.g, randomHSV.b);
        //randomHSV = Random.ColorHSV();
        //var beamBaseColor = Color.HSVToRGB(randomHSV.r, randomHSV.g, randomHSV.b);
        //randomHSV = Random.ColorHSV();
        //var beamEmissionColor = Color.HSVToRGB(randomHSV.r, randomHSV.g, randomHSV.b);
        //randomHSV = Random.ColorHSV();
        //var lightColor = Color.HSVToRGB(randomHSV.r, randomHSV.g, randomHSV.b);

        var paramList = paramString.Split(',');

        //foreach (string c in paramList) Debug.Log(c);



        // Use one-off type
        if (paramList[0] == "O")
        {
            if (paramList.Length > 2)
            {
                fsm.FsmVariables.GetFsmString("nextBeamType").Value = "O";
                // location stuff
                fsm.FsmVariables.GetFsmInt("nextBeamToFire").Value = int.Parse(paramList[1].ToString());

                // cannon and beam color stuff
                fsm.FsmVariables.GetFsmBool("setColors").Value = true;
                var baseColorValues = paramList[2].ToString().Split('-');
                var baseColor = new Color(
                    float.Parse(baseColorValues[0].ToString()) / 255,
                    float.Parse(baseColorValues[1].ToString()) / 255,
                    float.Parse(baseColorValues[2].ToString()) / 255
                    );

                var beamBaseColorValues = paramList[3].ToString().Split('-');
                var beamBaseColor = new Color(
                    float.Parse(beamBaseColorValues[0].ToString()) / 255,
                    float.Parse(beamBaseColorValues[1].ToString()) / 255,
                    float.Parse(beamBaseColorValues[2].ToString()) / 255
                    );

                float intensity = Mathf.Pow(2, 0.5f);

                var beamEmissionColorValues = paramList[4].ToString().Split('-');
                var beamEmissionColor = new Color(
                    float.Parse(beamEmissionColorValues[0].ToString()) * intensity / 255,
                    float.Parse(beamEmissionColorValues[1].ToString()) * intensity / 255,
                    float.Parse(beamEmissionColorValues[2].ToString()) * intensity / 255
                    );

                var lightColorValues = paramList[4].ToString().Split('-');
                var lightColor = new Color(
                    float.Parse(lightColorValues[0].ToString()) / 255,
                    float.Parse(lightColorValues[1].ToString()) / 255,
                    float.Parse(lightColorValues[2].ToString()) / 255
                    );

                fsm.FsmVariables.GetFsmColor("nextBaseColor").Value = baseColor;
                fsm.FsmVariables.GetFsmColor("nextBeamBaseColor").Value = beamBaseColor;
                fsm.FsmVariables.GetFsmColor("nextBeamEmissionColor").Value = beamEmissionColor;
                fsm.FsmVariables.GetFsmColor("nextLightColor").Value = lightColor;
            }
            
            // use fallback colors on prefab
            else
            {
                fsm.FsmVariables.GetFsmString("nextBeamType").Value = "O";
                fsm.FsmVariables.GetFsmInt("nextBeamToFire").Value = int.Parse(paramList[1].ToString());
                fsm.FsmVariables.GetFsmBool("setColors").Value = false;
            }

        }

        else if (paramList[0] == "B")
        {
            fsm.FsmVariables.GetFsmString("nextBeamType").Value = "B";

            // With colors (not yet)
            if (paramList.Length > 2)
            {

            }

            // Fallback colors
            else if (paramList.Length == 2)
            {
                var cannonLocationsRaw = paramList[1].Split('-');
                foreach(string s in cannonLocationsRaw) Debug.Log(s);
                var beamLocations = new int[cannonLocationsRaw.Length];
                var fsmInts = fsm.FsmVariables.GetFsmArray("nextBTypesToFire");
                for (int i=0; i<cannonLocationsRaw.Length; i++)
                {
                    beamLocations[i] = int.Parse(cannonLocationsRaw[i]);
                    //Debug.Log(beamLocations);
                    fsmInts.InsertItem(beamLocations[i], i);
                }
                fsmInts.SaveChanges();
                fsm.FsmVariables.GetFsmBool("setColors").Value = false;
            }
        }
        

        
        else if (paramList[0] == "R")
        {
            // Use one-off type
            if (paramList.Length > 5)
            {
                fsm.FsmVariables.GetFsmString("nextBeamType").Value = "R";
                // location stuff
                fsm.FsmVariables.GetFsmInt("nextBeamToFire").Value = int.Parse(paramList[1].ToString());

                // rotation stuff
                fsm.FsmVariables.GetFsmString("nextRotationDirection").Value = paramList[2].ToString();
                fsm.FsmVariables.GetFsmFloat("nextRotationDuration").Value = float.Parse(paramList[3].ToString())
                    * fsm.FsmVariables.GetFsmFloat("eighthNoteDuration").Value;
                fsm.FsmVariables.GetFsmFloat("nextRotationAmount").Value = float.Parse(paramList[4].ToString());

                // cannon and beam color stuff
                fsm.FsmVariables.GetFsmBool("setColors").Value = true;
                var baseColorValues = paramList[5].ToString().Split('-');
                var baseColor = new Color(
                    float.Parse(baseColorValues[0].ToString()) / 255,
                    float.Parse(baseColorValues[1].ToString()) / 255,
                    float.Parse(baseColorValues[2].ToString()) / 255
                    );

                var beamBaseColorValues = paramList[6].ToString().Split('-');
                var beamBaseColor = new Color(
                    float.Parse(beamBaseColorValues[0].ToString()) / 255,
                    float.Parse(beamBaseColorValues[1].ToString()) / 255,
                    float.Parse(beamBaseColorValues[2].ToString()) / 255
                    );

                float intensity = Mathf.Pow(2, 0.5f);

                var beamEmissionColorValues = paramList[7].ToString().Split('-');
                var beamEmissionColor = new Color(
                    float.Parse(beamEmissionColorValues[0].ToString()) * intensity / 255,
                    float.Parse(beamEmissionColorValues[1].ToString()) * intensity / 255,
                    float.Parse(beamEmissionColorValues[2].ToString()) * intensity / 255
                    );

                var lightColorValues = paramList[8].ToString().Split('-');
                var lightColor = new Color(
                    float.Parse(lightColorValues[0].ToString()) / 255,
                    float.Parse(lightColorValues[1].ToString()) / 255,
                    float.Parse(lightColorValues[2].ToString()) / 255
                    );

                fsm.FsmVariables.GetFsmColor("nextBaseColor").Value = baseColor;
                fsm.FsmVariables.GetFsmColor("nextBeamBaseColor").Value = beamBaseColor;
                fsm.FsmVariables.GetFsmColor("nextBeamEmissionColor").Value = beamEmissionColor;
                fsm.FsmVariables.GetFsmColor("nextLightColor").Value = lightColor;
            }
            else 
            {
                fsm.FsmVariables.GetFsmString("nextBeamType").Value = "R";
                fsm.FsmVariables.GetFsmInt("nextBeamToFire").Value = int.Parse(paramList[1].ToString());
                fsm.FsmVariables.GetFsmString("nextRotationDirection").Value = paramList[2].ToString();
                fsm.FsmVariables.GetFsmFloat("nextRotationDuration").Value = float.Parse(paramList[3].ToString())
                    * fsm.FsmVariables.GetFsmFloat("eighthNoteDuration").Value;
                fsm.FsmVariables.GetFsmFloat("nextRotationAmount").Value = float.Parse(paramList[4].ToString());
                fsm.FsmVariables.GetFsmBool("setColors").Value = false;
            }
        }

    }
}
