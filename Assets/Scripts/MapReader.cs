using System.Collections.Generic;
using System.Collections;

using System.Xml;
using UnityEngine;

using System; //Not Needed
using System.IO;


public class MapReader : MonoBehaviour
{

    Dictionary<ulong, OsmNode> nodes;
    List<OsmWay> ways;
    OsmBounds bounds;
    public static Vector3 Centre;
     

    [Tooltip("The resource file that contains the OSM map data.")]
    public string resourceFile;

    

    // Start is called before the first frame update
    void Start()
    {

        

        //Add ResourceFile
        if (Directory.Exists (Application.persistentDataPath))
        {
            string dataFolder = Application.persistentDataPath;

            DirectoryInfo d = new DirectoryInfo(dataFolder);

            foreach (var file in d.GetFiles("*.txt"))
            {
                Debug.Log(file);

                //Save File In Resource Path
                resourceFile = file.ToString();
            }
        }



        //Debug.Log("Running.");
        
        nodes = new Dictionary<ulong, OsmNode>();
        ways = new List<OsmWay>();

        var txtAsset = System.IO.File.ReadAllText(resourceFile);
        Debug.Log(txtAsset);
    

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(txtAsset);

        SetBounds(doc.SelectSingleNode("/osm/bounds"));
        GetNodes(doc.SelectNodes("/osm/node"));
        GetWays(doc.SelectNodes("/osm/way"));

    }

    void Update()
    { 
        foreach (OsmWay w in ways)
        {   
            if (w.Visible)
            {
                //Cyan for Buildings
                Color c = Color.cyan;  

                //Red for Roads           
                if (!w.IsBoundary) c = Color.red; 


                for (int i = 1; i < w.NodeIDs.Count; i++)
                {
                    OsmNode p1 = nodes[w.NodeIDs[i - 1]];
                    OsmNode p2 = nodes[w.NodeIDs[i]];

                    Vector3 v1 = p1 - bounds.Centre;
                    Vector3 v2 = p2 - bounds.Centre;

                    //Vector3 v3 = new Vector3(0.0f,0.0f,0.0f);
                    // Vector3 v4 = new Vector3(10.0f,0.0f,0.0f);

                    //Debug.DrawLine(v3, v4);
                    Debug.DrawLine(v1, v2);
                    
                }
            }
        }
    }

    void GetWays(XmlNodeList xmlNodeList)
    {
        foreach(XmlNode node in xmlNodeList)
        {
            OsmWay way = new OsmWay(node);
            ways.Add(way);
        }
    }

    void GetNodes(XmlNodeList xmlNodeList)
    {
        foreach (XmlNode n in xmlNodeList)
        {
            OsmNode node = new OsmNode(n);
            nodes[node.ID] = node; 
        }
    }

    void SetBounds (XmlNode xmlNode)
    {
        bounds = new OsmBounds(xmlNode);
        Centre = bounds.Centre;
    }
}
