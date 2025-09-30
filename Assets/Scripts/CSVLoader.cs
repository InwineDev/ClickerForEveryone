using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;

public class CSVLoader : MonoBehaviour
{
    private TextAsset csvFile;
    private char lineSeperator = '\n';
    private char surround = '"';
    private string[] fieldSeperator = { "\",\"" };

    public void LoadCSV()
    {
        csvFile = Resources.Load<TextAsset>("Localization");
    }

    public Dictionary<string, string> GetDictionaryValues(string attributeId)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        string[] lines = csvFile.text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None); 
        if (lines.Length < 1) return dictionary;
        string[] headers = lines[0].Split(fieldSeperator, StringSplitOptions.None);
        for (int i = 0; i < headers.Length; i++)
        {
            headers[i] = headers[i].Trim(surround);
        }

        int attributeIndex = -1;
        for (int i = 0; i < headers.Length; i++)
        {
            if (headers[i].Contains(attributeId))
            {
                attributeIndex = i;
                break;
            }
        }

        if (attributeIndex == -1) return dictionary;
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrEmpty(lines[i])) continue;
            string[] fields = SplitCSVLine(lines[i]);
            
            if (fields.Length <= attributeIndex) continue;

            var key = fields[0].Trim(surround).Trim();
            if (string.IsNullOrEmpty(key) || dictionary.ContainsKey(key)) continue;
            
            var value = fields[attributeIndex].Trim(surround).Trim();
            dictionary.Add(key, value);
        }

        return dictionary;
    }

    private string[] SplitCSVLine(string line)
    {
        List<string> fields = new List<string>();
        bool inQuotes = false;
        int startIndex = 0;

        for (int i = 0; i < line.Length; i++)
        {
            if (line[i] == surround)
            {
                inQuotes = !inQuotes;
            }
            else if (line[i] == ',' && !inQuotes)
            {
                string field = line.Substring(startIndex, i - startIndex).Trim(surround).Trim();
                fields.Add(field);
                startIndex = i + 1;
            }
        }

        string lastField = line.Substring(startIndex).Trim(surround).Trim();
        fields.Add(lastField);

        return fields.ToArray();
    }
}