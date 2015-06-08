using UnityEngine;
using System;
using System.Collections;

[IntegrationTest.DynamicTestAttribute("PlayerEnemyTests")]
[IntegrationTest.SucceedWithAssertions]
public class PassWhenHPIs : MonoBehaviour {

	public enum AssertType { Less, Equal, More };

	public AssertType assertType = AssertType.Less;
	public int expectedHP = 0;

	private int actualHP = 0;
	private GameObject playerHealth;
	private UnityEngine.UI.Text playerHealthText;

	void Start()
	{
		playerHealth =  GameObject.Find ("PlayerHealth");
		if (playerHealth == null)
			IntegrationTest.Fail ("Could not find 'PlayerHealth'!");
		else {
			playerHealthText = playerHealth.GetComponent<UnityEngine.UI.Text>();
			if (playerHealthText == null)
				IntegrationTest.Fail ("Could not find 'UnityEngine.UI.Text' component in 'PlayerHealth'!");
		}
	}

	void GetOnlyActualHP()
	{
		string actualHPstr = playerHealthText.text.Substring(0, playerHealthText.text.IndexOf("/"));
		int.TryParse (actualHPstr, out actualHP);
	}

	void Update() 
	{
		GetOnlyActualHP ();
		switch (assertType) {
			case AssertType.Equal:
				if (actualHP == expectedHP)
					IntegrationTest.Pass ();
				break;
			case AssertType.Less:
				if (actualHP < expectedHP)
					IntegrationTest.Pass ();
				break;
			case AssertType.More:
				if (actualHP > expectedHP)
					IntegrationTest.Pass ();
				break;
		}
	}
}
