using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Microlight.MicroBar;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour, IDamageable
    {
        public MicroBar hpBar;
        public LayerMask targetMask;
        public GameObject projectilePrefab;
        public float projectileSpeed;
        public GameObject projectileSpawnLocation;
        public AudioClip eatingSound;
        
        [Header("Stats")]
        public int health;
        public int maxHealth;
        public int lockedEnemyCount;
        public int damageReducePercent;
        public float movementSpeed;
        public float attackRange;
        public float attackCooldown;

        private List<Collider> currentTargets = null;
        private float lastAttackTime = 0;
        private PlayerMovement playerMovement;
        private Animator animator;

        private void Start()
        {
            hpBar.Initialize(maxHealth);
            playerMovement = GetComponent<PlayerMovement>();
            movementSpeed = playerMovement.speed;
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            FindClosestEnemies();
            AttackEnemies();
            hpBar.transform.position = new Vector3(transform.position.x - 1, hpBar.transform.position.y,
                transform.position.z - 1);
        }

        public void UpdateStats()
        {
            hpBar.SetMaxHealth(maxHealth);
            hpBar.UpdateHealthBar(health);
            playerMovement.speed = movementSpeed;
        }

        private void AttackEnemies()
        {
            if (currentTargets == null)
            {
                return;
            }

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                foreach (var target in currentTargets)
                {
                    var direction = (target.transform.position - transform.position).normalized;
                    var force = direction * projectileSpeed;
                    var projectile = Instantiate(projectilePrefab, projectileSpawnLocation.transform.position, target.transform.rotation);
                    projectile.gameObject.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
                }

                lastAttackTime = Time.time;
            }
        }

        private void FindClosestEnemies()
        {
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, attackRange, targetMask);

            if (rangeChecks.Length > 0)
            {
                var targets = rangeChecks.OrderBy(n => (n.transform.position - transform.position).sqrMagnitude)
                    .Take(lockedEnemyCount).ToList();
                currentTargets = targets;
            }
            else
            {
                currentTargets = null;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }

        public void TakeDamage(int damage)
        {
            var reducedDamage = damage - ((damage * damageReducePercent) / 100f); 
            health -= (int)reducedDamage;
            hpBar.UpdateHealthBar(health);
            AudioManager.instance.PlaySound(eatingSound, 1f);
            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            playerMovement.speed = 0;
            animator.SetTrigger("Dying");
            GameManager.instance.OpenGameOverScreen();
        }
    }
}