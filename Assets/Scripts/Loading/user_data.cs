using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System.IO; //Used for File Management
using Newtonsoft.Json.Linq;

using UnityEngine.SceneManagement;



public class account //JSON File Template
{
    public string user_name { get; set; }
    public string user_empire { get; set; }
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
            JObject user_data = JObject.Parse(File.ReadAllText(@user_save_data));
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        } else { // New User 
            user_type.text = "New User";
            log.text = "Initializing New User";
        }
    }

    public void submit_data(){
         //New Account
            account account = new account
            {
                user_name = entered_user_name.text,
                user_empire = entered_empire_name.text
            };
            
            var account_string = Newtonsoft.Json.JsonConvert.SerializeObject(account); //Serialize Object
            JObject user_data = JObject.Parse(account_string);

            //string user_name = (string)user_data["user_name"]; Debug.Log(user_name);
            //string empire_name = (string)user_data["user_empire"]; Debug.Log(empire_name);
            
            System.IO.File.WriteAllText(user_save_data, account_string);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}