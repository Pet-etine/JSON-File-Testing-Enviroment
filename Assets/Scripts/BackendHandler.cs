using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;


public class BackendHandler : MonoBehaviour
{
    const string jsonTestStr = "{ " +
    "\"scores\":[ " +
    "{\"id\":1, \"playername\":\"Matti\", \"score\":20, \"playtime\": \"2020-21-11 08:20:00\"}, " +
    "{\"id\":2, \"playername\":\"Hankka\", \"score\":30, \"playtime\": \"2020-21-11 08:20:00\"}, " +
    "{\"id\":3, \"playername\":\"Ismo\", \"score\":40, \"playtime\": \"2020-21-11 08:20:00\"} " +
    "] }";
    const string urlBackendHighScoresFile = "http://localhost/Unity-PHP-Demo-Backend/api/v1/highscores.json";
    //highscore table
    const string urlBackendHighScores = "http://localhost/Unity-PHP-Demo-Backend/api/v1/highscores.php";
    HighScores.HighScores hs;

    //Logging info
    string log = "";
    int fetchCounter = 0;

    //UI Elements
    public UnityEngine.UI.Text loggingText;
    void Start()
    {
        Debug.Log("BackendHandler started.");

        //testing json conversion
        hs = JsonUtility.FromJson<HighScores.HighScores>(jsonTestStr);
        Debug.Log("HighScore winner name:" + hs.scores[0].playername);
        Debug.Log("HighScores as json:" + JsonUtility.ToJson(hs));

        InsertToLog("Game started."); // Call the method with the correct name
    }

    // Update is called once per frame
    void Update()
    {
        loggingText.text = log;
    }
    public void FetchHighScoresJSONFile()
    {
        fetchCounter++;
        Debug.Log("FetchHighScoresJSONFile button clicked");
        StartCoroutine(GetRequestForHighScores(urlBackendHighScoresFile)); // Use the correct variable name

        EventSystem.current.SetSelectedGameObject(null);
    }


    public void FetchHighScoresJSON()
    {
        Debug.Log("FetchHighScoresJSON button clicked");
        // Add this line to prevent the gun from firing
        EventSystem.current.SetSelectedGameObject(null);
    }
    string InsertToLog(string s)
    {
        return log = "[" + fetchCounter + "]" + s + "\n" + log;
    }
    IEnumerator GetRequestForHighScores(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            InsertToLog(" Request sent to " + uri);
            // set downloadHandler for json
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");
            // Request and wait for reply
            yield return webRequest.SendWebRequest();

            // Updated condition to check for connection errors
            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                InsertToLog("Error encountered: " + webRequest.error);
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                // get raw data and convert it to string
                string resultStr = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);
                // create HighScore item from json string
                hs = JsonUtility.FromJson<HighScores.HighScores>(resultStr);
                InsertToLog("Response received succesfully ");
                Debug.Log("Received(UTF8): " + resultStr);
                Debug.Log("Received(HS): " + JsonUtility.ToJson(hs));
            }
        }
    }

}