using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;

public class INIFile
{
    [MustUseReturnValue]
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
    
    private Dictionary<string, Dictionary<string, string>> m_objData = 
        new Dictionary<string, Dictionary<string, string>>();
    
    public void toINI(string filePath)
    {
        using (StreamWriter fout = new StreamWriter(filePath, false))
        {
            foreach (var headerPair in m_objData)
            {
                string header = headerPair.Key;
                var data = headerPair.Value;
                
                fout.WriteLine($"[{header}]");


                foreach (var keyValuePair in data)
                {
                    fout.WriteLine($"{keyValuePair.Key}={keyValuePair.Value}");
                }
                
                fout.WriteLine();
            }
        }
    }
    
    [MustUseReturnValue("This is not the toINI which outputs a file, use a `string filePath` argument for that!")]
    public string toINI()
    {
        string output = "";
        
        foreach (var headerPair in m_objData) 
        {
            string header = headerPair.Key;
            var data = headerPair.Value;

            output += $"[{header}]\n";

            foreach (var keyValuePair in data)
            {
                output += $"{keyValuePair.Key}={keyValuePair.Value}\n";
            }
            
            output += "\n";
        }
        
        return output;
    }

    public string[] loadArray(string header)
    {
        string[] array = new string[m_objData[header].Count + 1];

        foreach (var cur in m_objData[header])
        {
            array[Convert.ToUInt32(cur.Key)] = cur.Value;
        }
        
        return array;
    }
    
    public static INIFile loadByDictionary(Dictionary<string, Dictionary<string, string>> objData)
    {
        INIFile output = new INIFile();
        output.m_objData = objData;
        
        return output;
    }
    
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
