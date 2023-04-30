using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


using System.IO; //Used for File Management
using Newtonsoft.Json.Linq;

public class read_user_data : MonoBehaviour
{
    public TextMeshProUGUI level_ui;
    public TextMeshProUGUI empire_name;
    public TextMeshProUGUI capital_name;

    // Start is called before the first frame update
    void Start()
    {
        string user_save_data = Application.persistentDataPath + "/user_data.json";
        JObject user_data = JObject.Parse(File.ReadAllText(@user_save_data));


        level_ui.text = (string)user_data["user_level"]; 
        empire_name.text = (string)user_data["empire_name"]; 
        capital_name.text = (string)user_data["capital_name"]; 
        
        
        //Set Stored Resources
        collect_resources.pawns_stored = (int)user_data["pawns"];  
        collect_resources.food_stored = (int)user_data["food"];  
        collect_resources.coal_stored = (int)user_data["coal"];  
        collect_resources.gears_stored = (int)user_data["gears"];  
        
        //Generate Buildings
        StartCoroutine(build_buildings(user_data));
    }

    private IEnumerator build_buildings (JObject user_data) {
        
        string user_save_data = Application.persistentDataPath + "/user_data.json";

        //Loop until we have centered the map. 
        while (true) {

            //Once Centered
            if (map_reader.centre != Vector3.zero) {

                //Initialize the array
                JArray building_obj = (JArray)user_data["building_obj"];

                //Loop over each building and build them.
                for (int j = 0; j < building_obj.Count; j++)  
                {

                    string building_type = (string)user_data["building_obj"][j]["building_type"];
                    float latitude = (float)user_data["building_obj"][j]["latitude"];
                    float longitude = (float)user_data["building_obj"][j]["longitude"];
                    int storage = (int)user_data["building_obj"][j]["storage"];
                    System.DateTime last_time = (System.DateTime)user_data["building_obj"][j]["date"];
                     
                    Vector3 centre = map_reader.centre;
                    
                    float z = (float) (mercator_projection.latToY(latitude) - centre.z);
                    float x = (float) (mercator_projection.lonToX(longitude) - centre.x);

                    
                    //Debug.Log("Lat:" +  latitude);
                    //Debug.Log("Long:" +  longitude);
                    //Debug.Log("Centre" + centre);
                    //Debug.Log("z-Lat:" + z);
                    //Debug.Log("x-Long" + x);

                    //The first time the data is switched, cities are wack, but from the second time on they work normal...

                    GameObject inst = Instantiate(GameObject.Find(building_type), new Vector3(x,0.3f,z), transform.rotation);

                    //Update Local Building Times
                    System.DateTime current_time = System.DateTime.Now;
                    double subtracted_time = (current_time.Subtract(last_time).TotalMinutes * 60); //Extra 60 to convert to mins.
                    inst.GetComponent<resource_generation>().elapsed_time = subtracted_time;
                    inst.GetComponent<resource_generation>().current_resource += storage;
                    inst.GetComponent<resource_generation>().building_id = j;

                    //Update System Building Times
                    double max_time = inst.GetComponent<resource_generation>().max_time;

                    //Debug.Log("Subtracted Time: " + subtracted_time);
                    //Debug.Log("Max Time: " + max_time);
    

                    if(subtracted_time > 0) {
                        while (subtracted_time > max_time) 
                        {
                            subtracted_time -= max_time; 
                        }
                    }

                    subtracted_time = ((subtracted_time * -1));
                    //Debug.Log("Subtracted Time: " + subtracted_time);

                    System.DateTime updated_time = current_time;
                    //Debug.Log("Current Time: " + updated_time);

                    updated_time = updated_time.AddSeconds(subtracted_time);

                    //Debug.Log("Updated Time: " + updated_time);

                    //Update Time: 
                    user_data["building_obj"][j]["date"] = updated_time;

                    string account_string = user_data.ToString();
                    System.IO.File.WriteAllText(user_save_data, account_string);

                };

                //Kill IEnumerator once buildings are placed.
                yield break;

            }
            else 
            {
                //Wait for a new Centre to be initialized
                yield return new WaitForSeconds(1.0f);
            } 
        }
    }
}