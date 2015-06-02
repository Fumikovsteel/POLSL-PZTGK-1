using UnityEngine;
using System.Collections;

namespace EquipmentItems
{
    public class AdvancedShield : Shield
    {
        public override EquipmentManager.EEquipmentItem _ItemName
        { get { return EquipmentManager.EEquipmentItem.AdvancedShield; } }
    }
}
