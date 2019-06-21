using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using XInput;

[CreateAssetMenu(fileName = "ControllerSchema", menuName = "Customs/Schema", order = 2)]
public class ControllerSchema : ScriptableObject
{
    public new string name = "Default";
    private int id;

    public Schema schema;
    private Schema backUp;

    public ActionButton[] ActionButtons
    {
        get { return schema.actionButtons; }
    }
    public ActionAxis[] ActionAxis
    {
        get { return schema.actionAxis; }
    }

    public void Load(int id)
    {
        this.id = id;

        backUp = (Schema)GetCopy(schema);

        var data = PlayerPrefs.GetString("schema." + this.id, string.Empty);
        if (!string.IsNullOrEmpty(data))
        {
            Debug.Log(data);
            schema = JsonUtility.FromJson<Schema>(data);
        }
        
    }

    public void Revert()
    {
        schema = (Schema)GetCopy(backUp);
    }

    public void Save()
    {
        var data = JsonUtility.ToJson(schema);
        PlayerPrefs.SetString("schema." + id, data);
    }

    private static object GetCopy(object input)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, input);
            stream.Position = 0;
            return formatter.Deserialize(stream);
        }
    }


    
}
