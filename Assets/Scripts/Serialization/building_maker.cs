using UnityEngine;
using System.Collections;
using System.Collections.Generic;


class building_maker : infrastructure_behaviour
{
    public Material building_material;

    public static bool buildings_built = false;

    IEnumerator Start()
    {
    
        while (!map.is_ready) 
        {
            yield return null;
        }

        //TODO: Processs Map Data to create Buildings;
        foreach(var way in map.ways.FindAll((W)=> {return W.is_boundary && W.node_ids.Count > 1;})) //Find all Building Boundaries 
        {
            GameObject go = new GameObject();
            Vector3 localOrigin = GetCentre(way);
            go.transform.position = localOrigin - map.bounds.centre;;

            MeshFilter mf = go.AddComponent<MeshFilter>();
            MeshRenderer mr = go.AddComponent<MeshRenderer>(); 

            mr.material = building_material;

            List<Vector3> vectors = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> indicies = new List<int>();

            for (int i = 1; i < way.node_ids.Count; i ++)
            {
                osm_node p1 = map.nodes[way.node_ids[i - 1]];
                osm_node p2 = map.nodes[way.node_ids[i]];

                Vector3 s1 = p1 - localOrigin;
                Vector3 s2 = p2 - localOrigin;

                Vector3 diff = (s2 - s1).normalized;
                var cross = Vector3.Cross(diff, Vector3.up) * 2.0f; //2 Meters = Width of Lane

                Vector3 v1 = s1 + cross;
                Vector3 v2 = s1 - cross;
                Vector3 v3 = s2 + cross;
                Vector3 v4 = s2 - cross;


                vectors.Add(v1 + new Vector3(0,0.2f,0));
                vectors.Add(v2 + new Vector3(0,0.2f,0));
                vectors.Add(v3 + new Vector3(0,0.2f,0));
                vectors.Add(v4 + new Vector3(0,0.2f,0));


                //Allows for Sprites on Roads
                uvs.Add(new Vector2(0,0));
                uvs.Add(new Vector2(1,0));
                uvs.Add(new Vector3(0,1));
                uvs.Add(new Vector3(1,1));



    
                normals.Add(-Vector3.up);
                normals.Add(-Vector3.up);
                normals.Add(-Vector3.up);
                normals.Add(-Vector3.up);

                int idx1, idx2, idx3, idx4;
                idx4 = vectors.Count - 1;
                idx3 = vectors.Count - 2;
                idx2 = vectors.Count - 3;
                idx1 = vectors.Count - 4;

                //First Triangle V1, V3, V2
                indicies.Add(idx1);
                indicies.Add(idx3);
                indicies.Add(idx2);

                //Second Triangle V3, V4, V2
                indicies.Add(idx3);
                indicies.Add(idx4);
                indicies.Add(idx2);

            }

            mf.mesh.vertices = vectors.ToArray();
            mf.mesh.normals = normals.ToArray();
            mf.mesh.triangles = indicies.ToArray();

            yield return null;
        }


        
        //Finished Buildings
        buildings_built = true;
        Debug.Log("Built Buildings.");
    }
}    

