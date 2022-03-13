using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SonicBloom.Koreo;
using PlayMaker;

/// <summary>
/// Class for navigating the main menu (because TextMeshPro is not natively supported by PlayMaker)
/// </summary>
public class MainMenuNavigation : MonoBehaviour
{
    // Canvases for broad activation/deactivation
    public GameObject titleCanvas;
    public GameObject songSelectCanvas;
    public GameObject settingCanvas;

    // Canvas child objects for navigation
    public TMP_Text songDiffText;
    public GameObject songChoices;
    public GameObject diffChoices;
    public GameObject diffSelectBack;

    // Control bools for song/difficulty selection
    bool selectingSongOrDiff = true; // true = song, false = diff
    string currentSelectedSong = ""; // used to identify what set of difficulties to show
    public List<GameObject> songDifficultySets;

    public bool canInteract = true;
    PlayMakerFSM navFSM;


    // Start is called before the first frame update
    void Start()
    {
        navFSM = GetComponent<PlayMakerFSM>();
        NavigateToTitle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NavigateToTitle()
    {
        if (canInteract == true)
        {
            titleCanvas.SetActive(true);
            songSelectCanvas.SetActive(false);
            settingCanvas.SetActive(false);
        }
    }

    public void NavigateToSongSelect()
    {
        if (canInteract == true)
        {
            titleCanvas.SetActive(false);
            songSelectCanvas.SetActive(true);
            settingCanvas.SetActive(false);

            // songDiffChoice set by button before calling this
            UpdateSongDiffChoiceUI();
        }
    }

    public void NavigateToSettings()
    {
        if (canInteract == true)
        {
            titleCanvas.SetActive(false);
            songSelectCanvas.SetActive(false);
            settingCanvas.SetActive(true);
        }
    }

    public void UpdateChoiceText()
    {
        if (canInteract == true)
        {
            if (selectingSongOrDiff == true) songDiffText.text = "Select a song";
            else songDiffText.text = "Select a difficulty";
        }
    }

    public void UpdateSongDiffChoiceUI()
    {
        if (canInteract == true)
        {
            if (selectingSongOrDiff == true)
            {
                songChoices.SetActive(true);
                diffChoices.SetActive(false);
            }
            else
            {
                diffChoices.SetActive(true);
                songChoices.SetActive(false);
            }

            //Debug.Log("Updating song/diff UI");

            UpdateChoiceText();
        }
    }

    public void SetSongDiffChoice(bool choice)
    {
        if (canInteract == true)
            selectingSongOrDiff = choice;
    }

    public void SongSelectShowBack()
    {
        if (canInteract == true)
        {
            if (selectingSongOrDiff == true)
            {
                diffSelectBack.SetActive(false);
            }
            else
            {
                diffSelectBack.SetActive(true);
            }
        }
    }

    public void SelectSong(string song)
    {
        if (canInteract == true)
            currentSelectedSong = song;
    }

    public void ShowDifficultySet()
    {
        if (canInteract == true)
        {
            foreach (GameObject diffSet in songDifficultySets)
            {
                if (diffSet.name == currentSelectedSong)
                {
                    diffSet.SetActive(true);
                }
                else diffSet.SetActive(false);
            }
        }
    }

    public void SelectSongDifficulty(Koreography songKoreo)
    {
        if (canInteract == true)
        {
            SelectedSongHolder.instance.StoreKoreography(songKoreo);

            // Begin transition to in-game scene, locking UI navigation
            canInteract = false;
            navFSM.SendEvent("Difficulty Selected");
        }
    }

}
