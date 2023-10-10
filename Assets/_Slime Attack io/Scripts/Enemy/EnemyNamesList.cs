using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyNamesList 
{
    private string _fileName = "Names";

    private List<string> _names;

    public EnemyNamesList()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(_fileName);

        _names = new List<string>();

        using (var reader = new StringReader(textAsset.text))
        {
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                _names.Add(line);
            }
        }
    }

    public string GetRandomName()
    {
        int value = Random.Range(0, _names.Count);

        return _names[value];
    }
}
