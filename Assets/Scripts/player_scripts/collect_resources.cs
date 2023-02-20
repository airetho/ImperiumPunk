using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class collect_resources : MonoBehaviour
{
    public float sphere_radius;
    public float countdown_start = 2;
    public float countdown = 2; 

    //Resources
    public TextMeshProUGUI  coal_ui;
    static public int coal_stored;
    public TextMeshProUGUI  pawn_ui;
    static public int pawns_stored;




    void Update() {

        countdown -= Time.deltaTime;   

        if(countdown  <= 0)       
        {
            resource_collecting();
            countdown = countdown_start;              
        }
    } 

    void resource_collecting() {
        
        Vector3 center = new Vector3(transform.position.x, 0.1f, transform.position.z);

        Collider[] hitColliders = Physics.OverlapSphere(center, sphere_radius);
        foreach (var hitCollider in hitColliders)
        {
            resource_generation  cs = hitCollider.gameObject.GetComponent<resource_generation>();
            if (cs.current_resource > 0)
            {
                switch (cs.resource_type)
                {
                    case "coal":
                        coal_stored += 1;
                        cs.current_resource -= 1;
                        coal_ui.text = coal_stored.ToString();
                        break;
                    case "pawn":
                        pawns_stored += 1;
                        cs.current_resource -= 1;
                        pawn_ui.text = pawns_stored.ToString();
                        break;
                }
            }
        }
    }
}
