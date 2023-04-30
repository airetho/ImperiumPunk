using UnityEngine;
using System.IO; //Used for File Management
using Newtonsoft.Json.Linq;

public class start_combat : MonoBehaviour
{
    public GameObject combat_popup;
    public GameObject enemy;

    //Spawn in Enemy
    void Start() {

        string user_save_data = Application.persistentDataPath + "/user_data.json";
        string user_data_string = File.ReadAllText(user_save_data);
        JObject data_file = JObject.Parse(user_data_string);
        int level = (int)data_file["user_level"];
        //Debug.Log(level);
        double combat_chance = UnityEngine.Random.value;
        float x_pos = Random.Range(-100,100);
        float z_pos = Random.Range(-100,100);


        string current_date_string = System.DateTime.Now.ToString("yyyyMMdd");
        string last_battle_string = (string)data_file["last_battle"];
        
        //Debug.Log("Started: " + combat_chance);
        if((combat_chance < 0.67) && (level > 4) && (current_date_string != last_battle_string)) {

            //Debug.Log("Enemy Spawned!");
            transform.position = new Vector3(x_pos,0.3f,z_pos);
        }
    }

    public void Update() {

        if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
        
                if(hit.collider.gameObject == enemy)
                {
                    
                    string user_save_data = Application.persistentDataPath + "/user_data.json";
                    string user_data_string = File.ReadAllText(user_save_data);
                    JObject data_file = JObject.Parse(user_data_string);

                    string current_date_string = System.DateTime.Now.ToString("yyyyMMdd");
                    string last_battle_string = (string)data_file["last_battle"];
                    data_file["last_battle"] = current_date_string;
                    
                    string account_string = data_file.ToString();
                    System.IO.File.WriteAllText(user_save_data, account_string);
                    
                    combat_popup.SetActive(true);
                    enemy.SetActive(false);
                }
            }
        }
    }
}
