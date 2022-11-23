using System.Xml;
using System.Collections.Generic;


class osm_way : base_osm
{
    public ulong id { get; private set; }

    public bool is_boundary { get; private set; }
    public bool is_building {get; private set;}
    public bool is_road {get; private set;}
    public float height {get; private set;}
    public bool visible { get; private set; }

    public List<ulong> node_ids { get; private set; }


    public osm_way(XmlNode node)
    {
        //Set Default Height
        height = 3.0f;
        

        node_ids = new List<ulong>();


        id = GetAttribute<ulong>("id", node.Attributes);
        visible = GetAttribute<bool>("visible", node.Attributes);


        //Search through XmlNode
        XmlNodeList nds = node.SelectNodes("nd");
        foreach(XmlNode n in nds) 
        {
            ulong ref_no = GetAttribute<ulong>("ref", n.Attributes);
            node_ids.Add(ref_no);
        }

       
        if (node_ids.Count > 1)
        {
            is_boundary = node_ids[0] == node_ids[node_ids.Count - 1];
        }


        
        XmlNodeList tags = node.SelectNodes("tag");
        foreach(XmlNode t in tags) 
        {
            string key = GetAttribute<string>("k", t.Attributes);
            
            //Switch statement decides on how to print objects in XmlNode file
            switch(key)
            {
                case "building:levels":
                    height = 3.0f * GetAttribute<float>("v", t.Attributes);
                    break;
                case "height":
                    
                    break;
                case "buildings":
                    is_building = GetAttribute<string>("v",t.Attributes) == "yes";
                    break;
                case "highway":
                    is_road = true;
                    break;
                
                
            }
        } 
    }
}