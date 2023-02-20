using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO; //Used for File Management
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



public class place_building : MonoBehaviour
{
    
    public GameObject current_building;
 

    public void create_building()
    {      

        //Place Building
        float z = (GameObject.Find("player").transform.position.z + GameObject.Find("main_camera").transform.position.z) / 2;
        float x = (GameObject.Find("player").transform.position.x + GameObject.Find("main_camera").transform.position.x) / 2;
        Instantiate(current_building, new Vector3(x,0,z), transform.rotation);

        StartCoroutine(saving_data(x,z));

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

                JArray  building_array = (JArray)data_file["building_obj"]; 


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

    