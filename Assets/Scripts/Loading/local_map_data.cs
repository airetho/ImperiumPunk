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
    

    public void DataDownload(double latitude, double longitude)
    {

        /* Testing Latitude and Longitude Accuracy 
        Debug.Log("Local Data Called.");
        Debug.Log("Latitude = " + latitude);
        Debug.Log("Longitude = " + longitude);
        */

        //Cut quadrant from total openstreetmap earth data. 
        double lowerLong = (Math.Round(1000 * (longitude - .005))) / 1000;
        double lowerLat = (Math.Round(1000 * (latitude - .005))) / 1000;

        double upperLong = (Math.Round(1000 * (longitude + .005))) / 1000;
        double upperLat = (Math.Round(1000 * (latitude + .005))) / 1000; 
             
        //Construct URL for the API call. 
        string URL = base_url + lowerLong + "," + lowerLat + "," + upperLong + "," + upperLat; 

        //Create File Name - Middle Center

        string file_name = ("key." + upperLat.ToString("00.000") + ",," + upperLong.ToString("00.000") + ".txt");
        //string file_name = ("map_data.txt");
        //API Coroutine --- Debug.Log(URL);
        
        
        //--DELETE OTHER FILES? --- //
        
        string[] files = Directory.GetFiles(Application.persistentDataPath);
        string data_file = Path.Combine(Application.persistentDataPath, "user_data.json");

        for (int i = 0; i < files.Length; i++) {
            if(files[i] != data_file)
            {
                File.Delete(files[i]);
                Debug.Log("Deleted File.");
            }
        }

        

        //Get the File Path 
        //---- Center File -----//
        string path = Path.Combine(Application.persistentDataPath, file_name);
        if (!System.IO.File.Exists(path)){
            StartCoroutine(download_file(URL,file_name));
        } 


        


        //Save 8 adjacent files
        /*
        //Top Left
        file_name = ("key." + (upperLat + 0.01).ToString("00.000") + ",," + (upperLong - 0.01).ToString("00.000") + ".txt");
        path = Path.Combine(Application.persistentDataPath, file_name);
        URL = base_url + (lowerLong - 0.02) + "," + lowerLat + "," + (upperLong - 0.01) + "," + (upperLat + 0.01); 
        if (!System.IO.File.Exists(path)){
            StartCoroutine(download_file(URL,file_name));
        }

        //Top Middle
        file_name = ("key." + (upperLat + 0.01).ToString("00.000") + ",," + (upperLong).ToString("00.000") + ".txt");
        URL = base_url + (lowerLong - 0.01) + "," + lowerLat + "," + (upperLong) + "," + (upperLat + 0.01); 
        path = Path.Combine(Application.persistentDataPath, file_name);
        if (!System.IO.File.Exists(path)){
            StartCoroutine(download_file(URL,file_name));
        }

        //Top Right
        file_name = ("key." + (upperLat + 0.01).ToString("00.000") + ",," + (upperLong + 0.01).ToString("00.000") + ".txt");
        URL = base_url + lowerLong + "," + lowerLat + "," + (upperLong + 0.01) + "," + (upperLat + 0.01); 
        path = Path.Combine(Application.persistentDataPath, file_name);
        if (!System.IO.File.Exists(path)){
            StartCoroutine(download_file(URL,file_name));
        }
        
        //---Wrong?--- Shifted Down and In//
        //Middle Left
        file_name = ("key." + (upperLat).ToString("00.000") + ",," + (upperLong - 0.01).ToString("00.000") + ".txt");
        path = Path.Combine(Application.persistentDataPath, file_name);
        URL = base_url + (lowerLong - 0.02) + "," + (lowerLat - 0.01) + "," + (upperLong - 0.01) + "," + (upperLat); 
        if (!System.IO.File.Exists(path)){
            StartCoroutine(download_file(URL,file_name));
        }
       


        //---Wrong?--- Shifted Down and In//
        //Middle Right
        file_name = ("key." + (upperLat).ToString("00.000") + ",," + (upperLong + 0.01).ToString("00.000") + ".txt");
        path = Path.Combine(Application.persistentDataPath, file_name);
        URL = base_url + lowerLong + "," + (lowerLat - 0.01) + "," + (upperLong + 0.01) + "," + (upperLat); 
        if (!System.IO.File.Exists(path)){
            StartCoroutine(download_file(URL,file_name));
        }

        //Bottom Left
        file_name = ("key." + (upperLat - 0.01).ToString("00.000") + ",," + (upperLong - 0.01).ToString("00.000") + ".txt");
        path = Path.Combine(Application.persistentDataPath, file_name);
        URL = base_url + (lowerLong - 0.02) + "," + (lowerLat - 0.02) + "," + (upperLong - 0.01) + "," + (upperLat - 0.01); 
        if (!System.IO.File.Exists(path)){
            StartCoroutine(download_file(URL,file_name));
        }

        //Bottom Middle
        file_name = ("key." + (upperLat - 0.01).ToString("00.000") + ",," + (upperLong).ToString("00.000") + ".txt");
        path = Path.Combine(Application.persistentDataPath, file_name);
        URL = base_url + (lowerLong - 0.01) + "," + (lowerLat - 0.02) + "," + (upperLong) + "," + (upperLat - 0.01); 
        if (!System.IO.File.Exists(path)){
            StartCoroutine(download_file(URL,file_name));
        }

        //Bottom Middle
        file_name = ("key." + (upperLat - 0.01).ToString("00.000") + ",," + (upperLong + 0.01).ToString("00.000") + ".txt");
        path = Path.Combine(Application.persistentDataPath, file_name);
        URL = base_url + lowerLong + "," + (lowerLat - 0.02) + "," + (upperLong + 0.01) + "," + (upperLat - 0.01); 
        if (!System.IO.File.Exists(path)){
            StartCoroutine(download_file(URL,file_name));
        }
        */

       






        //Once Downloaded Load game_world if this is called during the loading scene.
        if (SceneManager.GetActiveScene().buildIndex == 1) {
                
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                
        } else {
            //Do Nothing
        }
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
                    //Debug.Log("Creating File.");
                    unity_web_request.downloadHandler = new DownloadHandlerFile(path); 
                    break;
                } catch {
                    //Debug.Log("Thread Sleep.");
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
            //Do Nothing
        }       
    }
}