using UnityEngine;
using System.Collections;

public abstract class Weapon : EquipmentItem
{
    public abstract void _StartAttack(Player player);
}
