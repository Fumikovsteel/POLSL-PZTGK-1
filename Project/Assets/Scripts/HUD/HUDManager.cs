using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
    private GameObject armor1Image;
    [SerializeField]
    private GameObject armor2Image;

    private void Awake()
    {
        Zelda._Game._GameManager._Player._OnHealthChanged += OnHealthChanged;
        sword2Image.SetActive(false);
        shield2Image.SetActive(false);
        armor2Image.SetActive(false);
        showMessagebox(false);
    }

    private void OnDestroy()
    {
        if (Zelda._Game != null)
            Zelda._Game._GameManager._Player._OnHealthChanged -= OnHealthChanged;
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
        armor1Image.SetActive(false);
        armor2Image.SetActive(true);
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
