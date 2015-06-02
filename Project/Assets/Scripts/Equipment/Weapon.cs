using UnityEngine;
using System.Collections;

namespace EquipmentItems
{
    public abstract class Weapon : EquipmentItem
    {
        [SerializeField]
        private float strength = 10.0f;

        public override EquipmentManager.EEquipmentType _ItemType
        { get { return EquipmentManager.EEquipmentType.weapon; } }

        public void _StartAttack(Player player)
        {
            player.StartMeleeAttackAnimation(strength);
        }
    }
}
