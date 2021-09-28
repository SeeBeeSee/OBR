using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;


/*
 * KOREOGRAPHY BITS
 */

//[Serializable]
//public class KoreoBitTrackBit
//{
//    public int instanceID;
//}

[Serializable]
public class KoreoTempoBit
{
    public string sectionName;
    public int startSample;
    public float samplesPerBeat;
    public int beatsPerMeasure;
    public bool bStartNewMeasure;
}

[Serializable]
public class KoreoBits
{
    //public int instanceID;
    public string mAudioFilePath;
    public int mSampleRate;
    public bool mIgnoreLatencyOffset;
    public KoreoTempoBit[] mTempoSections;
    //public KoreoBitTrackBit[] mTracks;
}

/*
 * KOREOGRAPHY TRACK BITS
 */

[Serializable]
public class KoreoEventBits
{
    public int mStartSample;
    public int mEndSample;
}

[Serializable]
public class KoreoTextPayloadBits
{
    public string mTextVal;
}

[Serializable]
public class KoreoTrackBits
{
    public string m_Name;
    public string mEventID;
    public KoreoEventBits[] mEventList;
    public KoreoTextPayloadBits[] _TextPayloads;
}

//
//
//

public class KoreoJSON : MonoBehaviour
{
    public AudioClip clip;
    public Koreography koreo;
    public KoreographyTrack track;

    public SimpleMusicPlayer smp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KoreoTest()
    {
        //smp = GetComponent<SimpleMusicPlayer>();

        //Koreography koreo = new Koreography();
        //KoreographyTrack kTrack = new KoreographyTrack();
        //var koreo = ScriptableObject.CreateInstance<Koreography>();
        //var kTrack = ScriptableObject.CreateInstance<KoreographyTrack>();
        //koreo.AddTrack(kTrack);

        //koreo.SourceClip = clip;

        //var output = JsonUtility.ToJson(koreo, true);
        //Debug.Log(output);

        //var processedKoreo = JsonUtility.FromJson<Koreography>(output);
        //Debug.Log(processedKoreo.SourceClip);




        /*
        var koreo = JsonUtility.ToJson(k, true);
        Debug.Log(koreo);
        Debug.Log(k.GetInstanceID());

        var pKoreo = JsonUtility.FromJson<KoreoBits>(koreo);
        Debug.Log("instanceID:" + pKoreo.instanceID);
        Debug.Log("file path:" + pKoreo.mAudioFilePath);
        Debug.Log("samplerate:" + pKoreo.mSampleRate);
        Debug.Log("ignorelatency:" + pKoreo.mIgnoreLatencyOffset);
        Debug.Log("temposections:");
        foreach (KoreoTempoBit t in pKoreo.mTempoSections) Debug.Log(t);
        Debug.Log("mtracks:" + pKoreo.mTracks);
        foreach (KoreoTrackBit t in pKoreo.mTracks) Debug.Log(t);

        var newKoreo = ScriptableObject.CreateInstance<Koreography>();
        newKoreo.SourceClip = 
        */

        //var kTrack = ScriptableObject.CreateInstance<KoreographyTrack>();
        //var j = JsonUtility.ToJson(kTrack,true);

        //var j_pre = JsonUtility.ToJson(k,true);

        //Debug.Log(j);
        //Debug.Log(j_pre);

        //foreach (KoreographyEvent ke in track.GetAllEvents()) Debug.Log(ke.GetTextValue());

        /*
         * TURN KOREOGRAPHYTRACK TO JSON, RETRIEVE
         */

        var jKoreo = JsonUtility.ToJson(koreo, true);
        var jTrack = JsonUtility.ToJson(track, true);

        KoreographyTrack t = ScriptableObject.CreateInstance<KoreographyTrack>();

        var fjTrack = JsonUtility.FromJson<KoreoTrackBits>(jTrack);
        List<KoreographyEvent> events = new List<KoreographyEvent>();
        KoreographyTrack newTrack = ScriptableObject.CreateInstance<KoreographyTrack>();

        // both event list and payload list *should* be same length
        for (int i = 0; i < fjTrack.mEventList.Length; i++)
        {
            var keb = fjTrack.mEventList[i];
            var ktpb = fjTrack._TextPayloads[i];
            //Debug.Log(ktpb.mTextVal);
            var newEvent = new KoreographyEvent();
            newEvent.StartSample = keb.mStartSample;
            newEvent.EndSample = keb.mEndSample;
            var newTextPayload = new TextPayload();
            newTextPayload.TextVal = ktpb.mTextVal;
            newEvent.Payload = newTextPayload;
            //events.Add(newEvent);
            newTrack.AddEvent(newEvent);
        }


        //Debug.Log(fjTrack.mEventID);
        newTrack.EventID = fjTrack.mEventID;

        //foreach (KoreographyEvent ke in newTrack.GetAllEvents()) Debug.Log(ke.GetTextValue() + " " + ke.StartSample);

        //Debug.Log(newTrack.EventID);

        /*
         * KOREOGRAPHY TO JSON, RETRIEVE
         */

        Debug.Log(jKoreo);
        var fjKoreo = JsonUtility.FromJson<KoreoBits>(jKoreo);
        Koreography k = ScriptableObject.CreateInstance<Koreography>();
        // May be easier to just load mp3 assets via unity instead of
        // pointing the new Koreography to the file?
        k.SourceClip = clip;
        //TempoSectionDef td = new TempoSectionDef();
        //td.

        for (int i = 0; i < fjKoreo.mTempoSections.Length; i++)
        {
            var td = k.InsertTempoSectionAtIndex(i);
            var section = fjKoreo.mTempoSections[i];
            td.SectionName = section.sectionName;
            td.StartSample = section.startSample;
            td.SamplesPerBeat = section.samplesPerBeat;
            td.BeatsPerMeasure = section.beatsPerMeasure;
            td.DoesStartNewMeasure = section.bStartNewMeasure;
            Debug.Log("(LOADING) Tempo at section " + td.SectionName + ": " + td.GetBPM(44100));
        }

        k.AddTrack(newTrack);
        smp.LoadSong(k,0,false);
    }
}
