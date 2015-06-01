using UnityEngine;
using System.Collections;

namespace EquipmentItems
{
    public abstract class Armor : EquipmentItem
    {
        [SerializeField]
        private int armor;

        public int _Armor
        { get { return armor; } }
    }
}
