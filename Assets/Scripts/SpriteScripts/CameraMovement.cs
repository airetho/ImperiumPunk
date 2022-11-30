using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class camera_movement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform target;
    [SerializeField] private float distance_to_target = 10;
    
    private Vector3 previous_position;
    
    void Start() 
    {
       update_position();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            previous_position = cam.ScreenToViewportPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            update_position();
        }   
    }


     private void update_position()
     {
        Vector3 new_position = cam.ScreenToViewportPoint(Input.mousePosition);
        Vector3 direction = previous_position - new_position;
            
        float rotation_around_y_axis = -direction.x * 180; // camera moves horizontally
        // float rotation_around_x_axis = direction.y * 180; // camera moves vertically
            
        cam.transform.position = target.position;
            
        //cam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
        cam.transform.Rotate(new Vector3(0, 1, 0), rotation_around_y_axis, Space.World); // <— This is what makes it work!
            
        int buffer = 26; // Pushes Camera up by 10.

        cam.transform.Translate(new Vector3(0, buffer, -distance_to_target));
            
        previous_position = new_position;
     }
}