using System;
using System.Collections.Generic;
using System.IO;

public class INIFile
{
    public static INIFile loadFile(string filePath)
    {
        INIFile output = new INIFile();

        string curHeader = "";

        foreach (string line in File.ReadLines(filePath))
        {
            if (line == "" || line.StartsWith("#"))
                continue;
            
            if (line.StartsWith("[") && line.EndsWith("]"))
            {
                curHeader = line.Substring(1, line.Length - 2);
                continue;
            }

            if (curHeader == "")
            {
                throw new Exception("A heder is not provided!");
            }

            string value, key;
            string[] splitRes = line.Split('=');

            key = splitRes[0];
            value = splitRes[1];

            if (!output.m_objData.ContainsKey(curHeader))
                output.m_objData[curHeader] = new Dictionary<string, string>();
            
            output.m_objData[curHeader][key] = value;
        }
        
        return output;
    }
    
    private readonly Dictionary<string, Dictionary<string, string>> m_objData = 
        new Dictionary<string, Dictionary<string, string>>();
    
    public Dictionary<string, string> this[string key]
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
