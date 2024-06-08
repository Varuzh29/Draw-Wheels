using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace VarCo
{
    public static class CSVParser
    {
        public static Dictionary<string, Dictionary<string, string>> Parse(char columnSeparator, char rowSeparator, string csvText)
        {
            if (string.IsNullOrEmpty(csvText))
            {
                Debug.LogWarning("CSV text cannot be null or empty.");
                return null;
            }

            Dictionary<string, Dictionary<string, string>> result = new Dictionary<string, Dictionary<string, string>>();
            List<string> languages = new List<string>();
            string[] rows = csvText.Split(rowSeparator);
            for (int i = 0; i < rows.Length; i++)
            {
                List<string> fields = ParseRow(rows[i], columnSeparator);
                if (i == 0)
                {
                    fields.RemoveAt(0);
                    languages.AddRange(fields);
                }
                else
                {
                    string key = fields[0];
                    for (int j = 0; j < fields.Count; j++)
                    {
                        if (j == 0)
                        {
                            if (!result.ContainsKey(key))
                            {
                                result.Add(key, new Dictionary<string, string>());
                            }
                        }
                        else
                        {
                            result[key].Add(languages[j - 1], fields[j]);
                        }
                    }
                }
            }
            return result;
        }

        private static List<string> ParseRow(string row, char columnSeparator)
        {
            List<string> fields = new List<string>();
            StringBuilder stringBuilder = new StringBuilder();
            bool quoted = false;
            foreach (char c in row)
            {
                if (c != '"')
                {
                    if (!quoted)
                    {
                        if (c != columnSeparator)
                        {
                            stringBuilder.Append(c);
                        }
                        else
                        {
                            fields.Add(stringBuilder.ToString().Trim());
                            stringBuilder.Clear();
                        }
                    }
                    else
                    {
                        stringBuilder.Append(c);
                    }
                }
                else
                {
                    if (!quoted)
                    {
                        quoted = true;
                        continue;
                    }
                    else
                    {
                        quoted = false;
                        continue;
                    }
                }
            }
            fields.Add(stringBuilder.ToString().Trim());
            return fields;
        }
    }
}
