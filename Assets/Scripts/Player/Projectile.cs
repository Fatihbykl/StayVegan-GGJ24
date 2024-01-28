using System;
using Interfaces;
using UnityEngine;

namespace Player
{
    public class Projectile : MonoBehaviour
    {
        public int damage;
        
        private void OnTriggerEnter(Collider other)
        {
            var obj = other.gameObject.GetComponent<IDamageable>();
            if (obj == null) { return; }
            obj.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}