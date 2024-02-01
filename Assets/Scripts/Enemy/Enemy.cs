using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        public float speed;
        public float stoppingDistance;
        public int health;
        public int damage;
        public float attackCooldown;
        public float blinkIntensity;
        public float blinkDuration;
        public AudioClip tomatoSound;
        
        private float lastAttackTime;
        private SkinnedMeshRenderer meshRenderer;
        private WaveManager waveManager;
        private GameObject target;
        private Animator animator;
        private NavMeshAgent agent;

        private void Start()
        {
            meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            waveManager = GetComponentInParent<WaveManager>();
            target = GameManager.instance.player;
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            ChaseTarget();
        }

        private void ChaseTarget()
        {
            agent.SetDestination(target.transform.position);
            if(agent.pathPending) { return; }

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                animator.SetBool("Moving", false);

                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    lastAttackTime = Time.time;
                    animator.SetTrigger("Attack");
                }
            }
            else
            {
                animator.SetBool("Moving", true);
            }
        }

        public void Attack()
        {
            target.GetComponent<IDamageable>().TakeDamage(damage);
        }

        public void TakeDamage(int _damage)
        {
            health -= _damage;
            AudioManager.instance.PlaySound(tomatoSound);
            if (health <= 0)
            {
                Die();
            }
            else
            {
                StartTakeDamageAnim();
            }
        }
        
        private async void StartTakeDamageAnim()
        {
            await meshRenderer.material.DOColor(Color.red * blinkIntensity, blinkDuration / 2).ToUniTask();
            await meshRenderer.material.DOColor(Color.white, blinkDuration / 2).ToUniTask();
        }

        private async void Die()
        {
            if (this.gameObject == null) { return; }
            
            agent.isStopped = true;
            animator.SetTrigger("Dying");
            GetComponent<BoxCollider>().enabled = false;
            
            await UniTask.WaitForSeconds(1f);
            
            Destroy(gameObject, 0.5f);
        }

        private void OnDestroy()
        {
            waveManager.waves[waveManager.currentWaveIndex].enemiesLeft--;
        }
    }
}