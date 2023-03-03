using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class switch_camera_mode : MonoBehaviour
{
    public Transform main_camera;

    // Start is called before the first frame update
    public void switching_camera_mode()
    {
        if (camera_movement.camera_state == "low")
        {
            camera_movement.camera_state = "high";
            main_camera.position = new Vector3(GameObject.Find("player").transform.position.x, 700f, GameObject.Find("player").transform.position.z);
            main_camera.rotation = Quaternion.Euler(new Vector3(90f,0,0));
            
        } else if (camera_movement.camera_state == "high")
        {
            camera_movement.camera_state = "low";
            main_camera.position = new Vector3(0f, 50f, -50f);
            main_camera.rotation = Quaternion.Euler(new Vector3(25f,0,0));
            
        } 
    }
}        