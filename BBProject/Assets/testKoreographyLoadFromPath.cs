using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;

public class testKoreographyLoadFromPath : MonoBehaviour
{
    // Start is called before the first frame update

    public string koreoPath;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadKoreoFromResources(SimpleMusicPlayer musicPlayer)
    {
        Debug.Log(koreoPath);
        var track = Resources.Load(koreoPath);
        if (track != null && track.GetType() == typeof(Koreography))
        {
            Debug.Log(track.GetType());
            musicPlayer.LoadSong((Koreography)track, 0, false);
        }
        else Debug.LogError("Invalid path: " + koreoPath);
    }
}
