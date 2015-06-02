using UnityEngine;
using System.Collections;

namespace EquipmentItems
{
    public abstract class Mixture : EquipmentItem
    {
        public abstract void _Use(Player player);

        public override EquipmentManager.EEquipmentType _ItemType
        { get { return EquipmentManager.EEquipmentType.mixture; } }
    }
}
