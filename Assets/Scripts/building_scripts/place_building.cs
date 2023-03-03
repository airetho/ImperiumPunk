using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO; //Used for File Management
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using TMPro;


public class place_building : MonoBehaviour
{
    
    public GameObject current_building;
    public GameObject player;

    private bool buildable;
    public int pawn_cost;
    public int food_cost;
    public int coal_cost;
    public int gear_cost;
    public TextMeshProUGUI buildable_text; 
    public TextMeshProUGUI pawn_cost_text;
    public TextMeshProUGUI food_cost_text;
    public TextMeshProUGUI coal_cost_text;
    public TextMeshProUGUI gear_cost_text;
    public float sphere_radius = 50f;


    


    //Cost UI
    public void Start() {
        
        pawn_cost_text.text = ": " + pawn_cost.ToString();
        food_cost_text.text = ": " + food_cost.ToString();
        coal_cost_text.text = ": " + coal_cost.ToString();
        gear_cost_text.text = ": " + gear_cost.ToString();
    }
  
    public void Update() {

        

        //Check if user has enough resources
        if (
            collect_resources.pawns_stored >= pawn_cost &&
            collect_resources.food_stored >= food_cost &&
            collect_resources.coal_stored >= coal_cost &&
            collect_resources.gears_stored >= gear_cost
        ) 
        {
        
            buildable = true;
            buildable_text.text = "[O]";
            
        } else {
            buildable = false;
            buildable_text.text = "[X]";
        }
            
    } 

    public void create_building()
    {      
        
        //Check if area is clear of other buildings.
        //Detect Buildings within Radius

        Vector3 center = new Vector3(player.transform.position.x, 0.3f, player.transform.position.z);
        
        Collider[] hitColliders = Physics.OverlapSphere(center, sphere_radius);

        if(hitColliders.Length > 0){
            buildable = false; 
            Debug.Log("Buildings in the way.");
        } else {Debug.Log("Built.");}
        

        if (buildable == true)
        {
            //Pay for Building
            collect_resources.pawns_stored -= pawn_cost;
            collect_resources.food_stored -= food_cost;
            collect_resources.coal_stored -= coal_cost;
            collect_resources.gears_stored -= gear_cost;

            //Updating JSON file
            string user_save_data = Application.persistentDataPath + "/user_data.json";
            string user_data_string = File.ReadAllText(user_save_data);
            JObject data_file = JObject.Parse(user_data_string);
            
            //For Each Resource
            var a = (int)data_file["pawns"];
            a = collect_resources.pawns_stored;
            data_file["pawns"] = a;
            
            var b = (int)data_file["food"];
            b = collect_resources.food_stored;
            data_file["food"] = b;

            var c = (int)data_file["coal"];
            c = collect_resources.coal_stored;
            data_file["coal"] = c;

            var d = (int)data_file["gears"];
            d = collect_resources.gears_stored;
            data_file["gears"] = d;

            //Write Updated Values
            string account_string = data_file.ToString();
            System.IO.File.WriteAllText(user_save_data, account_string);




            //Place Building
            float z = (GameObject.Find("player").transform.position.z + GameObject.Find("main_camera").transform.position.z) / 2;
            float x = (GameObject.Find("player").transform.position.x + GameObject.Find("main_camera").transform.position.x) / 2;
            Instantiate(current_building, new Vector3(x,0.3f,z), transform.rotation);
            StartCoroutine(saving_data(x,z));
            
        } else {

            //Pop Up stating why.
            
        }
    }

    //Needed Due to the Centre Var
    IEnumerator saving_data (float x, float z) {
        
        while (true) {

            //Once Centered
            if (map_reader.centre != Vector3.zero) {

                Vector3 centre = map_reader.centre;
                
                float long_z = (float) (mercator_projection.yToLat(z + centre.z)); 
                float long_x = (float) (mercator_projection.xToLon(x + centre.x));
                
                //Add Building to Json File
                string user_save_data = Application.persistentDataPath + "/user_data.json";
                string user_data_string = File.ReadAllText(user_save_data);

                JObject data_file = JObject.Parse(user_data_string);

                JArray building_array = (JArray)data_file["building_obj"]; 


                building_array.Add(JToken.FromObject(new building_obj(){
                    building_type = current_building.name,
                    latitude = long_z,
                    longitude = long_x,  
                }));

                string account_string = data_file.ToString();
                System.IO.File.WriteAllText(user_save_data, account_string);
                
                //Kill IEnumerator once building is saved.
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