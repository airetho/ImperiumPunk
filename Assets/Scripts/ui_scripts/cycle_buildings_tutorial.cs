using UnityEngine;

using System.IO; //Used for File Management
using Newtonsoft.Json.Linq;

public class cycle_buildings_tutorial : MonoBehaviour
{

    public GameObject[] canvas_array;
    static public int current_canvas_num = 0;

    

    // Start is called before the first frame update
    public void left_cycle()
    {
        if (current_canvas_num > 0) {

            //De-activate current blueprint
            
            canvas_array[current_canvas_num].SetActive(false);
            

            //Activate new blueprint
            cycle_buildings_tutorial.current_canvas_num -= 1;
            
            canvas_array[current_canvas_num].SetActive(true);

        }
    }

    public void right_cycle()
    {
        if (current_canvas_num < 7) {

            //De-activate current canvas
            canvas_array[current_canvas_num].SetActive(false);

            
            //Activate new canvas
            cycle_buildings_tutorial.current_canvas_num += 1;
            canvas_array[current_canvas_num].SetActive(true);
        }
    }

    public void reset_num()
    {
        cycle_buildings_tutorial.current_canvas_num = 0;
    }
}
