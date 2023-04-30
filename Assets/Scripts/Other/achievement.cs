using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO; //Used for File Management
using Newtonsoft.Json.Linq;

public class achievement : MonoBehaviour
{
    private AudioSource[] source;
    public GameObject ach_one;
    public GameObject ach_two;
    public GameObject ach_three;
    public GameObject ach_four;
    public GameObject ach_five;
    public GameObject ach_six;
    public GameObject ach_seven;
    public GameObject ach_eight;
    public GameObject ach_nine;
    public GameObject ach_ten;

    public GameObject ach_one_popup;
    public GameObject ach_two_popup;
    public GameObject ach_three_popup;
    public GameObject ach_four_popup;
    public GameObject ach_five_popup;
    public GameObject ach_six_popup;
    public GameObject ach_seven_popup;
    public GameObject ach_eight_popup;
    public GameObject ach_nine_popup;
    public GameObject ach_ten_popup;

    public bool is_ready = false;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponents<AudioSource>();
        string user_save_data = Application.persistentDataPath + "/user_data.json";
        JObject user_data = JObject.Parse(File.ReadAllText(@user_save_data));

        JArray achievement_obj = (JArray)user_data["achievement_obj"];

        for (int j = 0; j < achievement_obj.Count; j++)  
        {
            string ach_name = (string)user_data["achievement_obj"][j]["ach_name"];
            //Debug.Log(ach_name);

            switch (ach_name) {
                case "Emperor":
                    ach_one.SetActive(true);
                    break;
                case "Scholar":
                    ach_two.SetActive(true);
                    break;
                case "Farmer":
                    ach_three.SetActive(true);
                    break;
                case "General":
                    ach_four.SetActive(true);
                    break;
                case "HackerMan":
                    ach_five.SetActive(true);
                    break;
                case "Pilot":
                    ach_six.SetActive(true);
                    break;
                case "Patron":
                    ach_seven.SetActive(true);
                    break;
                case "Stubborn":
                    ach_eight.SetActive(true);
                    break;
                case "Rich":
                    ach_nine.SetActive(true);
                    break;
                case "Engineer":
                    ach_ten.SetActive(true);
                    break;
            }
        }

        is_ready = true;
    }

    void Update() {
        
        string user_save_data = Application.persistentDataPath + "/user_data.json";
        JObject user_data = JObject.Parse(File.ReadAllText(@user_save_data));

        JArray achievement_obj = (JArray)user_data["achievement_obj"];

        if (is_ready == true) {
            StartCoroutine(check_ach(user_data,user_save_data));
        }
        
    }

    // Update is called once per frame
    private IEnumerator check_ach(JObject user_data, string user_save_data)
    {
        //Used for multiple Achievements
        //Check for Achievement Three!
        JArray building_array = (JArray)user_data["building_obj"];
        int built_buildings = 0;

        int cities_built = 0;
        int farms_built = 0;
        int mines_built = 0;
        int windmills_built = 0;
        int factories_built = 0;
        
        

        for (int z = 0; z < building_array.Count; z++)  
        {
            built_buildings += 1;
            string building_type = (string)user_data["building_obj"][z]["building_type"];
            switch (building_type) {
                case ("city"):
                    cities_built += 1;
                    break;
                case ("farm"):
                    farms_built += 1;
                    break;
                case ("mine"):
                    mines_built += 1;
                    break;
                case ("factory"):
                    factories_built += 1;
                    break;
                case ("windmill"):
                    windmills_built += 1;
                    break;
            }
        }


        
        //Check for Achievement One!
        int current_level = (int)user_data["user_level"]; 


        if ((current_level > 9) && (ach_one.activeSelf == false)) {
            JArray achievement_obj = (JArray)user_data["achievement_obj"];
            achievement_obj.Add(JToken.FromObject(new achievement_obj(){
                    ach_name = "Emperor",
                    //Time of Creation
                    ach_date = System.DateTime.Now
            }));

            //Write Data back to File.
            string account_string = user_data.ToString();
            System.IO.File.WriteAllText(user_save_data, account_string);
            ach_one.SetActive(true); //Keeps from Looping.
            ach_one_popup.SetActive(true);
            source[1].Play();
        }

        

        //Check for Achievement Two!
        bool tutorial_achievement = open_menu.tutorial_achievement;
        //Debug.Log("Tutorial: " + tutorial_achievement);
        if (tutorial_achievement == true && current_level > 4 && ach_two.activeSelf == false) {
            JArray achievement_obj = (JArray)user_data["achievement_obj"];
            achievement_obj.Add(JToken.FromObject(new achievement_obj(){
                    ach_name = "Scholar",
                    //Time of Creation
                    ach_date = System.DateTime.Now
            }));
            //Write Data back to File.
            string account_string = user_data.ToString();
            System.IO.File.WriteAllText(user_save_data, account_string);
            ach_two.SetActive(true); //Keeps from Looping.
            ach_two_popup.SetActive(true);
            source[1].Play();
        }





        if ((farms_built > 12) && (ach_three.activeSelf == false)) {
            Debug.Log("");
            JArray achievement_obj = (JArray)user_data["achievement_obj"];
            achievement_obj.Add(JToken.FromObject(new achievement_obj(){
                    ach_name = "Farmer",
                    //Time of Creation
                    ach_date = System.DateTime.Now
            }));

            //Write Data back to File.
            string account_string = user_data.ToString();
            System.IO.File.WriteAllText(user_save_data, account_string);
            ach_three.SetActive(true); //Keeps from Looping.
            ach_three_popup.SetActive(true);
            source[1].Play();
        }



        //Check for Achievement Four!
        int battles_fought = (int)user_data["battles"]; 
        
        if ((battles_fought > 24) && (ach_four.activeSelf == false)) {
            
            JArray achievement_obj = (JArray)user_data["achievement_obj"];
            achievement_obj.Add(JToken.FromObject(new achievement_obj(){
                    ach_name = "General",
                    //Time of Creation
                    ach_date = System.DateTime.Now
            }));

            //Write Data back to File.
            string account_string = user_data.ToString();
            System.IO.File.WriteAllText(user_save_data, account_string);
            ach_four.SetActive(true); //Keeps from Looping.
            ach_four_popup.SetActive(true);
            source[1].Play();
        }



        //Check for Achievement Five!
        bool haker = (bool)user_data["hacker"]; 
        
        if ((haker ==  true) && (ach_five.activeSelf == false)) {
            
            JArray achievement_obj = (JArray)user_data["achievement_obj"];
            achievement_obj.Add(JToken.FromObject(new achievement_obj(){
                    ach_name = "HackerMan",
                    //Time of Creation
                    ach_date = System.DateTime.Now
            }));

            //Write Data back to File.
            string account_string = user_data.ToString();
            System.IO.File.WriteAllText(user_save_data, account_string);
            ach_five.SetActive(true); //Keeps from Looping.
            ach_five_popup.SetActive(true);
            source[1].Play();
        }


        //Check for Achievement Six!
        int flights_taken = switch_camera_mode.achievement_camera;
        
        if ((flights_taken > 9) && (ach_six.activeSelf == false)) {
            
            JArray achievement_obj = (JArray)user_data["achievement_obj"];
            achievement_obj.Add(JToken.FromObject(new achievement_obj(){
                    ach_name = "Pilot",
                    //Time of Creation
                    ach_date = System.DateTime.Now
            }));

            //Write Data back to File.
            string account_string = user_data.ToString();
            System.IO.File.WriteAllText(user_save_data, account_string);
            ach_six.SetActive(true); //Keeps from Looping.
            ach_six_popup.SetActive(true);
            source[1].Play();
        }




        //Check for Achievement Seven!
        if ((windmills_built > 0) && (ach_seven.activeSelf == false)) {
            
            Debug.Log("");
            JArray achievement_obj = (JArray)user_data["achievement_obj"];
            achievement_obj.Add(JToken.FromObject(new achievement_obj(){
                    ach_name = "Patron",
                    //Time of Creation
                    ach_date = System.DateTime.Now
            }));

            //Write Data back to File.
            string account_string = user_data.ToString();
            System.IO.File.WriteAllText(user_save_data, account_string);
            ach_seven.SetActive(true); //Keeps from Looping.
            ach_seven_popup.SetActive(true);
            source[1].Play();
        }




        //Check for Achievement Eight!
        int stubborn_int = place_building.achievement_stubborn;

        if ((stubborn_int > 9) && (ach_eight.activeSelf == false)) {
            
            JArray achievement_obj = (JArray)user_data["achievement_obj"];
            achievement_obj.Add(JToken.FromObject(new achievement_obj(){
                    ach_name = "Stubborn",
                    //Time of Creation
                    ach_date = System.DateTime.Now
            }));

            //Write Data back to File.
            string account_string = user_data.ToString();
            System.IO.File.WriteAllText(user_save_data, account_string);
            ach_eight.SetActive(true); //Keeps from Looping.
            ach_eight_popup.SetActive(true);
            source[1].Play();
        }






        //Check for Achievement Nine!
        int pawns_stored = (int)user_data["pawns"];  
        int food_stored = (int)user_data["food"];  
        int coal_stored = (int)user_data["coal"];  
        int gears_stored = (int)user_data["gears"];  
        
        
        if ((pawns_stored > 9999) || (food_stored > 9999) || (coal_stored > 9999) || (gears_stored > 9999)) {
            if (ach_nine.activeSelf == false) {
                JArray achievement_obj = (JArray)user_data["achievement_obj"];
                achievement_obj.Add(JToken.FromObject(new achievement_obj(){
                    ach_name = "Rich",
                    //Time of Creation
                    ach_date = System.DateTime.Now
                }));

                //Write Data back to File.
                string account_string = user_data.ToString();
                System.IO.File.WriteAllText(user_save_data, account_string);
                ach_nine.SetActive(true); //Keeps from Looping.
                ach_nine_popup.SetActive(true);
                source[1].Play();
            }
        }


        //Check for Achievement Ten!
        if ((cities_built > 4) && (farms_built > 4) && (mines_built > 4) && (factories_built > 4) && (ach_ten.activeSelf == false )) {
                JArray achievement_obj = (JArray)user_data["achievement_obj"];
                achievement_obj.Add(JToken.FromObject(new achievement_obj(){
                    ach_name = "Engineer",
                    //Time of Creation
                    ach_date = System.DateTime.Now
                }));

                //Write Data back to File.
                string account_string = user_data.ToString();
                System.IO.File.WriteAllText(user_save_data, account_string);
                ach_ten.SetActive(true); //Keeps from Looping.
                ach_ten_popup.SetActive(true);
                source[1].Play();
        }





        //Wait a bit before checking again for achievements.
        yield return new WaitForSeconds(5.0f);

    }
}
