using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.Networking;


using System.IO;

using System;

using UnityEngine.SceneManagement;




public class local_map_data : MonoBehaviour
{

    //Template for testing
    //private const string URL = "https://api.openstreetmap.org/api/0.6/map?bbox=-89.235,37.7,-89.215,37.72";
    private const string base_url = "https://api.openstreetmap.org/api/0.6/map?bbox=";

    public TextMeshProUGUI data_log;
    

    public void DataDownload(double latitude, double longitude)
    {

        /* Testing Latitude and Longitude Accuracy 
        Debug.Log("Local Data Called.");
        Debug.Log("Latitude = " + latitude);
        Debug.Log("Longitude = " + longitude);
        */

        data_log.text = "Saving Local Data";

        //Cut quadrant from total openstreetmap earth data. 
        double lowerLat = (Math.Round(1000 * (longitude - .005))) / 1000;
        double lowerLong = (Math.Round(1000 * (latitude - .005))) / 1000;
        
        double upperLat = (Math.Round(1000 * (longitude + .005))) / 1000;
        double upperLong = (Math.Round(1000 * (latitude + .005))) / 1000; 
             
        //Construct URL for the API call. 
        string URL = base_url + lowerLat + "," + lowerLong + "," + upperLat + "," + upperLong; 

        //Create File Name
        string file_name = (upperLat + "," + upperLong + ".txt");

        //API Coroutine --- Debug.Log(URL);
        StartCoroutine(download_file(URL,file_name));
    }


    ///API Call && Downloading local data to user's folder
    private IEnumerator download_file(string url, string file_name)
    {
        //API Request to Recieve 
        var unity_web_request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
        string path = Path.Combine(Application.persistentDataPath, file_name);

        //If File Doesn't Exist, Save File
        if (!System.IO.File.Exists(path)){
            while (true) {
                try {
                    Debug.Log("Creating File.");
                    data_log.text = ("Creating File");
                    unity_web_request.downloadHandler = new DownloadHandlerFile(path); 
                    break;
                } catch {
                    Debug.Log("Thread Sleep.");
                    System.Threading.Thread.Sleep(10);
                }     
            }
        } else {
            //Do Nothing
        } 

        yield return unity_web_request.SendWebRequest();


        
        if (unity_web_request.result != UnityWebRequest.Result.Success) 
        {
            Debug.LogError("Error Code: 1001 - " + unity_web_request.error);
        }
    
        else
        {
            Debug.Log("File successfully downloaded and saved.");
            data_log.text = ("File Successfully Downloaded.");
            
            //Once Downloaded Load game_world
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }       
    }
}