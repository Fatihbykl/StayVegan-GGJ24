using UnityEngine;

namespace Card
{
    [CreateAssetMenu(fileName = "New Perk", menuName = "Perk")]
    public class Perks : ScriptableObject
    {
        public int increaseHealth;
        public int increaseMaxHealth;
        public int increaseAttackCountSameTime;
        public int increaseArmorByPercent;
        public int increaseAttackDamage;
        public float increaseMovementSpeed;
        public float increaseAttackRange;
        public float decreaseAttackCooldown;

        public void Upgrade(Player.Player player)
        {
            player.health += increaseHealth;
            player.maxHealth += increaseMaxHealth;
            player.lockedEnemyCount += increaseAttackCountSameTime;
            player.damageReducePercent += increaseArmorByPercent;
            player.movementSpeed += increaseMovementSpeed;
            player.attackRange += increaseAttackRange;
            player.attackCooldown -= decreaseAttackCooldown;
        }
    }
}