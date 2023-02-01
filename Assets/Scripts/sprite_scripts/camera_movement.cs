using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class camera_movement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] public Transform player;
    
    //all camera_states: "low" | "high" 
    [SerializeField] public static string camera_state = "low";
    
    public float distance_to_target;

    //"low" swipe var's
    private Vector2 previous_position; 
    private GameObject camera_parent; 
    public bool natural_motion = true; 
    public float rotation_speed; 
   

    ///In game-lore the "camera" is a small Observation Ballon manned by only a few people.


    void Start()
    {   
        

        Transform originalParent = transform.parent;            //check if t$$anonymous$$s camera already has a parent
        camera_parent = new GameObject ("camera");                //create a new gameObject
        camera_parent.transform.position = player.position;        //place the new gameObject at pivotPoint location
        transform.parent = camera_parent.transform;                    //make t$$anonymous$$s camera a c$$anonymous$$ld of the new gameObject
        camera_parent.transform.parent = originalParent;            //make the new gameobject a c$$anonymous$$ld of the original camera parent if it had one
        camera_dist();
    }


    void Update()
    {
        switch (camera_state)
        {

            //"low" or City View
            case "low":
                camera_dist();
                swipe_control();
            break;


            //"high" or Empire View 
            case "high":
                swipe_control();           
            break;
        }

    }

    void camera_dist(){
        cam.transform.position = player.position;
        camera_parent.transform.position = player.position; 
        int buffer_up = 20;          
        cam.transform.Translate(new Vector3(0, buffer_up, - distance_to_target));
    }

    void swipe_control(){
        
        if (Input.touchCount > 0)
                {
                    foreach (Touch touch in Input.touches)
                    {
                        if (touch.phase == TouchPhase.Began && touch.fingerId == 0)
                            {
                                previous_position = touch.position;
                            }
                            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                            {
                                if (touch.fingerId == 0)
                                {
                                    float x_difference = touch.position.x - previous_position.x;    //t$$anonymous$$s calculates the horizontal distance between the current finger location and the location last frame.
                                    if (!natural_motion){x_difference *= -1;}
                                    if (x_difference != 0){camera_parent.transform.Rotate (Vector3.up * x_difference * rotation_speed);}
                                    previous_position = touch.position;
                                }
                            }
                            if (touch.phase == TouchPhase.Ended && touch.fingerId == 0)
                            {
                                previous_position = touch.position;
                            }
                    }
                }
        
                //MOUSE
        
                if (Input.GetMouseButtonDown(0))
                {
                    previous_position = Input.mousePosition;
                }
                if (Input.GetMouseButton(0))
                {
                    float x_difference = Input.mousePosition.x - previous_position.x;
                    if(!natural_motion){x_difference *= -1;}
                    if(x_difference != 0){camera_parent.transform.Rotate(Vector3.up * x_difference * rotation_speed);}
                    previous_position = Input.mousePosition;
                }
                if (Input.GetMouseButtonUp (0))
                {
                    previous_position = Input.mousePosition;
                }
    }
}



