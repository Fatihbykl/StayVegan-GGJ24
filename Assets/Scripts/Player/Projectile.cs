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
            // if projectile hit the game bounds, destroy it
            if (other.CompareTag("Bounds"))
            {
                Destroy(this.gameObject);
                return;
            }
            
            // if projectile hit not damageable object return
            var obj = other.gameObject.GetComponent<IDamageable>();
            if (obj == null) { return; }
            
            // deal damage and instantiate particle
            obj.TakeDamage(damage);
            var particle = Instantiate(damageParticle, transform.position, Quaternion.identity);
            
            // destroy unnecessary objects
            Destroy(particle, 1f);
            Destroy(gameObject);
        }
    }
}