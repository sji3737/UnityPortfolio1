using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : MonoBehaviour
{
    Rigidbody _rdBody;
    GameObject _target;
    float _speed;
    float _rotateSpeed;
    float _lifeTime;
    float _damage;


    private void Awake()
    {
        _rdBody = GetComponent<Rigidbody>();
    }
    public void Init(GameObject target, float speed, float rotateSpeed, float lifeTime, float damage)
    {
        _target = target;
        _speed = speed;
        _rotateSpeed = rotateSpeed;
        _lifeTime = lifeTime;
        _damage = damage;

        Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        _rdBody.velocity = transform.forward * _speed;
        if(_target != null)
        {
            Quaternion goalAngle = Quaternion.LookRotation(_target.transform.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, goalAngle, Time.deltaTime * _rotateSpeed);
        }
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy == null)
            {
                enemy = other.GetComponentInParent<Enemy>();
            }
            enemy.Damaged((int)_damage);
            EffectManager._Instance.EffectMissileExplosion(transform.position + transform.forward * 0.3f);
            Destroy(gameObject);
            
        }
    }
}
