using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_code : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

/*
//Swipe Input: https://forum.unity.com/threads/swipe-in-all-directions-touch-and-mouse.165416/
                //Time.timeScale = 2.0f;
                
                

                if(Input.touches.Length > 0)
                {
                    Touch t = Input.GetTouch(0); //Zero Indexed for the first finger to touch. 
                    
                    if(t.phase == TouchPhase.Began)
                    {
                        //save began touch float point
                        first_press_position = t.position.x;
                    }

                    if(t.phase == TouchPhase.Moved)
                    {
                        //save moved touch float point
                        float second_press_position = t.position.x;
                    
                        //create current_swipe from the change in position
                        current_swipe = second_press_position - first_press_position;
                        //Debug.Log("Current Swipe # " + current_swipe);
                        //Debug.Log("Screen Width: " + Screen.width);
                        current_swipe = (current_swipe / Screen.width);
                        //Debug.Log("Current Swipe % of screen: " + current_swipe);
                        current_swipe =  -360 * current_swipe;
                        //Debug.Log("Current Swipe % of 360: " + current_swipe + "%");
                        
                    }   

                }

                //Check if current Swipe was updated
                if (Mathf.Abs(previous_swipe) < Mathf.Abs(current_swipe)) {

                    
                    if (Mathf.Abs(rotation_around_y_axis) != Mathf.Round(Mathf.Abs(current_swipe) / 10f) * 10f) 
                    {
                        Debug.Log("Calculated Swipe: " + Mathf.Round(Mathf.Abs(current_swipe) / 10f) * 10f);
                        Debug.Log("Current rotation: " + Mathf.Abs(rotation_around_y_axis));
                        
                        rotation_around_y_axis += 10f;
                        //Debug.Log("Moving Towards: " + rotation_around_y_axis);

                        //Don't understand why this code is needed...
                        cam.transform.position = target.position;
                                        
                        //Rotates 2 degrees each frame, with sign determined by the current_swipe
                        cam.transform.Rotate(new Vector3(0, 1, 0), 10 * Mathf.Sign(current_swipe), Space.World); 

                        int buffer_up = 20;                   
                        cam.transform.Translate(new Vector3(0, buffer_up, -distance_to_target));

                        //Reset Position
                        previous_swipe = current_swipe;

                    } else { rotation_around_y_axis = previous_swipe = 0; }
                    

                }
                */
