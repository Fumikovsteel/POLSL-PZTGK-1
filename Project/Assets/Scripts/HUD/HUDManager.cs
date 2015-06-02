using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private RectTransform healthTransform;

    [SerializeField]
    private GameObject messageBox;
    [SerializeField]
    private Text messageText;

    [SerializeField]
    private GameObject sword1Image;
    [SerializeField]
    private GameObject sword2Image;
    [SerializeField]
    private GameObject shield1Image;
    [SerializeField]
    private GameObject shield2Image;
    [SerializeField]
    private Image armor1Image;
    [SerializeField]
    private Image armor2Image;

    private void Awake()
    {
        Zelda._Game._GameManager._Player._OnHealthChanged += OnHealthChanged;
        Zelda._Game._GameManager._Player._OnItemGathered += OnItemGathered;
        sword2Image.SetActive(false);
        shield2Image.SetActive(false);
        //armor2Image.SetActive(false);
        showMessagebox(false);

        List<EquipmentManager.Stock> alItems = Zelda._Game._GameManager._Player._AllEquipmentItems;
    }

    private void OnDestroy()
    {
        if (Zelda._Game != null)
        {
            Zelda._Game._GameManager._Player._OnHealthChanged -= OnHealthChanged;
            Zelda._Game._GameManager._Player._OnItemGathered -= OnItemGathered;
        }
    }

    private void OnHealthChanged(int life, int maxLife)
    {
        healthText.text = life.ToString() + '/' + maxLife.ToString();
        float fMaxLife = (float)maxLife;
        float fLife = (float)life;

        if (fLife >= 0)
        {
            float xPosition = 0f - ((fMaxLife - fLife) / fMaxLife) * 430f;

            Vector2 pos = healthTransform.anchoredPosition;

            pos.x = xPosition;
            
            healthTransform.anchoredPosition = pos;
        }

        if (life <= 0)
        {
            setMessage("You are dead!");
            showMessagebox(true);
        }
    }

    private void OnItemGathered(EquipmentItems.EquipmentItem equipmentItem)
    {
        switch (equipmentItem._ItemType)
        {
            case EquipmentManager.EEquipmentType.armor:
                armor1Image.sprite = equipmentItem._ItemSprite; break;
        }
    }

    private void showAdvancedSword()
    {
        sword1Image.SetActive(false);
        sword2Image.SetActive(true);
    }

    private void showAdvancedShield()
    {
        shield1Image.SetActive(false);
        shield2Image.SetActive(true);
    }

    private void showAdvancedArmor()
    {
        //armor1Image.SetActive(false);
        //armor2Image.SetActive(true);
    }

    private void showMessagebox(bool value)
    {
        messageBox.SetActive(value);
    }

    private void setMessage(string text)
    {
        messageText.text = text;
    }
}
