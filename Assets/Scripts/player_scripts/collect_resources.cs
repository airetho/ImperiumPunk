using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using System.IO; //Used for File Management
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class collect_resources : MonoBehaviour
{
    public float sphere_radius;
    public float countdown_start = 2;
    public float countdown = 2; 

    //Resources
    public TextMeshProUGUI  pawn_ui;
    static public int pawns_stored;
    public TextMeshProUGUI  food_ui;
    static public int food_stored;
    
    public TextMeshProUGUI  coal_ui;
    static public int coal_stored;

    public TextMeshProUGUI  gear_ui;
    static public int gears_stored;
    




    void Update() {

        countdown -= Time.deltaTime;   

        //Update UI
        pawn_ui.text = ": " + pawns_stored.ToString();
        food_ui.text = ": " + food_stored.ToString();
        coal_ui.text = ": " + coal_stored.ToString();
        gear_ui.text = ": " + gears_stored.ToString();

        if(countdown  <= 0)       
        {
            resource_collecting();
            countdown = countdown_start;              
        }
    } 

    void resource_collecting() {
        

        //Updating JSON file
        string user_save_data = Application.persistentDataPath + "/user_data.json";
        string user_data_string = File.ReadAllText(user_save_data);
        JObject data_file = JObject.Parse(user_data_string);


        //Detect Buildings within Radius
        Vector3 center = new Vector3(transform.position.x, 0.3f, transform.position.z);
        

        Collider[] hitColliders = Physics.OverlapSphere(center, sphere_radius);
        foreach (var hitCollider in hitColliders)
        {
            resource_generation cs = hitCollider.gameObject.GetComponent<resource_generation>();
            if (cs.current_resource > 0)
            {
                switch (cs.resource_type)
                {
                    case "pawn":
                        pawns_stored += 1;
                        cs.current_resource -= 1;
                        var a = (int)data_file["pawns"];
                        a = a += 1;
                        data_file["pawns"] = a;
                        break;    
                    case "food":
                        food_stored += 1;
                        cs.current_resource -= 1;
                        var b = (int)data_file["food"];
                        b = b += 1;
                        data_file["food"] = b;
                        break;   
                    case "coal":
                        coal_stored += 1;
                        cs.current_resource -= 1;
                        var c = (int)data_file["coal"];
                        c = c += 1;
                        data_file["coal"] = c;
                        break;
                    case "gears":
                        gears_stored += 1;
                        cs.current_resource -= 1;
                        var d = (int)data_file["gears"];
                        d = d += 1;
                        data_file["gears"] = d;
                        break;   
                }
            }
        }

        //Write Updated Values
        string account_string = data_file.ToString();
        System.IO.File.WriteAllText(user_save_data, account_string);

    }
}
