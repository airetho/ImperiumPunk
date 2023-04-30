using UnityEngine;
using System;

using System.IO; //Used for File Management
using Newtonsoft.Json.Linq;
using TMPro;

public class game_controller : LP2
{
    public GameObject battle_screen;
    public GameObject result_screen;
    public TextMeshProUGUI battle_string_GUI;
    public TextMeshProUGUI battle_result_GUI;

    //static public int pawns_stored;
    //static public int food_stored;
    //static public int coal_stored;
    //static public int gears_stored;
    public TextMeshProUGUI  pawn_ui;
    public TextMeshProUGUI  food_ui;
    public TextMeshProUGUI  coal_ui;
    public TextMeshProUGUI  gears_ui;



    public double[,] payoffs = { 
        //This AI chooses COLUMNS in order to MAXIMIZE the payoff it gets against any choice of row
        //Assault Hold Flank Gas Airstrike Bombard
        
        { .5, .6, .4, .4, .6, .5 }, //Assault
        { .4, .5, .6, .8, .3, .6 }, //Hold
        { .6, .4, .5, .4, .7, .3 }, //Flank
        { .6, .2, .6, .5, .7, .4 }, //Gas 
        { .4, .7, .3, .3, .5, .8 }, //Airstrike
        { .5, .4, .7, .6, .2, .5 }  //Bombard
    };

    public int[,] battle_array = { 
        //This AI chooses COLUMNS in order to MAXIMIZE the payoff it gets against any choice of row
        //Assault Hold Flank Gas Airstrike Bombard
        //x player / y ai
        
        { 0, -1, 1, 1, -1, 0 }, //Assault
        { 1, 0, -1, -3, 2, -1 }, //Hold
        { -1, 1, 0, 1, -2, 2 }, //Flank
        { -1, 3, -1, 0, -2, 1 }, //Gas 
        { 1, -2, 2, 2, 0, -3 }, //Airstrike
        { 0, 1, -2, -1, 3, 0 }  //Bombard
    };

    private double ai_assault_chance;
    private double ai_flank_chance;
    private double ai_hold_chance;
    private double ai_gas_chance;
    private double ai_bombard_chance;
    private double ai_airstrike_chance;
    private string player_move_string;


    public void assault_button()
    {
        player_move_string = "assault";
        enemy_move_ai(0);
    }

    public void hold_button()
    {
        player_move_string = "hold";
        enemy_move_ai(1);
    }

    public void flank_button()
    {
        player_move_string = "flank";
        enemy_move_ai(2);
    }
    public void gas_button()
    {
        player_move_string = "gassing";
        enemy_move_ai(3);
    }
       public void airstrike_button()
    {
        player_move_string = "bombing";
        enemy_move_ai(4);
    }
    public void bombard_button()
    {
        player_move_string = "shelling";
        enemy_move_ai(5);
    }
 




    //payoffs: original payoff matrix
    //bias: the index of the option to make more likely to happen (or less likely)
    //biasAmount: the factor to scale the weight by. 1 means no change, 0 means impossible
    //aggressiveness: how much the AI will gun for the biggest payoffs. 0 is optimal, positive is too risky, negative is too safe

    private double[] LP_spicy(double[,] payoffs, int bias, double biasAmount, double aggressiveness) {
        double[,] temppayoffs = new double[payoffs.GetLength(0),payoffs.GetLength(1)];
        //scales payoff matrix values according to aggressiveness
        for(int i = 0; i < payoffs.GetLength(0); i++) {
            for(int j = 0; j < payoffs.GetLength(1); j++) {
                double temp = payoffs[i,j];
                temppayoffs[i,j] = Math.Pow(temp, Math.Pow(2,aggressiveness));
            }
        }
        double[] x = LPinator(temppayoffs);
        double sum = 1; //tracks sum of vector components
        sum += x[bias]*(biasAmount-1);
        x[bias] *= biasAmount; //scale
        for(int i = 0; i < x.Length; i++) { //normalize
            x[i] /= sum;
        }
        return x;
    }



    private void enemy_move_ai(int player_move)
    {
        int enemy_move = -1;

        double personality1 = UnityEngine.Random.value; //risk vs safe meta-playstyle
        double personality2 = UnityEngine.Random.value; //specific option preferences

        String personality_string = "";

        //personality probabilities
        double[] prob1 = {.05, .3, .7, .95}; //Cumaltive :)
        double[] prob2 = {.4,.5,.6,.7,.8,.9}; //defaults used if p>.95 or .9, do not change
        
        int personality1_index = prob1.Length;
        int personality2_index = prob2.Length;
        String[] personality1_names = {"Cowardly", "Cautious", "Prepared", "Risky", "Unhinged"}; //also seriously considered: "very safe" "very risky"
        String[] personality2_names = {"Strategic", "Aggressive", "Defensive", "Cunning", "Brutal", "Detached", "Punctual"};



        //convert roll to a specific option
        for(int i = 0; i < prob1.Length; i++) {
            if(personality1 < prob1[i]) {
                personality1_index = i;
                break;
            }
        }
        for(int i = 0; i < prob2.Length; i++) {
            if(personality2 < prob2[i]) {
                personality2_index = i;
                break;
            }
        }

        /*
        //sets the personality name: personality_string
        if(personality1_index == 0 && personality2_index == 0) {
            personality_string = personality1_names[personality1_index] + " & " + personality2_names[personality2_index]; //16% chance
        } else if (personality1_index == 0 || personality2_index == 0) {
            personality_string = personality1_names[personality1_index] + " & " + personality2_names[personality2_index]; //48% chance
        } else {
            personality_string = personality1_names[personality1_index] + " & " + personality2_names[personality2_index]; //36% chance
        }
        */

        personality_string = personality1_names[personality1_index] + " & " + personality2_names[personality2_index];
        
        Debug.Log(personality_string);

        //gets the move probabilities
        double agg = personality1_index - 2;
        agg = agg/2;
        int bias_index = 0;
        double bias_amount = 1;
        if(personality2_index != 0) {
            bias_amount = 2;
            bias_index = personality2_index-1;
        }
        
        double[] x = LP_spicy(payoffs, bias_index, bias_amount, agg);


        double random_move = UnityEngine.Random.value; 

        for(int i=0; i < x.Length; i++) {
            //Debug.Log("Probability of option " + (i+1) + ": " + x[i]);
            switch (i+1) {
                case 1:
                    ai_assault_chance =  Math.Round(x[i],3);
                    Debug.Log("ai_assault_chance: " + ai_assault_chance);
                    break;
                case 2:
                    ai_hold_chance =  Math.Round(x[i],3);
                    Debug.Log("ai_hold_chance: " + ai_hold_chance);
                    break;
                case 3:
                    ai_flank_chance =  Math.Round(x[i],3);
                    Debug.Log("ai_flank_chance: " + ai_flank_chance);
                    break;
                case 4:
                    ai_gas_chance =  Math.Round(x[i],3);
                    Debug.Log("ai_gas_chance: " + ai_gas_chance);
                    break;
                case 5:
                    ai_airstrike_chance =  Math.Round(x[i],3);
                    Debug.Log("ai_airstrike_chance: " + ai_airstrike_chance);
                    break;
                case 6:
                    ai_bombard_chance =  Math.Round(x[i],3);
                    Debug.Log("ai_bombard_chance: " + ai_bombard_chance);
                    break;
            }
        }

        //Ugly Code :(
        if (random_move < ai_assault_chance) 
        {
            enemy_move = 0;
        } else if (random_move < ai_assault_chance + ai_flank_chance) {
            enemy_move = 1;
        } else if (random_move < ai_assault_chance + ai_flank_chance + ai_hold_chance) {
            enemy_move = 2;
        } else if (random_move < ai_assault_chance + ai_flank_chance + ai_hold_chance + ai_gas_chance) {
            enemy_move = 3;
        } else if (random_move < ai_assault_chance + ai_flank_chance + ai_hold_chance + ai_gas_chance + ai_airstrike_chance) {
            enemy_move = 4;
        } else {
            enemy_move = 5;
        }


        
        string enemy_move_string = "";

        switch(enemy_move) {
            case 0:
                enemy_move_string = "assault";
                break;
            case 1:
                enemy_move_string = "holding against";
                break;
            case 2:
                enemy_move_string = "flank";
                break;
            case 3:
                enemy_move_string = "gas";
                break;
            case 4:
                enemy_move_string = "bomb";
                break;
            case 5:
                enemy_move_string = "shell";
                break;
        }


        Debug.Log("The enemy picked: " + enemy_move_string);
        Debug.Log("The player picked: " + player_move);
        
        int result = battle_array[player_move,enemy_move];
        string battle_string = "";
        string battle_result = "";
        
        string user_save_data = Application.persistentDataPath + "/user_data.json";
        JObject data_file = JObject.Parse(File.ReadAllText(@user_save_data));

        string user_name = (string)data_file["user_name"];
        string empire_name = (string)data_file["empire_name"];
        string capital_name = (string)data_file["capital_name"];


        
        result_screen.SetActive(true);

        int a = 0;
        int b = 0;
        int c = 0;
        int d = 0;
        string account_string = "";
        int battles = (int)data_file["battles"];
        data_file["battles"] = (battles + 1);

        switch (result) {


            case -3:
                battle_string = ("Today, " + System.DateTime.Now.ToString("MM/dd/yyyy") + ", at " +  System.DateTime.Now.ToString("HH:mm") +
                ", " + empire_name + " suffered a catastrophic defeat against a rival metropolis. Their leader " + personality_string + 
                " decided to " + enemy_move_string + " us. Our " + player_move_string 
                + ", was foolhardy,resulting in our rout.");
                battle_result = ("Total Defeat");

                a = (int)data_file["pawns"];
                a = a - 60;
                if (a < 0) {a = 0;}
                data_file["pawns"] = a;
                pawn_ui.text = ": " + a.ToString();

                b = (int)data_file["food"];
                b = b - 30;
                if (b < 0) {b = 0;}
                data_file["food"] = b;
                food_ui.text = ": " + b.ToString();

                c = (int)data_file["coal"];
                c = c - 300;
                if (c < 0) {c = 0;}
                data_file["coal"] = c;
                coal_ui.text = ": " + c.ToString();

                d = (int)data_file["gears"];
                d = d - 3;
                if (d < 0) {d = 0;}
                data_file["gears"] = d;
                gears_ui.text = ": " + d.ToString();

                //Set Stored Resources
                collect_resources.pawns_stored = (int)data_file["pawns"];  
                collect_resources.food_stored = (int)data_file["food"];  
                collect_resources.coal_stored = (int)data_file["coal"];  
                collect_resources.gears_stored = (int)data_file["gears"];  

                account_string = data_file.ToString();
                System.IO.File.WriteAllText(user_save_data, account_string);


               
                break;
            case -2:
                battle_string = ("Today, " + System.DateTime.Now.ToString("MM/dd/yyyy") + ", at " +  System.DateTime.Now.ToString("HH:mm") +
                ", " + empire_name + " yielded to a raiding city after a bitter defeat. Their leader " + personality_string + 
                " decided to " + enemy_move_string + " us. Our " + player_move_string + ", proved ineffectual, losing us the battle.");
                battle_result = ("Humiliating Defeat");

                a = (int)data_file["pawns"];
                a = a - 40;
                if (a < 0) {a = 0;}
                data_file["pawns"] = a;
                pawn_ui.text = ": " + a.ToString();

                b = (int)data_file["food"];
                b = b - 20;
                if (b < 0) {b = 0;}
                data_file["food"] = b;
                food_ui.text = ": " + b.ToString();

                c = (int)data_file["coal"];
                c = c - 200;
                if (c < 0) {c = 0;}
                data_file["coal"] = c;
                coal_ui.text = ": " + c.ToString();

                d = (int)data_file["gears"];
                d = d - 2;
                if (d < 0) {d = 0;}
                data_file["gears"] = d;
                gears_ui.text = ": " + d.ToString();

                //Set Stored Resources
                collect_resources.pawns_stored = (int)data_file["pawns"];  
                collect_resources.food_stored = (int)data_file["food"];  
                collect_resources.coal_stored = (int)data_file["coal"];  
                collect_resources.gears_stored = (int)data_file["gears"];  

                account_string = data_file.ToString();
                System.IO.File.WriteAllText(user_save_data, account_string);


                break;
            case -1:
                battle_string = ("Today, " + System.DateTime.Now.ToString("MM/dd/yyyy") + ", at " +  System.DateTime.Now.ToString("HH:mm") +
                ", " + empire_name + " suffered inexcusable damage when eradicating a local village. Their leader " + personality_string + 
                " decided to " + enemy_move_string + " us. Our attempted " + player_move_string + ", failed.");
                battle_result = ("Pyrrhic Victory");

                a = (int)data_file["pawns"];
                a = a - 20;
                if (a < 0) {a = 0;}
                data_file["pawns"] = a;
                pawn_ui.text = ": " + a.ToString();

                b = (int)data_file["food"];
                b = b - 10;
                if (b < 0) {b = 0;}
                data_file["food"] = b;
                food_ui.text = ": " + b.ToString();

                c = (int)data_file["coal"];
                c = c - 100;
                if (c < 0) {c = 0;}
                data_file["coal"] = c;
                coal_ui.text = ": " + c.ToString();

                d = (int)data_file["gears"];
                d = d - 1;
                if (d < 0) {d = 0;}
                data_file["gears"] = d;
                gears_ui.text = ": " + d.ToString();

                //Set Stored Resources
                collect_resources.pawns_stored = (int)data_file["pawns"];  
                collect_resources.food_stored = (int)data_file["food"];  
                collect_resources.coal_stored = (int)data_file["coal"];  
                collect_resources.gears_stored = (int)data_file["gears"];  

                account_string = data_file.ToString();
                System.IO.File.WriteAllText(user_save_data, account_string);

                break;
            case 0:
                battle_string = ("Today, " + System.DateTime.Now.ToString("MM/dd/yyyy") + ", at " +  System.DateTime.Now.ToString("HH:mm") +
                ", " + empire_name + " fought against a respectable municipality to a standstill. Their leader " + personality_string + 
                " decided to " + enemy_move_string + " us. Our " + player_move_string + ", resulted in deadlock.");
                battle_result = ("Stalemate");
                break;
            case 1:
                battle_string = ("Today, " + System.DateTime.Now.ToString("MM/dd/yyyy") + ", at " +  System.DateTime.Now.ToString("HH:mm") +
                ", " + empire_name + " crushed a minor townlet in the field of battle. Their leader " + personality_string + 
                " desperately decided to " + enemy_move_string + " us. Our " + player_move_string + ", countered effectively, finishing the enemy.");
                battle_result = ("Well-Earned Victory");

                a = (int)data_file["pawns"];
                a = a + 40;
                if (a < 0) {a = 0;}
                data_file["pawns"] = a;
                pawn_ui.text = ": " + a.ToString();

                b = (int)data_file["food"];
                b = b + 20;
                if (b < 0) {b = 0;}
                data_file["food"] = b;
                food_ui.text = ": " + b.ToString();

                c = (int)data_file["coal"];
                c = c + 200;
                if (c < 0) {c = 0;}
                data_file["coal"] = c;
                coal_ui.text = ": " + c.ToString();

                d = (int)data_file["gears"];
                d = d + 2;
                if (d < 0) {d = 0;}
                data_file["gears"] = d;
                gears_ui.text = ": " + d.ToString();

                //Set Stored Resources
                collect_resources.pawns_stored = (int)data_file["pawns"];  
                collect_resources.food_stored = (int)data_file["food"];  
                collect_resources.coal_stored = (int)data_file["coal"];  
                collect_resources.gears_stored = (int)data_file["gears"];  

                account_string = data_file.ToString();
                System.IO.File.WriteAllText(user_save_data, account_string);

                break;
            case 2:
                battle_string = ("Today, " + System.DateTime.Now.ToString("MM/dd/yyyy") + ", at " +  System.DateTime.Now.ToString("HH:mm") +
                ", " + empire_name + " annihilated a notable suburb. Their leader " + personality_string + 
                " decided to " + enemy_move_string + " us. Our " + player_move_string + ", completely broke the enemy.");
                battle_result = ("Utter Rout");

                a = (int)data_file["pawns"];
                a = a + 80;
                if (a < 0) {a = 0;}
                data_file["pawns"] = a;
                pawn_ui.text = ": " + a.ToString();

                b = (int)data_file["food"];
                b = b + 40;
                if (b < 0) {b = 0;}
                data_file["food"] = b;
                food_ui.text = ": " + b.ToString();

                c = (int)data_file["coal"];
                c = c + 400;
                if (c < 0) {c = 0;}
                data_file["coal"] = c;
                coal_ui.text = ": " + c.ToString();

                d = (int)data_file["gears"];
                d = d + 4;
                if (d < 0) {d = 0;}
                data_file["gears"] = d;
                gears_ui.text = ": " + d.ToString();

                //Set Stored Resources
                collect_resources.pawns_stored = (int)data_file["pawns"];  
                collect_resources.food_stored = (int)data_file["food"];  
                collect_resources.coal_stored = (int)data_file["coal"];  
                collect_resources.gears_stored = (int)data_file["gears"];

                account_string = data_file.ToString();
                System.IO.File.WriteAllText(user_save_data, account_string);



                break;
            case 3:
                battle_string = ("Today, " + System.DateTime.Now.ToString("MM/dd/yyyy") + ", at " +  System.DateTime.Now.ToString("HH:mm") +
                ", " + empire_name + "obliterated a rival city. Their pathetic leader " + personality_string + 
                "decided to " + enemy_move_string + " us. In a stroke of genius, our " + player_move_string + " crushed our rival.");
                battle_result = ("Perfect Victory");

                a = (int)data_file["pawns"];
                a = a + 120;
                if (a < 0) {a = 0;}
                data_file["pawns"] = a;
                pawn_ui.text = ": " + a.ToString();

                b = (int)data_file["food"];
                b = b + 60;
                if (b < 0) {b = 0;}
                data_file["food"] = b;
                food_ui.text = ": " + b.ToString();

                c = (int)data_file["coal"];
                c = c + 600;
                if (c < 0) {c = 0;}
                data_file["coal"] = c;
                coal_ui.text = ": " + c.ToString();

                d = (int)data_file["gears"];
                d = d + 6;
                if (d < 0) {d = 0;}
                data_file["gears"] = d;
                gears_ui.text = ": " + d.ToString();

                //Set Stored Resources
                collect_resources.pawns_stored = (int)data_file["pawns"];  
                collect_resources.food_stored = (int)data_file["food"];  
                collect_resources.coal_stored = (int)data_file["coal"];  
                collect_resources.gears_stored = (int)data_file["gears"];  

                account_string = data_file.ToString();
                System.IO.File.WriteAllText(user_save_data, account_string);


                break;
            
        }

        Debug.Log(battle_string);
        Debug.Log(battle_result);

        battle_string_GUI.SetText(battle_string);
        battle_result_GUI.SetText(battle_result);

        battle_screen.SetActive(false);
    }
}
