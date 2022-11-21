using UnityEngine;

[RequireComponent(typeof(map_reader))]
abstract class infrastructure_behaviour : MonoBehaviour 
{
    protected map_reader map;

    void Awake()
    {
        map = GetComponent<map_reader>();
    }


    protected Vector3 GetCentre(osm_way way)
    {
        Vector3 total = Vector3.zero;
    
        foreach(var id in way.node_ids) 
        {
            total += map.nodes[id];
        }

        return total / way.node_ids.Count;
    }
}


