using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;
using HutongGames.PlayMaker;

//
//
//

[Serializable]
public class ChartData
{
    public string track_title;
    public string track_artist;
    public string track;
}

public class KoreoJSON : MonoBehaviour
{
    public Koreography koreoExportEditor;
    public KoreographyTrack koreoTrackExportEditor;
    public string koreoExportLocationInSA;

    public string testdirectory;
    public string testtrack;
    public string testaudiofile;

    AudioClip clip;
    public Koreography koreo;
    KoreographyTrack track;

    public SimpleMusicPlayer smp;

    // Start is called before the first frame update
    void Start()
    {
        //ExportKoreoToJSON_Editor();
        //ExportKoreoTrackToJSON_Editor();

        //Koreographer.Instance.RegisterForEvents("This Time -- Main -- Normal", FireEventDebug);

        //smp.Stop();
        //EditorKoreoImport();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FireEventDebug(KoreographyEvent evt)
    {
        Debug.Log("Event!");
    }

    public void EditorTestNoLoad()
    {
        smp.LoadSong(koreoExportEditor, 0, false);
    }

    public void LoadChartFromStreaming(string directory, string track, string audiofile)
    {
        // Find valid directory that matches string

        var chartDirectory = Application.dataPath + "/StreamingAssets/Tracks/" + directory;

        Debug.Log(chartDirectory);

        ChartData chartdata;

        if (Directory.Exists(chartDirectory))
        {
            Debug.Log("Matching chart directory found");
            foreach (string s in Directory.GetFiles(chartDirectory))
            {
                Debug.Log(s);
                // Find and parse chart data
                if (s == chartDirectory + "\\chart-data.json")
                {
                    chartdata = JsonUtility.FromJson<ChartData>(File.ReadAllText(s));
                    Debug.Log(chartdata.track);
                    break;
                }
            }
        }

        else
        {
            Debug.LogError("Directory not found: " + chartDirectory);
        }
        
        // Validate files (koreography, difficulty, audio file)

        // Load koreography from json to Koreography object
        
        // Load 
    }

    public void ExportKoreoToJSON_Editor()
    {
        string koreoJSON = JsonUtility.ToJson(koreoExportEditor);

        // Write koreo JSON to file
        var path = Application.dataPath + "/StreamingAssets/Tracks/" + koreoExportLocationInSA;
        var endPath = Application.persistentDataPath;
        File.WriteAllText(endPath + koreoExportLocationInSA + ".json", koreoJSON);

    }

    public void ExportKoreoTrackToJSON_Editor()
    {
        string koreoTrackJSON = JsonUtility.ToJson(koreoTrackExportEditor);
        var path = Application.persistentDataPath;
        File.WriteAllText(path + koreoTrackExportEditor.name + ".json", koreoTrackJSON);
    }

    /// <summary>
    /// Loads Koreography and KoreographyTracks from JSON files.
    /// No parameters are passed, as the locations/filenames are meant to be retrieved
    /// from global variables.
    /// 
    /// Note: locations/filenames are hard-coded for the sake of testing until 
    /// song selection is complete.
    /// </summary>
    public void LoadChartJSONs()
    {
        // Load the base Koreography file from the StreamingAssets/Tracks folder

        // TODO: change location to persistentDataPath so updates won't require players
        // to keep moving custom tracks to new build locations
        var koreoJSON = File.ReadAllText(
            Application.dataPath + 
            "/StreamingAssets/Tracks/" + 
            testdirectory + 
            "/" + 
            testdirectory + ".json"
        );

        // Convert to Koreography instance
        Koreography koreography = ScriptableObject.CreateInstance<Koreography>();
        JsonUtility.FromJsonOverwrite(koreoJSON, koreography);

        // Load the specific KoreographyTrack file from the StreamingAssets/Tracks folder

        // TODO: change location to a subfolder within the directory, so different 
        // difficulties are separated and more easily located
        var koreoTrackJSON = File.ReadAllText(
            Application.dataPath + 
            "/StreamingAssets/Tracks/" + 
            testdirectory + 
            "/" + 
            testtrack + 
            ".json"
        );

        // Convert to KoreographyTrack instance
        KoreographyTrack track = ScriptableObject.CreateInstance<KoreographyTrack>();
        JsonUtility.FromJsonOverwrite(koreoTrackJSON, track);

        // Clear all track references (if any) from the new Koreography
        var baseTrackCount = koreography.Tracks.Count;
        for (int i = 0; i < baseTrackCount; i++) koreography.RemoveTrack(koreography.Tracks[0]);

        // Add the current KoreographyTrack to the Koreography

        // TODO: expand this to add controls track, as well (for ending/controlling playback),
        // game event track should always be first
        koreography.AddTrack(track);

        // Save Koreography to internal variable for reference while loading audio,
        // and load the Koreography var
        koreo = koreography;
        Koreographer.Instance.LoadKoreography(koreo);

        // Pull event trigger ID from loaded koreography and set event trigger global var
        var koreoTracks = koreo.Tracks;
        var eventID = koreoTracks[0].EventID;
        FsmVariables.GlobalVariables.FindFsmString("koreoEventTriggerID").Value = eventID;

        // Load the audio file associated with this chart
        //LoadKoreographyAudio("Kayzo - This Time.wav");
        LoadKoreographyAudio(testaudiofile + ".wav");
    }

    void LoadKoreographyAudio(string songName)
    {
        var path = "file://" + Application.dataPath + "/StreamingAssets/Tracks/" + testdirectory + "/";
        //Debug.Log(path + songName);
        StartCoroutine(LoadAudioClip(path, songName));
    }

    IEnumerator LoadAudioClip(string path, string songName)
    {
        // Fetch audio file from path
        WWW request = GetAudioFromFile(path, songName);
        yield return request;

        // Load audio clip retrieved from request (no MP3s)
        AudioClip clip = request.GetAudioClip();
        Debug.Log(clip.length);
        AudioSource koreoAudioSource = smp.transform.GetComponent<AudioSource>();
        clip.name = "CLIP";

        // Set audio clip
        koreoAudioSource.clip = clip;
        koreo.SourceClip = clip;
        koreo.SampleRate = 44100;

        // Load the audio into the SimpleMusicPlayer, but don't play yet        
        smp.LoadSong(koreo, 0, false);

        // Debugging

        //Debug.Log(smp.GetCurrentClipName());
        //Debug.Log(smp.GetTotalSampleTimeForClip(smp.GetCurrentClipName()));
        //Debug.Log(Koreographer.Instance.GetMusicBeatLength());
        //Debug.Log(Koreographer.Instance.GetMusicSampleRate());
        //Debug.Log("tempo: " + Koreographer.Instance.GetMusicBPM());
    }

    WWW GetAudioFromFile(string path, string filename)
    {
        string load = string.Format(path + filename);
        WWW request = new WWW(load);
        return request;
    }

    public void EditorKoreoImport()
    {
        // get base JSON file
        var jKoreo = File.ReadAllText(Application.dataPath + "/StreamingAssets/Tracks/" + koreoExportLocationInSA + "/" + koreoExportLocationInSA + ".json");

        //Debug.Log(jKoreo);

        // turn json to koreography chart
        Koreography k = ScriptableObject.CreateInstance<Koreography>();
        JsonUtility.FromJsonOverwrite(jKoreo, k);

        //Debug.Log(k.SampleRate);

        // get track JSON file
        // note: hardcoded because a standard for koreo/track names isn't established yet (TODO)
        var jKoreoTrack = File.ReadAllText(Application.dataPath + "/StreamingAssets/Tracks/This Time/This Time -- Main -- Normal.json");
        KoreographyTrack track = ScriptableObject.CreateInstance<KoreographyTrack>();
        JsonUtility.FromJsonOverwrite(jKoreoTrack, track);

        var baseTrackCount = k.Tracks.Count;
        for (int i = 0; i < baseTrackCount; i++) k.RemoveTrack(k.Tracks[0]);

        k.AddTrack(track);
        //Debug.Log(k.Tracks[0].GetAllEvents().Count);

        //foreach (KoreographyTrackBase ktb in k.Tracks)
        //{
        //    Debug.Log(ktb.EventID);
        //}

        // load the chart
        //smp.LoadSong(k, 0, false);
        koreo = k;
        Koreographer.Instance.LoadKoreography(koreo);


        // pull event trigger ID from loaded koreography
        var koreoTracks = k.Tracks;
        //foreach (KoreographyTrackBase ktb in koreoTracks) Debug.Log(ktb.EventID);

        var eventID = koreoTracks[0].EventID;

        // Set the event trigger global var
        FsmVariables.GlobalVariables.FindFsmString("koreoEventTriggerID").Value = eventID;

        LoadKoreographyAudio("Kayzo - This Time.wav");
    }


    public void KoreoOverwriteTest()
    {
        // create json file
        var jKoreo = JsonUtility.ToJson(koreo, true);

        Debug.Log(jKoreo);

        // turn json to koreography chart
        Koreography k = ScriptableObject.CreateInstance<Koreography>();
        JsonUtility.FromJsonOverwrite(jKoreo, k);

        // load the chart
        smp.LoadSong(k, 0, false);

        // pull event trigger ID from loaded koreography
        var koreoTracks = k.Tracks;
        //foreach (KoreographyTrackBase ktb in koreoTracks) Debug.Log(ktb.EventID);

        var eventID = koreoTracks[0].EventID;

        // Set the event trigger global var
        FsmVariables.GlobalVariables.FindFsmString("koreoEventTriggerID").Value = eventID;
    }
}
