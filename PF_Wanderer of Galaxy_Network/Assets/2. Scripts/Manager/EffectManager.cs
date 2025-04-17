using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] GameObject _prefabMissileExplosion;
    [SerializeField] GameObject _prefabEnemyExplosion;
    static EffectManager _uniqueInstance;

    public static EffectManager _Instance
    {
        get
        {
            return _uniqueInstance;
        }
    }

    private void Awake()
    {
        _uniqueInstance = this;
    }

    public void EffectMissileExplosion(Vector3 pos)
    {
        Instantiate(_prefabMissileExplosion, pos, Quaternion.identity);
    }

    public void EffectEnemyExplosion(Vector3 pos)
    {
        Instantiate(_prefabEnemyExplosion, pos, Quaternion.identity);
    }
}
