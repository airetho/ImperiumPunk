using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class road_maker : infrastructure_behaviour
{

    public static bool roads_built = false;

    public Material road_material;
    IEnumerator Start()
    {
        while (!map.is_ready) 
        {
            yield return null;
        }

        //TODO: Processs Map Data to create Roads;

        foreach (var way in map.ways.FindAll((w)=> {return w.is_road;}))
        {
            GameObject go = new GameObject();
            Vector3 localOrigin = GetCentre(way);
            go.transform.position = localOrigin - map.bounds.centre;;

            MeshFilter mf = go.AddComponent<MeshFilter>();
            MeshRenderer mr = go.AddComponent<MeshRenderer>(); 

            mr.material = road_material;

            List<Vector3> vectors = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> indicies = new List<int>();


            //stores the last value of cross to fill out corners properly
            //initial values are garbage to keep unity from getting mad
            Vector3 last_cross = Vector3.up;
            Vector3 last_v1 = Vector3.up;
            Vector3 last_v2 = Vector3.up;

            for (int i = 1; i < way.node_ids.Count; i++)
            {
                //gets the start and end points of a linear section
                osm_node p1 = map.nodes[way.node_ids[i - 1]];
                osm_node p2 = map.nodes[way.node_ids[i]];

                Vector3 s1 = p1 - localOrigin;
                Vector3 s2 = p2 - localOrigin;

                //direction vector for the section
                Vector3 diff = (s2 - s1).normalized;
                //could fix to use Vector2, would require an alternate way of finding perpendicular vector to diff
                var cross = Vector3.Cross(diff, Vector3.up) * 2.0f; //2 Meters = Width of Lane

                //these values, representing the "further" side of the road, should get overwritten, ignored if i=1, or used if i=1 is the only i.
                Vector3 v1 = s2 + cross;
                Vector3 v2 = s2 - cross;
                if (i == 1) //nothing to draw yet, since we don't know the right endpoints
                {
                    last_v1 = s1 + cross;
                    last_v2 = s1 - cross;
                    last_cross = cross;
                    if(way.node_ids.Count != 2) //unless this is the only segment
                    {
                        continue;
                    }
                }
                else
                {
                    v1 = s1 + (cross + last_cross) / 2;
                    v2 = s1 - (cross + last_cross) / 2;
                    last_cross = cross;
                }

                Vector3 v3 = last_v1;
                Vector3 v4 = last_v2;
                last_v1 = v1;
                last_v2 = v2;

                vectors.Add(v1);
                vectors.Add(v2);
                vectors.Add(v3);
                vectors.Add(v4);

                //Allows for Sprites on Roads
                uvs.Add(new Vector2(0,0));
                uvs.Add(new Vector2(1,0));
                uvs.Add(new Vector2(0,1));
                uvs.Add(new Vector2(1,1));



    
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
                indicies.Add(idx2);
                indicies.Add(idx3);

                //Second Triangle V3, V4, V2
                indicies.Add(idx3);
                indicies.Add(idx2);
                indicies.Add(idx4);

                if (i == way.node_ids.Count - 1)
                {
                    Vector3 v5 = s2 + cross;
                    Vector3 v6 = s2 - cross;


                    vectors.Add(v5);
                    vectors.Add(v6);
                    vectors.Add(v1);
                    vectors.Add(v2);

                    //idk what this does
                    uvs.Add(new Vector2(0, 0));
                    uvs.Add(new Vector2(1, 0));
                    uvs.Add(new Vector2(0, 1));
                    uvs.Add(new Vector2(1, 1));

                    normals.Add(-Vector3.up);
                    normals.Add(-Vector3.up);
                    normals.Add(-Vector3.up);
                    normals.Add(-Vector3.up);


                    idx4 = vectors.Count - 1;
                    idx3 = vectors.Count - 2;
                    idx2 = vectors.Count - 3;
                    idx1 = vectors.Count - 4;

                    //First Triangle V1, V3, V2
                    indicies.Add(idx1);
                    indicies.Add(idx2);
                    indicies.Add(idx3);
                    //Second Triangle V3, V4, V2
                    indicies.Add(idx3);
                    indicies.Add(idx2);
                    indicies.Add(idx4);
                }

            }

            mf.mesh.vertices = vectors.ToArray();
            mf.mesh.normals = normals.ToArray();
            mf.mesh.triangles = indicies.ToArray();
            mf.mesh.uv = uvs.ToArray();

            yield return null;

        }

        //Finished Roads
        roads_built = true;
        Debug.Log("Built Roads.");
    }
}    
