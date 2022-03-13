using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

/// <summary>
/// Simple singleton used for carrying selected song tracks/audio files from scene to scene.
/// Useful for starting new songs and repeating current ones.
/// Only holds a single song at a time (for now).
/// </summary>

public class SelectedSongHolder : MonoBehaviour
{
    public static SelectedSongHolder instance { get; private set; }

    AudioClip songFile;
    public Koreography songKoreo;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StoreKoreography(Koreography koreo)
    {
        songKoreo = koreo;
    }

    public Koreography LoadKoreography()
    {
        return songKoreo;
    }

    public void ClearKoreography()
    {
        songKoreo = null;
    }
}
