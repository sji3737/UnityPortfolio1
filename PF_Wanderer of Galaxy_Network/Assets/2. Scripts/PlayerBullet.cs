using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if(enemy == null)
            {
                enemy = other.GetComponentInParent<Enemy>();
            }
            Destroy(gameObject);
            enemy.Damaged((int)_damage);
        }
        
    }
}
