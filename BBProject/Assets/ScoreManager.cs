using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

[Serializable]
public class ChartScores
{
    public SessionScore[] scores;
}

[Serializable]
public class SessionScore
{
    public float accuracy;
    public string scoreDate = System.DateTime.Now.ToString();
}

public class ScoreManager : MonoBehaviour
{
    string currentChartName = "This Time";
    string currentChartDiff = "Normal";
    float currentEvalScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Executed at the end of a successful session. Controls whether new scores
    /// are written to the score file for the given chart/difficulty or not.
    /// </summary>
    public void EvaluateScore()
    {
        bool checkScoreTop10 = CheckNewHighScore();

        if (checkScoreTop10)
            WriteNewScoreChartDiff();
        // Does nothing if score doesn't qualify.
    }

    /// <summary>
    /// Using the currently loaded chart name and difficulty, load (up to) top 10
    /// scores saved for that chart/difficulty.
    /// </summary>
    /// <returns>Returns a list of up to 10 floats, if any scores are saved.
    /// Otherwise, returns an empty list of floats.</returns>
    public List<float> ReadScoresChartDiff()
    {
        List<float> scores = new List<float>();

        return scores;
    }

    /// <summary>
    /// Writes a new score to the top list of scores for the loaded chart/difficulty.
    /// </summary>
    void WriteNewScoreChartDiff()
    {

    }

    /// <summary>
    /// Determines if the current evaluation score qualifies for top 10 for the 
    /// currently loaded chart/difficulty.
    /// </summary>
    /// <returns>Returns true if the score qualifies as a top 10 score for the 
    /// chart. Otherwise, returns false.</returns>
    bool CheckNewHighScore()
    {
        bool scoreQualifiesTop10 = false;

        var top10scores = ReadScoresChartDiff();
        if (top10scores.Count > 0)
        {
            foreach (float score in top10scores)
            {
                if (currentEvalScore > score)
                {
                    scoreQualifiesTop10 = true;
                    break;
                }
            }
        }

        return scoreQualifiesTop10;
    }

    /// <summary>
    /// Loads the current JSON score file for the chart/difficulty.
    /// If no score file is available for the given chart/difficulty, a new one is generated.
    /// </summary>
    /// <returns></returns>
    public string LoadScoreChartJSON()
    {
        string loadedChartScores = "";
        var currentPath = Application.persistentDataPath +
            "/Tracks/" + currentChartName + "/" + currentChartDiff;
        Debug.Log(currentPath);
        Debug.Log(Directory.Exists(currentPath));
        if (!Directory.Exists(currentPath))
        {
            Directory.CreateDirectory(currentPath);
            var newChartScores = new ChartScores();
            newChartScores.scores = new SessionScore[10];
            //var newSessionScore = new SessionScore();
            //newSessionScore.accuracy = 69.42f;
            //newChartScores.scores[0] = newSessionScore;
            string newJson = JsonUtility.ToJson(newChartScores, true);
            Debug.Log("New score json: " + newJson);
            File.WriteAllText(currentPath + "/scores.json", newJson);
        }
        else if (!File.Exists(currentPath + "/scores.json"))
        {
            var newChartScores = new ChartScores();
            newChartScores.scores = new SessionScore[10];

            string newJson = JsonUtility.ToJson(newChartScores, true);
            Debug.Log("New score json: " + newJson);
            File.WriteAllText(currentPath + "/scores.json", newJson);
        }
        Debug.Log(Directory.GetFiles(currentPath).Length);

        return loadedChartScores;
    }
}
