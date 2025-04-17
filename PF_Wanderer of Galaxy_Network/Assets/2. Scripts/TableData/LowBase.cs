using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sheet class에 필요한것들
using System.IO;
using System.Text;
using SimpleJSON;

public class LowBase : MonoBehaviour
{
    protected Dictionary<string, Dictionary<string, string>> _nodeDic = new Dictionary<string, Dictionary<string, string>>();

    public virtual void Load(string dataJson)
    {
        
    }

    public virtual string ToS(string key, string subkey)
    {
        if (!_nodeDic.ContainsKey(key))
        {
            Debug.Log("Key가 존재하지 않음");
            return null;
        }

        if (!_nodeDic[key].ContainsKey(subkey))
        {
            Debug.Log("subkey가 존재하지 않음");
            return null;
        }
        return _nodeDic[key][subkey];
    }

    public string ToS(int key, string subkey)
    {
        return ToS(key.ToString(), subkey);
    }

    public int ToI(int index, string columnName)
    {
        int result = 0;
        string value = "";

        string key = index.ToString();
        if (_nodeDic.ContainsKey(key))
        {
            _nodeDic[key].TryGetValue(columnName, out value);
        }
        int.TryParse(value, out result);
        return result;
    }

    public float ToF(int index, string columnName)
    {
        float result = 0;
        string value = "";

        string key = index.ToString();
        if (_nodeDic.ContainsKey(key))
        {
            _nodeDic[key].TryGetValue(columnName, out value);
        }
        float.TryParse(value, out result);
        return result;
    }

    public void Add(string key, string subkey, string val)
    {
        // _nodeDic에 넣도록 한다.
        if (!_nodeDic.ContainsKey(key))
        {
            _nodeDic.Add(key, new Dictionary<string, string>());
        }
        if (!_nodeDic[key].ContainsKey(subkey))
        {
            _nodeDic[key].Add(subkey, val);
        }
    }

    public int MaxCount()
    {
        return _nodeDic.Count;
    }
}
