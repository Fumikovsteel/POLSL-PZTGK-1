using UnityEngine;
using System.Collections;

namespace EquipmentItems
{
    public abstract class Shield : EquipmentItem
    {
        [SerializeField]
        private int defence;

        public override EquipmentManager.EEquipmentType _ItemType
        { get { return EquipmentManager.EEquipmentType.shield; } }

        public int _Defence
        { get { return defence; } }
    }
}
