using UnityEngine;
using System.Collections;

public class AdvancedSword : Weapon
{
    [SerializeField]
    private float strength = 10.0f;

    public override EquipmentManager.EEquipmentItem _ItemName
    { get { return EquipmentManager.EEquipmentItem.AdvancedSword; } }

    public override void _StartAttack(Player player)
    {
        player.StartMeleeAttackAnimation(strength);
    }
}
