using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EquipmentItems;
using System;

public class EquipmentManager
{
    public enum EEquipmentItem
    {
        // All weapons and armors in enum should be sorted from the least importent to most importent. AdvancedSword will change BasicSword in our inventory
        // but not inversely
        none, BasicSword, AdvancedSword, HealthMixture, SpeedMixture, BasicShield, AdvancedShield, BasicArmor, AdvancedArmor
    }

    public enum EEquipmentType
    {
        none, weapon, shield, armor, mixture
    }

    [Serializable]
    public class Stock
    {
        public EquipmentItem _EquipmentItem;
        public int _Count;
    }

    private Transform equipmentParent;

    private Weapon weapon = null;
    private Dictionary<EEquipmentItem, Stock> mixtures = new Dictionary<EEquipmentItem, Stock>();
    private Armor armor = null;
    private Shield shield = null;

    private bool AddWeapon(Weapon newWeapon)
    {
        if (weapon == null || newWeapon._ItemName > weapon._ItemName)
        {
            weapon = newWeapon;
            _OnItemGathered(newWeapon);
            return true;
        }
        return false;
    }

    private bool AddMixture(Mixture newMixture, int numberOfMixtures)
    {
        if (mixtures.ContainsKey(newMixture._ItemName))
            mixtures[newMixture._ItemName]._Count += numberOfMixtures;
        else
            mixtures.Add(newMixture._ItemName, new Stock() { _EquipmentItem = newMixture, _Count = numberOfMixtures });
        _OnItemGathered(newMixture);
        return true;
    }

    private bool AddArmor(Armor newArmor)
    {
        if (armor == null || newArmor._ItemName > armor._ItemName)
        {
            armor = newArmor;
            _OnItemGathered(newArmor);
            return true;
        }
        return false;
    }

    private bool AddShield(Shield newShield)
    {
        if (shield == null || newShield._ItemName > shield._ItemName)
        {
            shield = newShield;
            _OnItemGathered(newShield);
            return true;
        }
        return false;
    }

    public EquipmentManager(Transform player)
    {
        equipmentParent = new GameObject("Equipment").transform;
        equipmentParent.SetParentResetLocal(player);
    }

    public event Action<EquipmentItem> _OnItemGathered = (x) => { };
    public event Action<EquipmentItem> _OnWeaponUsed = (x) => { };
    public event Action<EquipmentItem> _OnMixtureUsed = (x) => { };

    public int _ArmorValue
    { get { return armor != null ? armor._Armor : 0; } }

    public int _ShieldValue
    { get { return shield != null ? shield._Defence : 0; } }

    public List<Stock> _GetEquipmentState()
    {
        List<Stock> allItemsInEquipment = new List<Stock>();
        if (shield != null)
            allItemsInEquipment.Add(new Stock() { _EquipmentItem = shield, _Count = 1 });
        if (weapon != null)
            allItemsInEquipment.Add(new Stock() { _EquipmentItem = weapon, _Count = 1 });
        if (armor != null)
            allItemsInEquipment.Add(new Stock() { _EquipmentItem = armor, _Count = 1 });
        foreach (KeyValuePair<EEquipmentItem, Stock> mixture in this.mixtures)
            allItemsInEquipment.Add(new Stock() { _EquipmentItem = mixture.Value._EquipmentItem, _Count = mixture.Value._Count });
        return allItemsInEquipment;
    }

    public void _UseWeapon(Player player)
    {
        if (weapon != null)
        {
            weapon._StartAttack(player);
            _OnWeaponUsed(weapon);
        }
    }

    public void _UseMixture(Player player, EEquipmentItem mixtureName)
    {
        if (mixtures.ContainsKey(mixtureName))
        {
            _OnMixtureUsed(mixtures[mixtureName]._EquipmentItem);

            (mixtures[mixtureName]._EquipmentItem as Mixture)._Use(player);
            mixtures[mixtureName]._Count--;
            if (mixtures[mixtureName]._Count <= 0)
                mixtures.Remove(mixtureName);

        }
    }

    public bool _AddToEquipment(EquipmentItem itemToAdd, int numberOfItems = 1)
    {
        if (numberOfItems <= 0)
            return false;

        switch (itemToAdd._ItemType)
        {
            case EEquipmentType.armor:
                return AddArmor(itemToAdd as Armor);
            case EEquipmentType.mixture:
                return AddMixture(itemToAdd as Mixture, numberOfItems);
            case EEquipmentType.shield:
                return AddShield(itemToAdd as Shield);
            case EEquipmentType.weapon:
                return AddWeapon(itemToAdd as Weapon);
            default:
                Debug.Log("Given item type " + itemToAdd._ItemType + " not supported!"); break;
        }
        return false;
    }
}
