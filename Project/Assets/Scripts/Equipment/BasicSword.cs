using UnityEngine;
using System.Collections;

public class BasicSword : Weapon
{
    [SerializeField]
    private float strength = 5.0f;

    public override EquipmentManager.EEquipmentItem _ItemName
    { get { return EquipmentManager.EEquipmentItem.BasicSword; } }

    public override void _StartAttack(Player player)
    {
        player.StartMeleeAttackAnimation(strength);
    }
}
