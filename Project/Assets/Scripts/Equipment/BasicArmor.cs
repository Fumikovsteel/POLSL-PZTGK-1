using UnityEngine;
using System.Collections;

namespace EquipmentItems
{
    public class BasicArmor : Armor
    {
        public override EquipmentManager.EEquipmentItem _ItemName
        { get { return EquipmentManager.EEquipmentItem.BasicArmor; } }
    }
}

