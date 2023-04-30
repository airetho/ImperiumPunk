using UnityEngine;

public class walle : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //Update Walle's Location

        float z = (GameObject.Find("player").transform.position.z + GameObject.Find("main_camera").transform.position.z) / 2;
        float x = (GameObject.Find("player").transform.position.x + GameObject.Find("main_camera").transform.position.x) / 2;

        transform.position = transform.position + new Vector3(x, 0.3f, z);
    }
}
