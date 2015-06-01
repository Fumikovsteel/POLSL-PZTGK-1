using UnityEngine;
using System.Collections;

namespace EquipmentItems
{
    public class SpeedMixture : Mixture
    {
        [SerializeField]
        private float extraSpeedValue = 0.5f;
        [SerializeField]
        private float boostTime = 3.0f;

        public override EquipmentManager.EEquipmentItem _ItemName
        { get { return EquipmentManager.EEquipmentItem.SpeedMixture; } }

        public override void _Use(Player player)
        {
            player.UseSpeedMixture(extraSpeedValue, boostTime);
        }
    }
}