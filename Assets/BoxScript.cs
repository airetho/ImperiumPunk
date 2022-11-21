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
        Vector3 Centre = MapReader.Centre;
        float x = (float) (Centre.x - MercatorProjection.lonToX(Input.location.lastData.longitude));
        float y = (float) (Centre.z - MercatorProjection.latToY(Input.location.lastData.latitude));
        Debug.Log("coords = " + x + ", " + y);
        transform.position = new Vector3(x,0,y);
    }
}
