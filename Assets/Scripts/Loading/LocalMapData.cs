using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using TMPro;

using System.IO;

using System;

using UnityEngine.SceneManagement;


public class LocalMapData : MonoBehaviour
{

    //private const string URL = "https://api.openstreetmap.org/api/0.6/map?bbox=-89.235,37.7,-89.215,37.72";
    private const string BaseURL = "https://api.openstreetmap.org/api/0.6/map?bbox=";

    public TextMeshProUGUI Datalog;

    public void DataDownload(double latitude, double longitude)
    {
        Debug.Log("Local Data Called.");
        Debug.Log("Latitude = " + latitude);
        Debug.Log("Longitude = " + longitude);

        Datalog.text = "LocalData Routine Start";

        double lowerLat = (Math.Round(1000 * (longitude - .01))) / 1000;
        double lowerLong = (Math.Round(1000 * (latitude - .01))) / 1000;
        
        double upperLat = (Math.Round(1000 * (longitude + .01))) / 1000;
        double upperLong = (Math.Round(1000 * (latitude + .01))) / 1000; 
        
        

        string URL = BaseURL + lowerLat + "," + lowerLong + "," + upperLat + "," + upperLong; 
        
        Debug.Log(URL);

        //Call to download
        StartCoroutine(DownloadFile(URL));
       
    }


    private IEnumerator DownloadFile(string url)
    {
        var uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
        string path = Path.Combine(Application.persistentDataPath, "local_data.txt");
        uwr.downloadHandler = new DownloadHandlerFile(path);
        yield return uwr.SendWebRequest();
        if (uwr.result != UnityWebRequest.Result.Success)
            //Datalog.text = "Error.";
            Debug.LogError(uwr.error);
        else
            Debug.Log("File successfully downloaded and saved to " + path);
            Datalog.text = "Downloaded: " + path ;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}