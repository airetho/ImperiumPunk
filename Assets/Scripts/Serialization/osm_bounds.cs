using UnityEngine;

using System.Xml;


class osm_bounds : base_osm
{
    public Vector3 centre { get; private set; }
    //public double[] centre { get; private set; }

    public float min_lat { get; private set; }
    public float max_lat { get; private set; }


    public float min_lon { get; private set; }
    public float max_lon { get; private set; }
    
    public osm_bounds(XmlNode node) 
    {
        min_lat = GetAttribute<float>("minlat", node.Attributes);
        max_lat = GetAttribute<float>("maxlat", node.Attributes);

        min_lon = GetAttribute<float>("minlon", node.Attributes);
        max_lon = GetAttribute<float>("maxlon", node.Attributes);
    
        float y = (float)((mercator_projection.latToY(max_lat) + mercator_projection.latToY(min_lat)) / 2);
        float x = (float)((mercator_projection.lonToX(max_lon) + mercator_projection.lonToX(min_lon)) / 2);

        //Find Centre
        centre = new Vector3(x, 0 , y);
        //centre = new { x, 0, y };
    }
    
}