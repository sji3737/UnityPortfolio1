using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected float _hp;
    protected float _damage;
    protected float _fireRateTime;
    protected float _bulletSpeed;
    protected GameObject _player;
    protected GameObject _bullet;
    protected bool _isFire;
    protected int _score;
    protected PublicDefine.eEnemyState _state;
    protected PublicDefine.eEnemyType _type;

    protected virtual void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("PlayerUnit");
        _isFire = false;
    }


    protected virtual void Attack()
    {

    }

    protected virtual void Rotate()
    {

    }

    public virtual void Damaged(int damage)
    {

    }

    public void Dead()
    {
        EffectManager._Instance.EffectEnemyExplosion(transform.position);
        _state = PublicDefine.eEnemyState.DEAD;
        InGameManager._Instance.AddKillScore(_score);
        SpawnManager._Instance.RemoveEnemy(this);
        Destroy(gameObject);
    }

    public void SetStateEnd()
    {
        _state = PublicDefine.eEnemyState.DEAD;
    }

    public void SetStateEscape()
    {
        _state = PublicDefine.eEnemyState.ESCAPE;
    }

    protected virtual void RemoveInScene()
    {
        _state = PublicDefine.eEnemyState.DEAD;
        Destroy(gameObject);
    }

    public virtual void SetLockOnColor(Color c)
    {

    }
}
