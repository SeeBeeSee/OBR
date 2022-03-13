using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo.Players;

public class LoadKoreoFromHolder : MonoBehaviour
{
    public SimpleMusicPlayer simp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadKoreo()
    {
        if (SelectedSongHolder.instance != null)
        {
            var loadedKoreo = SelectedSongHolder.instance.LoadKoreography();

            if (loadedKoreo != null)
            {
                simp.LoadSong(loadedKoreo, 0, false);
            }
            else Debug.LogError("SongHolder found, but it didn't contain a song! Please report this!");
        }

        else
        {
            Debug.LogError("Could not find a SelectedSongHolder in the scene. Please report this!");
        }
    }
}
