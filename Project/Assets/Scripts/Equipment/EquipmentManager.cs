using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentManager
{
    public enum EEquipmentItem
    {
        // All weapons in enum should be sorted from the least importent to most importent. AdvancedSword will change BasicSword in our inventory
        // but not inversely
        BasicSword, AdvancedSword, HealthMixture, SpeedMixture
    }

    public class MixtureStock
    {
        public Mixture _Mixture;
        public int _Count;
    }

    private Transform equipmentParent;

    private Weapon weapon = null;
    private Dictionary<EEquipmentItem, MixtureStock> mixtures = new Dictionary<EEquipmentItem, MixtureStock>();

    private bool AddWeapon(Weapon newWeapon)
    {
        if (weapon == null || newWeapon._ItemName > weapon._ItemName)
        {
            weapon = newWeapon;
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
        return true;
    }

    public EquipmentManager(Transform player)
    {
        equipmentParent = new GameObject("Equipment").transform;
        equipmentParent.SetParentResetLocal(player);
    }

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
        Weapon weapon = itemToAdd as Weapon;;
        if (weapon != null)
            return AddWeapon(weapon);

        Mixture mixture = itemToAdd as Mixture;
        if (mixture != null)
            return AddMixture(mixture);

        Debug.LogError("Undefined equipment object!");
        return false;
    }
}
