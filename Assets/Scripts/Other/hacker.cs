using UnityEngine;
using System.IO; //Used for File Management
using Newtonsoft.Json.Linq;

public class hacker : MonoBehaviour
{

    public GameObject self;

    // Start is called before the first frame update
    void Start()
    {
        string user_save_data = Application.persistentDataPath + "/user_data.json";
        string user_data_string = File.ReadAllText(user_save_data);
        JObject data_file = JObject.Parse(user_data_string);
        int level = (int)data_file["user_level"];

        if (level < 7) {
            self.SetActive(false);
        }
    }

    // Update is called once per frame
    public void clicked()
    {
        string user_save_data = Application.persistentDataPath + "/user_data.json";
        string user_data_string = File.ReadAllText(user_save_data);
        JObject data_file = JObject.Parse(user_data_string);
        data_file["hacker"] = true;

        string account_string = data_file.ToString();
        System.IO.File.WriteAllText(user_save_data, account_string);


    }
}
