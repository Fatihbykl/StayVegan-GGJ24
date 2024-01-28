using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Interfaces;
using UnityEngine;

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

        private void Start()
        {
            meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            waveManager = GetComponentInParent<WaveManager>();
            target = GameManager.instance.player;
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            ChaseTarget();
        }

        private void ChaseTarget()
        {
            var distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance <= stoppingDistance)
            {
                animator.SetBool("Moving", false);
                Attack();
            }
            else
            {
                animator.SetBool("Moving", true);
                var targetPos = new Vector3(target.transform.position.x, transform.position.y,
                    target.transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                transform.LookAt(target.transform.position, Vector3.up);
            }
        }

        private void Attack()
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                target.GetComponent<IDamageable>().TakeDamage(damage);
                lastAttackTime = Time.time;
            }
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
            speed = 0;
            animator.SetTrigger("Dying");
            await UniTask.WaitForSeconds(1f);
            this.gameObject.SetActive(false);
            Destroy(gameObject.transform.parent.gameObject, 0.5f);
        }

        private void OnDestroy()
        {
            waveManager.waves[waveManager.currentWaveIndex].enemiesLeft--;
        }
    }
}