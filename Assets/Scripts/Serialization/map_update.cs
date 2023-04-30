using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Xml;

public class map_update : MonoBehaviour
{
    //Developer Option to manually keep the GPS off for testing.
    public bool gps_off;
    public List<string> resource_file;


    
    void Start()
    {
        if (gps_off == false) {
            InvokeRepeating("UpdateMapData", 1.0f, 10.0f);
        }
    }


    //Every x seconds check if the player is in a new area then load in the region.
    void UpdateMapData()
    {
        //Update Map Location
        //Choose the proper data file.
        
        string data_folder = Application.persistentDataPath;

        DirectoryInfo d = new DirectoryInfo(data_folder);

        foreach (var file in d.GetFiles("*.txt"))
        {
            //Save File In Resource Path
            resource_file.Add(file.ToString());
        }
        
        bool located = false;

        for (int j = 0; j < resource_file.Count; j++) {
                
            int pFrom = resource_file[j].IndexOf("key.") + "key.".Length;
            int pTo = resource_file[j].LastIndexOf(",,");

            double upperLat = Convert.ToDouble(resource_file[j].Substring(pFrom, pTo - pFrom));    
            double lowerLat = upperLat - 0.01;
                
            pFrom = resource_file[j].IndexOf(",,") + ",,".Length;
            pTo = resource_file[j].LastIndexOf(".txt");
            
            double upperLong = Convert.ToDouble(resource_file[j].Substring(pFrom, pTo - pFrom));    
            double lowerLong = upperLong - 0.01;
            
            if (
                Input.location.lastData.latitude < upperLat && Input.location.lastData.latitude > lowerLat 
                
                &&
                Input.location.lastData.longitude < upperLong && Input.location.lastData.longitude > lowerLong 
                //Input.location.lastData.longitude < upperLong && Input.location.lastData.longitude > lowerLong 
            ){
                located = true;
                j = resource_file.Count + 1;
            } else {
                located = false;
            }
        }

        if (located == false) {          
            local_map_data local_map_data = gameObject.GetComponent<local_map_data>();
            local_map_data.DataDownload(Input.location.lastData.latitude, Input.location.lastData.longitude);

        }

    }
}
