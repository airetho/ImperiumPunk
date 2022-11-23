using UnityEngine;

using System.IO;
using System.Xml;
using System.Collections.Generic;

class map_reader : MonoBehaviour
{

    [HideInInspector]
    public Dictionary<ulong, osm_node> nodes;

    [HideInInspector]
    public List<osm_way> ways;

    [HideInInspector]
    public osm_bounds bounds; 

    [Tooltip("The resource file that contains the OSM map data.")]
    public string resource_file;

    public static Vector3 centre;

    public bool is_ready {get; private set;}


    //Starting Code for map_reader
    void Start()
    {

        //Add ResourceFile then Convert it to Xml


        //TODO: Save file as Xml in "Loading" as to not have to convert file to txt

        if (Directory.Exists (Application.persistentDataPath))
        {
            string data_folder = Application.persistentDataPath;

            DirectoryInfo d = new DirectoryInfo(data_folder);

            foreach (var file in d.GetFiles("*.txt"))
            {
                Debug.Log(file);

                //Save File In Resource Path
                resource_file = file.ToString();
            }
        }

        nodes = new Dictionary<ulong, osm_node>();
        ways = new List<osm_way>();

        var txt_asset = System.IO.File.ReadAllText(resource_file);
        Debug.Log(txt_asset);
    

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(txt_asset);

        SetBounds(doc.SelectSingleNode("/osm/bounds"));
        get_nodes(doc.SelectNodes("/osm/node"));
        get_ways(doc.SelectNodes("/osm/way"));

        is_ready = true;
    }


    void Update()
    { 
        foreach (osm_way w in ways)
        {   
            if (w.visible)
            {
                //Red for buildings
                Color color = Color.red;  

                //yellow for roads           
                if (!w.is_boundary) 
                {
                    color = Color.yellow;
                }


                for (int i = 1; i < w.node_ids.Count; i++)
                {
                    osm_node p1 = nodes[w.node_ids[i - 1]];
                    osm_node p2 = nodes[w.node_ids[i]];

                    Vector3 v1 = p1 - bounds.centre;
                    Vector3 v2 = p2 - bounds.centre;

                    Debug.DrawLine(v1, v2, color);
                }
            }
        }
    }


    void get_ways(XmlNodeList xml_node_list)
    {
        foreach(XmlNode node in xml_node_list)
        {
            osm_way way = new osm_way(node);
            ways.Add(way);
        }
    }

    void get_nodes(XmlNodeList xml_node_list)
    {
        foreach (XmlNode n in xml_node_list)
        {
            osm_node node = new osm_node(n);
            nodes[node.id] = node; 
        }
    }

    void SetBounds (XmlNode xml_node)
    {
        bounds = new osm_bounds(xml_node);
        centre = bounds.centre;
    }
}