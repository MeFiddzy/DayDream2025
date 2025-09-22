using System;
using System.Collections.Generic;
using System.IO;

public class INIFile
{
    public static INIFile loadFile(string filePath)
    {
        INIFile output = new INIFile();

        string curHeder = "";

        foreach (string line in File.ReadLines(filePath))
        {
            if (line == "" || line.StartsWith("#"))
                continue;
            
            if (line.StartsWith("[") && line.EndsWith("]"))
            {
                curHeder = line.Substring(1, line.Length - 2);
                continue;
            }

            if (curHeder == "")
            {
                throw new Exception("A heder is not provided!");
            }

            string value, key;
            string[] splitRes = line.Split('=');

            key = splitRes[0];
            value = splitRes[1];

            if (!output.m_objData.ContainsKey(curHeder))
                output.m_objData[curHeder] = new Dictionary<string, object>();
            
            output.m_objData[curHeder][key] = value;
        }
        
        return output;
    }
    
    private readonly Dictionary<string, Dictionary<string, object>> m_objData = 
        new Dictionary<string, Dictionary<string, object>>();
    
    public Dictionary<string, object> this[string key]
    {
        get
        {
            if (m_objData.ContainsKey(key))
            {
                return m_objData[key];
            }
            else
            {
                throw new Exception("Key not found!");
            }
        }
    }
}
