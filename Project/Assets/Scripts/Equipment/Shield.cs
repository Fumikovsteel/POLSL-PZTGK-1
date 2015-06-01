using UnityEngine;
using System.Collections;

namespace EquipmentItems
{
    public abstract class Shield : EquipmentItem
    {
        [SerializeField]
        private int defence;

        public int _Defence
        { get { return defence; } }
    }
}
