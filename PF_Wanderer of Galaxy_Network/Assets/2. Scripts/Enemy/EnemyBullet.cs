using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerUnit"))
        {
            PlayerControl player = other.GetComponentInParent<PlayerControl>();
            player.PlayerDamaged((int)_damage);
            Destroy(gameObject);
        }
    }
}
