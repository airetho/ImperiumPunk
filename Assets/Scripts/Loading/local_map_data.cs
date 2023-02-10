using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using TMPro;

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




        /* --- TODO: Quadrant Saving System. --- */




        //Cut quadrant from total openstreetmap earth data. 
        double lowerLat = (Math.Round(1000 * (longitude - .005))) / 1000;
        double lowerLong = (Math.Round(1000 * (latitude - .005))) / 1000;
        
        double upperLat = (Math.Round(1000 * (longitude + .005))) / 1000;
        double upperLong = (Math.Round(1000 * (latitude + .005))) / 1000; 
             
        //Construct URL for the API call. 
        string URL = base_url + lowerLat + "," + lowerLong + "," + upperLat + "," + upperLong; 
        
        //API Coroutine --- Debug.Log(URL);
        StartCoroutine(download_file(URL));
    
    }


    ///API Call && Downloading local data to user's folder
    private IEnumerator download_file(string url)
    {
        //API Request to Recieve 
        var unity_web_request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
        
    
        string path = Path.Combine(Application.persistentDataPath, "local_data.txt");

        if (File.Exists(@Application.persistentDataPath + "/local_data.txt")) {
            
            Debug.Log("File Exists");


            //Deleting File
            File.Delete(@Application.persistentDataPath + "/local_data.txt");

            //Program must wait before attempting to create the file.
            while(File.Exists(@Application.persistentDataPath + "/local_data.txt"))
            {
                System.Threading.Thread.Sleep(100);
                Debug.Log("Sleep");
            }
            Debug.Log("File Deleted");
                     
            
        } else {
            Debug.Log("File Doesn't Exist");
            //Does Nothing
       
        }

        //Creates a new instance and a file on disk where downloaded data will be written to. 
        Debug.Log("Creating File.");
        unity_web_request.downloadHandler = new DownloadHandlerFile(path);
        yield return unity_web_request.SendWebRequest();

        
        if (unity_web_request.result != UnityWebRequest.Result.Success) 
        {
            //Datalog.text = "Error.";
            Debug.LogError(unity_web_request.error + "Error Code: apples");
        }
    
        else
        {
            Debug.Log("File successfully downloaded and saved to " + path);

            //Once Downloaded Load game_world
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }       
    }
}