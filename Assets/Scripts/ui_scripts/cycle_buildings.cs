using UnityEngine;

using System.IO; //Used for File Management
using Newtonsoft.Json.Linq;

public class cycle_buildings : MonoBehaviour
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
            cycle_buildings.current_canvas_num -= 1;
            
            canvas_array[current_canvas_num].SetActive(true);

        }
    }

    public void right_cycle()
    {
    
        //Debug.Log("Building Checked!");
        //Used to keep the player from building "buildings" out of order.
        string user_save_data = Application.persistentDataPath + "/user_data.json";
        string user_data_string = File.ReadAllText(user_save_data);
        JObject data_file = JObject.Parse(user_data_string);

        int level = (int)data_file["user_level"];
            
        //Add Level
        if (current_canvas_num < 4 && ((current_canvas_num + 1 ) < level) )   {
                
            //De-activate current canvas
            canvas_array[current_canvas_num].SetActive(false);

            //Activate new canvas
            cycle_buildings.current_canvas_num += 1;
            canvas_array[current_canvas_num].SetActive(true);
            
        }
    }
}
