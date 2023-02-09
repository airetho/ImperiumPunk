using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class place_building : MonoBehaviour
{

    public GameObject coal_building;
 

    public void create_building()
    {       
        float x = (GameObject.Find("player").transform.position.x + GameObject.Find("main_camera").transform.position.x) / 2;
        float z = (GameObject.Find("player").transform.position.z + GameObject.Find("main_camera").transform.position.z) / 2;
        Instantiate(coal_building, new Vector3(x,0,z), transform.rotation);
    }
}