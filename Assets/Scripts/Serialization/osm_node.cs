using UnityEngine;

using System.Xml;


class osm_node : base_osm
{
    public ulong id { get; private set; }
    public float latitude { get; private set; }
    public float longitude { get; private set; }

    public float x { get; private set; }
    public float y { get; private set; }

    public static implicit operator Vector3 (osm_node node) 
    {
        return new Vector3(node.x, 0, node.y);
    }

    public osm_node(XmlNode node)
    {
        id = GetAttribute<ulong>("id", node.Attributes);
        latitude = GetAttribute<float>("lat", node.Attributes);
        longitude = GetAttribute<float>("lon", node.Attributes);

        //Convert X & Y using MercatorProjection
        y = (float)mercator_projection.latToY(latitude);
        x = (float)mercator_projection.lonToX(longitude);

    }
}