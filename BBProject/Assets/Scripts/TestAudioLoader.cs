using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TestAudioLoader : MonoBehaviour
{
    public string songName;

    public AudioSource source;
    public AudioClip clip;
    string path;

    // Start is called before the first frame update
    void Start()
    {
        //Load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Load()
    {
        path = "file://" + Application.dataPath + "/StreamingAssets/Tracks/This Time/";
        StartCoroutine(LoadAudio());
    }

    IEnumerator LoadAudio()
    {
        // Get WWW request for audio file
        WWW request = GetAudioFromFile(path, songName);
        yield return request;

        Debug.Log(request);

        clip = request.GetAudioClip();
        //clip.name = songName;

        PlayAudioFile();
    }

    void PlayAudioFile()
    {
        source.clip = clip;
        source.Play();
    }

    WWW GetAudioFromFile(string path, string filename)
    {
        string load = string.Format(path + filename);
        WWW request = new WWW(load);
        return request;
    }
}
