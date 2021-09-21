using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamParamParser : MonoBehaviour
{
    PlayMakerFSM fsm;

    // Start is called before the first frame update
    void Start()
    {
        fsm = GetComponent<PlayMakerFSM>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ParseBeamParamString(string paramString)
    {
        // Expected string split char: ,
        // Expected string param format:
        //  <type (O,B,R: string)>, <location (1-N: int)>, 
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





        if (paramList.Length > 2)
        {
            // type stuff for paramList[0]

            // location stuff
            fsm.FsmVariables.GetFsmInt("nextBeamToFire").Value = int.Parse(paramList[1].ToString());

            fsm.FsmVariables.GetFsmBool("setColors").Value = true;
            // cannon and beam color stuff
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
            //fsm.FsmVariables.GetFsmColor("nextBeamEmissionColor").Value = beamEmissionColor;
            fsm.FsmVariables.GetFsmColor("nextLightColor").Value = lightColor;
        }
        else
        {
            fsm.FsmVariables.GetFsmInt("nextBeamToFire").Value = int.Parse(paramList[0].ToString());
            fsm.FsmVariables.GetFsmBool("setColors").Value = false;
        }

    }
}
