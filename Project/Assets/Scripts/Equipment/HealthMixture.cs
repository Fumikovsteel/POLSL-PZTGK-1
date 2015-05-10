using UnityEngine;
using System.Collections;

public class HealthMixture : Mixture
{
    [SerializeField]
    private int extraHealth = 10;

    public override EquipmentManager.EEquipmentItem _ItemName
    { get { return EquipmentManager.EEquipmentItem.HealthMixture; } }

    public override void _Use(Player player)
    {
        player.UseHealthMixture(extraHealth);
    }
}
