using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class PopulateMenuComponents : MonoBehaviour
{
    // used for populating song select with all songs in Tracks directory
    public Transform SongSelectTransform;
    // MenuButton prefab
    public GameObject MenuButtonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Path.GetDirectoryName(Application.dataPath));
        Debug.Log(Application.dataPath);

        DirectoryInfo trackDirInfo = new DirectoryInfo(Application.dataPath + "/../Tracks");

        if (!trackDirInfo.Exists)
        {
            Debug.Log("Tracks directory not found, copying...");

            var trackDir = Directory.CreateDirectory(Application.dataPath + "/../Tracks");

            //string[] files = Directory.GetFiles(Application.streamingAssetsPath);
            //foreach(string s in files)
            //{
            //    Debug.Log(s);
            //}

            Debug.Log("Copy directory: " + trackDir.FullName);

            CopyBaseTracksDirectory(Application.streamingAssetsPath + "/Tracks", Application.dataPath + "/../Tracks");
        }

        // Get all directories in Tracks folder
        var songs = Directory.GetDirectories(Application.dataPath + "/../Tracks");
        foreach (string dir in songs)
        {
            Debug.Log(dir);
            var button = Instantiate(MenuButtonPrefab, SongSelectTransform);
            button.GetComponentInChildren<Text>().text = Path.GetFileName(dir);
        }

        // Validate base song directory (root has valid Koreography JSON)

        // Get all difficulty directories in song directory

        // Validate difficulty directory (root has valid KoreographyTrack JSON)
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CopyBaseTracksDirectory(string directoryPath, string target)
    {
        // Create next directory
        Directory.CreateDirectory(target);

        // Copy each file in the active directory path
        foreach (string file in Directory.GetFiles(directoryPath))
        {
            Debug.Log(file);
            File.Copy(file, Path.Combine(target, Path.GetFileName(file)));
        }

        // Copy sub-directories
        foreach (string dir in Directory.GetDirectories(directoryPath))
        {
            Debug.Log(dir);
            CopyBaseTracksDirectory(dir, Path.Combine(target, Path.GetFileName(dir)));
        }
    }
}
