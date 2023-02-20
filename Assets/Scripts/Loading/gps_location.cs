using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gps_location : MonoBehaviour
{
    public TextMeshProUGUI  gps_status;
    public TextMeshProUGUI  latitude_value;
    public TextMeshProUGUI  longitude_value;
    public TextMeshProUGUI  altitude_value;
    public TextMeshProUGUI  horizontal_accuracyValue;
    public TextMeshProUGUI  timestamp_value;
    public TextMeshProUGUI  log;


    //Call GPS Connection Coroutine
    void Start()
    {
        log.text = "Project Runtime";
        StartCoroutine(gps_loc());
    }

    //Attempts to connect to the GPS.
    IEnumerator gps_loc()
    {
        log.text = "Entered gps_loc";

        //Check if user has location service enabled - If not requests for user access.
        if (!Input.location.isEnabledByUser) 
        {
            log.text = "Waiting on user";
            yield return new WaitForSeconds(5);
        }
            


        //Start GPS location service before querying location
        log.text = "Input Location";
        Input.location.Start();
        

        //Wait for GPS location service to initalize.
        int maxWait = 30; 
        while ((Input.location.status == LocationServiceStatus.Initializing) && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }



        /* --- TODO: Add a loop to retry connecting to GPS. --- */



        //If GPS location service doen't initialize in 30 seconds, break.
        if (maxWait < 1) 
        {
            gps_status.text = "Timed out.";
            yield break;
        }


        //If GPS location service failed to connect. 
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            gps_status.text = "Connection Failed";
            yield break;
        } 
        //Else Invoke our GPS function.
        else 
        {
            gps_status.text = "Inovke Function";

            //Invoke GPS Location Data every sixty seconds. With a zero second initial delay. 
            InvokeRepeating("updating_gps_data", 0.6f, 0.15f);
        }

    }



    //Method used to update GPS location data.
    private void updating_gps_data()
        {
            if (Input.location.status == LocationServiceStatus.Running) 
            {                
                

                //Access Granted to GPS Values
                gps_status.text = "Running.";
                latitude_value.text = Input.location.lastData.latitude.ToString();
                longitude_value.text = Input.location.lastData.longitude.ToString();
                altitude_value.text = Input.location.lastData.altitude.ToString();
                horizontal_accuracyValue.text = Input.location.lastData.horizontalAccuracy.ToString();
                timestamp_value.text = Input.location.lastData.timestamp.ToString(); 
                
                //Send Data to be Downloaded.
                local_map_data local_map_data = gameObject.GetComponent<local_map_data>();
                local_map_data.DataDownload(Input.location.lastData.latitude, Input.location.lastData.longitude);
                
                /* --- TODO: Check if this method repeats ad-infinitum. --- */
                
                //This method continues to repeat? 
                //Input.location.Stop();

            } 
            else //Service is Stopped 
            {
                gps_status.text = "Location Service Not Running";   
            }  
        }
}