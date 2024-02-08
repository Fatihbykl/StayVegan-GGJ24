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
            player.AddHealth(increaseHealth);
            player.maxHealth += increaseMaxHealth;
            player.lockedEnemyCount += increaseAttackCountSameTime;
            player.ReduceTakenDamage(increaseArmorByPercent);
            player.movementSpeed += increaseMovementSpeed;
            player.attackDamage += increaseAttackDamage;
            player.attackRange += increaseAttackRange;
            player.DecreaseCooldown(decreaseAttackCooldown);
        }
    }
}