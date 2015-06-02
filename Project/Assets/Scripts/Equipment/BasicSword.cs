using UnityEngine;
using System.Collections;

namespace EquipmentItems
{
    public class BasicSword : Weapon
    {
        public override EquipmentManager.EEquipmentItem _ItemName
        { get { return EquipmentManager.EEquipmentItem.BasicSword; } }
    }
}
