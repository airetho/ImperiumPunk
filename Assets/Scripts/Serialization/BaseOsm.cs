using System;
using System.Xml;

class BaseOsm
{
    
    protected T GetAttribute<T>(string attrName, XmlAttributeCollection attributes)
    {
        // Assume 'attrName' exists in collection.
        string strValue = attributes[attrName].Value;
        return (T)Convert.ChangeType(strValue, typeof(T));
    } 
}