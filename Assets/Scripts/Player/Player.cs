﻿using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Microlight.MicroBar;
using TMPro;
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
        [Space(20)]
        [SerializeField] private int health;
        [SerializeField] private float attackCooldown;
        [SerializeField] private int damageReducePercent;
        public int maxHealth;
        public int lockedEnemyCount;
        public float movementSpeed;
        public int attackDamage;
        public float attackRange;

        private List<Collider> currentTargets = null;
        private PlayerMovement playerMovement;
        private Animator animator;
        private float lastAttackTime = 0;
        private bool canAttack = true;

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

        public void ReduceTakenDamage(int value)
        {
            if (damageReducePercent <= 50)
            {
                damageReducePercent += value;
            }
        }

        public void DecreaseCooldown(float value)
        {
            if (attackCooldown >= 0.4f)
            {
                attackCooldown -= value;
            }
        }

        public void AddHealth(int healthValue)
        {
            var value = this.health + healthValue;
            if (value <= this.maxHealth)
            {
                this.health = value;
            }
            else
            {
                this.health = this.maxHealth;
            }
        }

        private void AttackEnemies()
        {
            if (currentTargets == null || !canAttack)
            {
                return;
            }
            
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                canAttack = false;
                foreach (var target in currentTargets)
                {
                    var direction = (target.transform.position - transform.position).normalized;
                    var force = direction * projectileSpeed;
                    var projectile = Instantiate(projectilePrefab, projectileSpawnLocation.transform.position,
                        target.transform.rotation);
                    projectile.gameObject.GetComponent<Projectile>().damage = attackDamage;
                    projectile.gameObject.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
                }

                lastAttackTime = Time.time;
                canAttack = true;
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