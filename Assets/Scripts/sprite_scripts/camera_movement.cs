using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_movement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] public Transform player;
    [SerializeField] public static string camera_state = "low"; //all camera_states: "low" | "high"

    public float distance_to_target;
    //"low" swipe var's
    private Vector2 previous_position;
    private GameObject camera_parent;
    public float rotation_speed;
    public float zoom_speed;
    public float min_clamp;
    public float max_clamp;

    // Declare variables for pinch zoom
    private float previousTouchDeltaMagnitude;
    private float currentTouchDeltaMagnitude;

    void Start()
    {
        Transform originalParent = transform.parent;
        camera_parent = new GameObject("camera");
        transform.parent = camera_parent.transform;
        camera_parent.transform.parent = originalParent;
        camera_dist();
    }

    void Update()
    {
        switch (camera_state)
        {
            //"low" or City View
            case "low":
                swipe_control();
                camera_dist();
            break;
            //"high" or Empire View
            case "high":
                swipe_control();
            break;
        }
    }

    void camera_dist()
    {
        cam.transform.position = player.position;
        camera_parent.transform.position = player.position;
        int buffer_up = 20;
        cam.transform.Translate(new Vector3(0, buffer_up, -distance_to_target));
    }

   void swipe_control()
    {
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
                        float x_difference = touch.position.x - previous_position.x;

                        if (x_difference != 0)
                        {
                            float angle = Mathf.Rad2Deg * x_difference / distance_to_target;
                            camera_parent.transform.Rotate(Vector3.up * angle);
                        }
                        previous_position = touch.position;
                    }

                    if (Input.touchCount == 2)
                    {
                        Touch touch1 = Input.GetTouch(0);
                        Touch touch2 = Input.GetTouch(1);

                        Vector2 touch1_previous_pos = touch1.position - touch1.deltaPosition;
                        Vector2 touch2_previous_pos = touch2.position - touch2.deltaPosition;

                        float prev_magnitude = (touch1_previous_pos - touch2_previous_pos).magnitude;
                        float current_magnitude = (touch1.position - touch2.position).magnitude;
                        float difference = current_magnitude - prev_magnitude;

                        if (Mathf.Abs(difference) > 0.01f)
                        {
                            cam.transform.Translate(new Vector3(0, 0, difference * 0.1f));
                            distance_to_target = Mathf.Clamp(distance_to_target - difference * zoom_speed, min_clamp, max_clamp);
                        }
                    }
                }

                if (touch.phase == TouchPhase.Ended && touch.fingerId == 0)
                {
                    previous_position = touch.position;
                }
            }
        }
        camera_dist();
    }
}