using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


using System.IO; //Used for File Management
using Newtonsoft.Json.Linq;
using System.Text.Json;

public class read_user_data : MonoBehaviour
{
    public TextMeshProUGUI level_ui;
    public TextMeshProUGUI empire_name;

    // Start is called before the first frame update
    void Start()
    {
        string user_save_data = Application.persistentDataPath + "/user_data.json";
        JObject user_data = JObject.Parse(File.ReadAllText(@user_save_data));


        level_ui.text = (string)user_data["user_level"]; 
        empire_name.text = (string)user_data["user_empire"]; 
        StartCoroutine(build_buildings(user_data));
    }

    private IEnumerator build_buildings (JObject user_data) {

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
                         
                    Vector3 centre = map_reader.centre;
                    
                    float z = (float) (mercator_projection.latToY(latitude) - centre.z);
                    float x = (float) (mercator_projection.lonToX(longitude) - centre.x);

                    Instantiate(GameObject.Find(building_type), new Vector3(x,0,z), transform.rotation);
                        
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