using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System.IO; //Used for File Management
using Newtonsoft.Json.Linq;

using UnityEngine.SceneManagement;




public class building_obj 
{
    public string building_type { get; set; }
    public float latitude { get; set; }
    public float longitude { get; set; }
}

public class account //JSON File Template
{
    public string user_name { get; set; }
    public string user_empire { get; set; }
    public int user_level { get; set; }
    public int buildings { get; set; }
    public List<building_obj> building_obj { get; set; }
    
}



public class user_data : MonoBehaviour
{
    public TextMeshProUGUI  user_type;
    public TextMeshProUGUI  log;
    public TextMeshProUGUI entered_user_name;
    public TextMeshProUGUI entered_empire_name;
    string user_save_data;


    //Check for User Data
    void Awake()
    {
        log.text = "Initializing";
        user_save_data = Application.persistentDataPath + "/user_data.json";
        read_file();
    }
    
    
    void read_file()
    {   

        log.text = "Checking File";

        // Does the file exist?  
        if (File.Exists(@user_save_data)) // Old User
        {
            user_type.text = "Returning User";
            log.text = "Reading File";

            //Read from File
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        } else { // New User 
            user_type.text = "New User";
            log.text = "Initializing New User";
        }
    }

    public void submit_data(){
            

        var object_to_serialize = new account();

        object_to_serialize = new account{
            user_name = entered_user_name.text,
            user_empire = entered_empire_name.text,
            user_level = 1,
            buildings = 1,
        };
        
        object_to_serialize.building_obj = new List<building_obj>
        {
            new building_obj 
            {
                building_type = "coal_mine",
                latitude = 37.710640F,
                longitude = -89.224582F,
            },

             new building_obj 
            {
                building_type = "coal_mine",
                latitude = 37.710018F,
                longitude = -89.223252F,
            }
        };

        user_save_data = Application.persistentDataPath + "/user_data.json"; //Just to make sure user_save_data has been initialized.
            
        var account_string = Newtonsoft.Json.JsonConvert.SerializeObject(object_to_serialize); //Serialize Object
            

        //string user_name = (string)user_data["user_name"]; Debug.Log(user_name);
        //string empire_name = (string)user_data["user_empire"]; Debug.Log(empire_name);
            
        System.IO.File.WriteAllText(user_save_data, account_string);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}