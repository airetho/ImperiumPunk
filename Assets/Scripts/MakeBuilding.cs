using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeBuilding : MonoBehaviour
{

    public GameObject coal_building;


    public void create_building()
    {       
        Instantiate(coal_building, GameObject.Find("player").transform.position, transform.rotation);
    }
}
