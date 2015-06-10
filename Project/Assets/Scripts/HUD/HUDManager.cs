using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class HUDManager : MonoBehaviour
{
	[SerializeField]
	private Text
		healthText;
	[SerializeField]
	private RectTransform
		healthTransform;

	[SerializeField]
	private GameObject
		messageBox;
	[SerializeField]
	private Text
		messageText;

	[SerializeField]
	private Image
		weaponImage;
	[SerializeField]
	private Image
		shieldImage;
	[SerializeField]
	private Image
		armorImage;
	[SerializeField]
	private Image
		healthMixtureImage;
	[SerializeField]
	private Image
		speedMixtureImage;

	[SerializeField]
	private Text
		healthMixturesAmountText;
	[SerializeField]
	private Text
		speedMixturesAmountText;

	private int healthMixtures = 0;
	private int speedMixtures = 0;

	private void Awake ()
	{
		Zelda._Game._GameManager._Player._OnHealthChanged += OnHealthChanged;
		Zelda._Game._GameManager._Player._OnItemGathered += OnItemGathered;
		Zelda._Game._GameManager._Player._OnMixtureUsed += OnMixtureUsed;
		showMessagebox (false);

		armorImage.gameObject.SetActive (false);
		shieldImage.gameObject.SetActive (false);
		weaponImage.gameObject.SetActive (false);

		healthMixtureImage.gameObject.SetActive (false);
		speedMixtureImage.gameObject.SetActive (false);
	}

	private void OnDestroy ()
	{
		if (Zelda._Game != null && Zelda._Game._GameManager._Player != null) {
			Zelda._Game._GameManager._Player._OnHealthChanged -= OnHealthChanged;
			Zelda._Game._GameManager._Player._OnItemGathered -= OnItemGathered;
			Zelda._Game._GameManager._Player._OnMixtureUsed -= OnMixtureUsed;
		}
	}

	private void OnHealthChanged (int life, int maxLife)
	{
		healthText.text = "Health: " + life.ToString ();
		float fMaxLife = (float)maxLife;
		float fLife = (float)life;

		if (fLife >= 0) {
			float xPosition = 0f - ((fMaxLife - fLife) / fMaxLife) * 430f;

			Vector2 pos = healthTransform.anchoredPosition;

			pos.x = xPosition;
            
			healthTransform.anchoredPosition = pos;
		}
	}

	private void OnItemGathered (EquipmentItems.EquipmentItem equipmentItem)
	{
		switch (equipmentItem._ItemType) {
		case EquipmentManager.EEquipmentType.armor:
			{
				armorImage.gameObject.SetActive (true);
				armorImage.sprite = equipmentItem._ItemSprite;
				break;
			}
		case EquipmentManager.EEquipmentType.shield:
			{
				shieldImage.gameObject.SetActive (true);
				shieldImage.sprite = equipmentItem._ItemSprite;
				break;
			}
		case EquipmentManager.EEquipmentType.weapon:
			{
				weaponImage.gameObject.SetActive (true);
				weaponImage.sprite = equipmentItem._ItemSprite;
				break;
			}
		case EquipmentManager.EEquipmentType.mixture:
			{
				if (equipmentItem._ItemName == EquipmentManager.EEquipmentItem.HealthMixture) {
					healthMixtures++;
					healthMixtureImage.gameObject.SetActive (true);
					healthMixturesAmountText.text = "x " + healthMixtures + " (press \"1\" to use)";
					healthMixtureImage.sprite = equipmentItem._ItemSprite;
				} else if (equipmentItem._ItemName == EquipmentManager.EEquipmentItem.SpeedMixture) {
					speedMixtures++;
					speedMixtureImage.gameObject.SetActive (true);
					speedMixturesAmountText.text = "x " + speedMixtures + " (press \"2\" to use)";
					speedMixtureImage.sprite = equipmentItem._ItemSprite;
				}
				break;
			}
		}
	}

	private void OnMixtureUsed (EquipmentItems.EquipmentItem equipmentItem)
	{
		if (equipmentItem._ItemName == EquipmentManager.EEquipmentItem.HealthMixture) {
			healthMixtures--;
			if (healthMixtures < 1) {
				healthMixtureImage.gameObject.SetActive (false);
			}
			healthMixturesAmountText.text = "x " + healthMixtures + " (press \"1\" to use)";
		} else if (equipmentItem._ItemName == EquipmentManager.EEquipmentItem.SpeedMixture) {
			speedMixtures--;
			if (speedMixtures < 1) {
				speedMixtureImage.gameObject.SetActive (false);
			}
			speedMixturesAmountText.text = "x " + speedMixtures + " (press \"2\" to use)";
		}
	}

	private void showMessagebox (bool value)
	{
		messageBox.SetActive (value);
	}

	private void setMessage (string text)
	{
		messageText.text = text;
	}

	public GameObject getMessageBox ()
	{
		return messageBox;
	}
	
	public Text getMessageText ()
	{
		return messageText;
	}
}
