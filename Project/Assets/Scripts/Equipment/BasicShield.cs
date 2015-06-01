using UnityEngine;
using System.Collections;

namespace EquipmentItems
{
    public class BasicShield : Shield
    {
        public override EquipmentManager.EEquipmentItem _ItemName
        { get { return EquipmentManager.EEquipmentItem.BasicShield; } }
    }
}