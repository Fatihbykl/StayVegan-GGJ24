using System;
using Interfaces;
using UnityEngine;

namespace Player
{
    public class Projectile : MonoBehaviour
    {
        public int damage;
        public GameObject damageParticle;
        
        private void OnTriggerEnter(Collider other)
        {
            var obj = other.gameObject.GetComponent<IDamageable>();
            if (obj == null) { return; }
            obj.TakeDamage(damage);
            var particle = Instantiate(damageParticle, transform.position, Quaternion.identity);
            Destroy(particle, 1f);
            Destroy(gameObject);
        }
    }
}