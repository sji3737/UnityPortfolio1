using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody _rdBody;
    protected float _damage;
    private void Awake()
    {
        _rdBody = GetComponent<Rigidbody>();
    }

    public virtual void BulletInit(float speed, float lifeTime, float damage, Vector3 dir)
    {
        _rdBody.AddForce(dir * speed);
        _damage = damage;
        Destroy(gameObject, lifeTime);
    }
}
