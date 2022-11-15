using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GPSLocation : MonoBehaviour
{
    public TextMeshProUGUI  GPSStatus;
    public TextMeshProUGUI  latitudeValue;
    public TextMeshProUGUI  longitudeValue;
    public TextMeshProUGUI  altitudeValue;
    public TextMeshProUGUI  horizontalAccuracyValue;
    public TextMeshProUGUI  timestampValue;
    public TextMeshProUGUI  log;


    // Start is called before the first frame update
    void Start()
    {
        log.text = "Project Runtime";
        StartCoroutine(GPSLoc());
    }

    IEnumerator GPSLoc()
    {
         log.text = "Entered GPSLoc";

        //Check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            log.text = "Waiting on user";
            yield return new WaitForSeconds(5);


        //Start Service before querying location
        log.text = "Input Location";
        Input.location.Start();
        

        int maxWait = 20; 
        while ((Input.location.status == LocationServiceStatus.Initializing) && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        //Service didn't init in 20 seconds
        if (maxWait < 1) 
        {
            GPSStatus.text = "Timed out.";
            yield break;
        }

        //Connection Failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            GPSStatus.text = "Connection Failed";
            yield break;
        } 
        else 
        {
            // Access Granted
            GPSStatus.text = "Inovke Function";
            InvokeRepeating("UpdatingGPSData", 2f, 1f);

        }

    }//End of GPSLoc

    private void UpdatingGPSData()
        {
            if (Input.location.status == LocationServiceStatus.Running) 
            {                
                //Access Granted to GPS Values
                GPSStatus.text = "Running.";
                latitudeValue.text = Input.location.lastData.latitude.ToString();
                longitudeValue.text = Input.location.lastData.longitude.ToString();
                altitudeValue.text = Input.location.lastData.altitude.ToString();
                horizontalAccuracyValue.text = Input.location.lastData.horizontalAccuracy.ToString();
                timestampValue.text = Input.location.lastData.timestamp.ToString(); 
                
                //Send Data to be Downloaded.
                LocalMapData localMapData = gameObject.GetComponent<LocalMapData>();
                localMapData.DataDownload(Input.location.lastData.latitude, Input.location.lastData.longitude);
                
                Input.location.Stop();

            } 
            else 
            {
                GPSStatus.text = "Stopped.";
                //Service is Stopped
            }  
        }
} //End of GPSLocation