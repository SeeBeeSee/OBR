using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationHelperSpawner : MonoBehaviour
{
    public GameObject rotationHelperObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnHelpers(float startAngle, float angleAmount, float angleDuration, string direction, Transform warningHolder)
    {
        StartCoroutine(SpawnHelpersTimed(startAngle, angleAmount, angleDuration, direction, warningHolder));

        
    }

    IEnumerator SpawnHelpersTimed(float startAngle, float angleAmount, float angleDuration, string direction, Transform warningHolder)
    {
        var angleRate = 1 / ((720 / angleAmount) / angleDuration);
        var helperCount = 16 * angleRate;
        var start = 0;

        // unparent the warning holder so helpers don't move
        warningHolder.parent = null;

        // Negative angles give me headaches
        startAngle = 450 - startAngle;
        startAngle *= Mathf.Deg2Rad;

        // projecting clockwise, subtract
        if (direction == "R")
        {
            for (int i = start; i <= helperCount; i++)
            {
                // instantiate a helper at the next spot in the path
                var angle = startAngle - (i * (2 * Mathf.PI) * (angleAmount / 360) / helperCount);
                var x = Mathf.Cos(angle) * 4.5f;
                var z = Mathf.Sin(angle) * 4.5f;
                Vector3 helperPos = warningHolder.position + new Vector3(x, 0, z);
                var newSpawn = Instantiate(rotationHelperObj, helperPos, Quaternion.identity);
                // parent the helper to the warningHolder
                newSpawn.transform.parent = warningHolder;

                yield return new WaitForSeconds(0.4f / helperCount);
            }
        }
        // projecting counterclockwise, add
        else
        {
            for (int i = start; i <= helperCount; i++)
            {
                // instantiate a helper at the next spot in the path
                var angle = startAngle + (i * (2 * Mathf.PI) * (angleAmount / 360) / helperCount);
                var x = Mathf.Cos(angle) * 4.5f;
                var z = Mathf.Sin(angle) * 4.5f;
                Vector3 helperPos = warningHolder.position + new Vector3(x, 0, z);
                var newSpawn = Instantiate(rotationHelperObj, helperPos, Quaternion.identity);
                // parent the helper to the warningHolder
                newSpawn.transform.parent = warningHolder;

                yield return new WaitForSeconds(0.4f / helperCount);
            }
        }
    }
}
