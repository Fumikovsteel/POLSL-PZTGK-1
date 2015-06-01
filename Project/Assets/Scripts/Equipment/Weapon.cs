using UnityEngine;
using System.Collections;

namespace EquipmentItems
{
    public abstract class Weapon : EquipmentItem
    {
        [SerializeField]
        private float strength = 10.0f;

        public void _StartAttack(Player player)
        {
            player.StartMeleeAttackAnimation(strength);
        }
    }
}
