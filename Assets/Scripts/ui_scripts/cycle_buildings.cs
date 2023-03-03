using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cycle_buildings : MonoBehaviour
{

    public GameObject[] blueprint_array;
    static public int current_blueprint_num = 0;

    // Start is called before the first frame update
    public void left_cycle()
    {
        if (current_blueprint_num > 0) {

            //De-activate current blueprint
            blueprint_array[current_blueprint_num].SetActive(false);

            //Activate new blueprint
            cycle_buildings.current_blueprint_num -= 1;
            
            blueprint_array[current_blueprint_num].SetActive(true);

        }
    }

    public void right_cycle()
    {
        if (current_blueprint_num < 3) {

            //De-activate current blueprint
            blueprint_array[current_blueprint_num].SetActive(false);

            //Activate new blueprint
            cycle_buildings.current_blueprint_num += 1;
            blueprint_array[current_blueprint_num].SetActive(true);
            
        }
    }
}
