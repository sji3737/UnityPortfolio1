using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum LowDataType
{
    AreaInfo = 0,
    StageInfo,
    ItemInfo,
    UpgradeList,
    UpgradeIncrement,
    UpgradeCredit

}

public class TableDataManager : MonoBehaviour
{
    Dictionary<LowDataType, LowBase> _datas;

    static TableDataManager _uniqueInstance;

    public static TableDataManager _Instance
    {
        get { return _uniqueInstance; }
    }

    private void Awake()
    {
        if(_uniqueInstance == null)
        {
            _uniqueInstance = this;

            _datas = new Dictionary<LowDataType, LowBase>();
            _datas.Add(LowDataType.AreaInfo, new AreaInfoTable());
            _datas.Add(LowDataType.StageInfo, new StageInfoTable());
            _datas.Add(LowDataType.ItemInfo, new ItemInfoTable());
            _datas.Add(LowDataType.UpgradeList, new UpgradeListTable());
            _datas.Add(LowDataType.UpgradeIncrement, new UpgradeIncrementTable());
            _datas.Add(LowDataType.UpgradeCredit, new UpgradeCreditTable());

            Load(LowDataType.AreaInfo);
            Load(LowDataType.StageInfo);
            Load(LowDataType.ItemInfo);
            Load(LowDataType.UpgradeList);
            Load(LowDataType.UpgradeIncrement);
            Load(LowDataType.UpgradeCredit);

            DontDestroyOnLoad(gameObject);
        }
        
    }


    public LowBase Get(LowDataType dataType)
    {
        return _datas[dataType];
    }

    void Load(LowDataType dataType)
    {
        string file = dataType.ToString();
        TextAsset data = Resources.Load("Tables/" + file, typeof(TextAsset)) as TextAsset;
        StringReader sr = new StringReader(data.text);

        string line = sr.ReadLine();
        _datas[dataType].Load(line);
    }


}
