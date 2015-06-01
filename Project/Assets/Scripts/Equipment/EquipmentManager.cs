﻿using UnityEngine;
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

    public class MixtureStock
    {
        public Mixture _Mixture;
        public int _Count;
    }

    private Transform equipmentParent;

    private Weapon weapon = null;
    private Dictionary<EEquipmentItem, MixtureStock> mixtures = new Dictionary<EEquipmentItem, MixtureStock>();
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

    private bool AddMixture(Mixture newMixture)
    {
        if (mixtures.ContainsKey(newMixture._ItemName))
            mixtures[newMixture._ItemName]._Count++;
        else
            mixtures.Add(newMixture._ItemName, new MixtureStock() { _Mixture = newMixture, _Count = 1 });
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

    public int _ArmorValue
    { get { return (armor != null ? armor._Armor : 0) + (shield != null ? shield._Defence : 0); } }

    public void _UseWeapon(Player player)
    {
        if (weapon != null)
            weapon._StartAttack(player);
        else
            Debug.Log("You don't have any weapon!");
    }

    public void _UseMixture(Player player, EEquipmentItem mixtureName)
    {
        if (mixtures.ContainsKey(mixtureName))
        {
            mixtures[mixtureName]._Mixture._Use(player);
            mixtures[mixtureName]._Count--;
            if (mixtures[mixtureName]._Count <= 0)
                mixtures.Remove(mixtureName);
        }
        else
            Debug.Log("You don't have mixture " + mixtureName + " in equipment!");
    }

    public Weapon _GetWeapon()
    {
        return weapon;
    }

    public List<MixtureStock> _GetAllAvailableMixtures()
    {
        List<MixtureStock> allMixtures = new List<MixtureStock>();
        foreach (MixtureStock mixture in mixtures.Values)
            allMixtures.Add(mixture);
        return allMixtures;
    }

    public bool _AddToEquipment(EquipmentItem itemToAdd)
    {
        switch (itemToAdd._ItemType)
        {
            case EEquipmentType.armor:
                return AddArmor(itemToAdd as Armor);
            case EEquipmentType.mixture:
                return AddMixture(itemToAdd as Mixture);
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
