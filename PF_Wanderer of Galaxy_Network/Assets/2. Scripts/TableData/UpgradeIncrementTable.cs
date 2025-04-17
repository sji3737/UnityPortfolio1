using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class UpgradeIncrementTable : LowBase
{
    public enum eIndex
    {
        Level = 0,
        HP,
        BasicWeaponDamage,
        MissileWeaponDamage,
        MaxSpeed,
        Acceleration,
        Rotate,

        MAX_COUNT
    }

    public override void Load(string dataJson)
    {
        base.Load(dataJson);
        JSONNode node = JSONNode.Parse(dataJson);
        //node[시트넘버][번호순서][컬럼이름]
        //Debug.Log(node[0][0][5]);
        for (int i = 0; i < node[0].Count; i++)
        {
            Dictionary<string, string> temp = new Dictionary<string, string>();
            for (int j = 0; j < node[0][i].Count; j++)
            {
                eIndex idx = (eIndex)j;
                temp.Add(idx.ToString(), node[0][i][j]);
            }
            //int key = i + 2;
            //_nodeDic.Add(key.ToString(), temp);
            _nodeDic.Add(node[0][i][0], temp);
        }
    }
}
