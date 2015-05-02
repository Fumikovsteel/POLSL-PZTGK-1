using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    private Text healthText;

    private void Awake()
    {
        Zelda._Game._GameManager._Player._OnHealthChanged += OnHealthChanged;
    }

    private void OnDestroy()
    {
        if (Zelda._Game != null)
            Zelda._Game._GameManager._Player._OnHealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int life, int maxLife)
    {
        healthText.text = life.ToString() + '/' + maxLife.ToString();
    }
}
