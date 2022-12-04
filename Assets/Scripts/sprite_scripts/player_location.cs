using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_location : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateLocation", 1.0f, 1.0f);
    }

    void UpdateLocation()
    {
        Vector3 centre = map_reader.centre;
        Debug.Log("centre: " + centre);
        
        //(-9932481.00, 0.00, 4512401.00) <- #If centre is undefined.

        //Check if location is enabled by user.
        if (!Input.location.isEnabledByUser) 
        {
            Debug.Log("Location Services unavailable.");
            centre = new Vector3(0,0,0);
            CancelInvoke();
        } 


        float x = (float) (mercator_projection.lonToX(Input.location.lastData.longitude - centre.x));
        float y = (float) (mercator_projection.latToY(Input.location.lastData.latitude - centre.z));
        
        Debug.Log("coords = " + x + ", " + y);
        transform.position = new Vector3(x,0,y);
    }
}
