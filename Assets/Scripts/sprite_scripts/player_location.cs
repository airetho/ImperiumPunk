using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_location : MonoBehaviour
{

    //Developer Option to manually keep the GPS off for testing.
    public bool gps_off;

    // Start is called before the first frame update
    void Start()
    {

        //Check if location is enabled by user. As to only calculate the mercator_projection if the GPS is on.
        if (Input.location.isEnabledByUser) {
            gps_off = false; 
        }
        

        if (gps_off == false) {
            InvokeRepeating("UpdateLocation", 1.0f, 1.0f);
        }
          
    }

    void UpdateLocation()
    {
        Vector3 centre = map_reader.centre;
        
        
        //(-9932481.00, 0.00, 4512401.00) <- #If centre is undefined. --- Debug.Log("centre: " + centre); --- Debug.Log("coords = " + x + ", " + y);

        float z = (float) (mercator_projection.latToY(Input.location.lastData.latitude) - centre.z);
        float x = (float) (mercator_projection.lonToX(Input.location.lastData.longitude) - centre.x);
        
        
        transform.position = new Vector3(x,0.3f,z);
    }
}