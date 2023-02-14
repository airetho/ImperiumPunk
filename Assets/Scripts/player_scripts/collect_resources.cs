using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class collect_resources : MonoBehaviour
{
    public float sphere_radius;
    public TextMeshProUGUI  coal_ui;
    static public int coal_stored;

    public float countdown_start = 2;
    public float countdown = 2; 



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
        Debug.Log(center);

        Collider[] hitColliders = Physics.OverlapSphere(center, sphere_radius);
        foreach (var hitCollider in hitColliders)
        {
            resource_generation  cs = hitCollider.gameObject.GetComponent<resource_generation>();
            if (cs.current_resource > 0)
            {
                coal_stored += 1;
                cs.current_resource -= 1;
                coal_ui.text = coal_stored.ToString();
            }

            Debug.Log(hitCollider.gameObject);
           
        }

    }
}
