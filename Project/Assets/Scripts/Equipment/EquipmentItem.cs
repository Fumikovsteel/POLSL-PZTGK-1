using UnityEngine;
using System.Collections;

namespace EquipmentItems
{
    public abstract class EquipmentItem : MonoBehaviour, ICollectableObject
    {
        [SerializeField]
        private GameObject collectableObject;
        [SerializeField]
        private SpriteRenderer itemSprite;

        public abstract EquipmentManager.EEquipmentItem _ItemName
        { get; }

        public abstract EquipmentManager.EEquipmentType _ItemType
        { get; }

        public Sprite _ItemSprite
        { get { return itemSprite.sprite; } }

        public void _Collect()
        {
            Destroy(collectableObject);
        }
    }
}
