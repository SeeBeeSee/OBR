using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MasterAudioMixer : MonoBehaviour
{
    public AudioMixer mixer;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("MasterVol"))
        {
            PlayerPrefs.SetFloat("MasterVol", .7f);
        }
        SetMasterLevel(PlayerPrefs.GetFloat("MasterVol"));


        if (!PlayerPrefs.HasKey("MusicVol"))
        {
            PlayerPrefs.SetFloat("MusicVol", .7f);
        }
        SetMusicLevel(PlayerPrefs.GetFloat("MusicVol"));


        if (!PlayerPrefs.HasKey("SfxVol"))
        {
            PlayerPrefs.SetFloat("SfxVol", .7f);
        }
        SetSfxLevel(PlayerPrefs.GetFloat("SfxVol"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMasterLevel(float sliderValue)
    {
        mixer.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MasterVol", sliderValue);
        Debug.Log(PlayerPrefs.GetFloat("MasterVol"));
    }

    public void SetMusicLevel(float sliderValue)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVol", sliderValue);
    }

    public void SetSfxLevel(float sliderValue)
    {
        mixer.SetFloat("SfxVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("SfxVol", sliderValue);
    }
}
