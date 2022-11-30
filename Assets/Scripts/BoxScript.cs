using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateLocation", 2f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateLocation()
    {
        Vector3 centre = map_reader.centre;
        float x = (float) (centre.x - mercator_projection.lonToX(Input.location.lastData.longitude));
        float y = (float) (centre.z - mercator_projection.latToY(Input.location.lastData.latitude));
        Debug.Log("coords = " + x + ", " + y);
        transform.position = new Vector3(x,0,y);
    }
}
