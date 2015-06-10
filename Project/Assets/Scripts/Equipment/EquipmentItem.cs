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

        private void Awake()
        {
            iTween.ScaleTo(gameObject, iTween.Hash("scale", new Vector3(1.8f, 1.8f, 1.8f), "time", 0.5f, "easetype", iTween.EaseType.linear,
                                                   "looptype", iTween.LoopType.pingPong));
        }

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
