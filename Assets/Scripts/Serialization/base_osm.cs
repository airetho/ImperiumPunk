using System;
using System.Xml;


class base_osm
{
    protected t GetAttribute<t>(string attrName, XmlAttributeCollection attributes)
    {
        // Assume 'attrName' exists in collection.
        string string_value = attributes[attrName].Value;
        return (t)Convert.ChangeType(string_value, typeof(t));
    } 
}