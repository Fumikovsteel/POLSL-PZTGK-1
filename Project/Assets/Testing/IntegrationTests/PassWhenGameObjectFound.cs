using System;
using System.Collections;
using UnityEngine;

[IntegrationTest.DynamicTestAttribute("PlayerEnemyTests")]
[IntegrationTest.SucceedWithAssertions]
public class PassWhenGameObjectFound : MonoBehaviour {

	public string gameObjectName = "";

	void Update() 
	{
		GameObject gameObject = GameObject.Find (gameObjectName);
		if (gameObject != null)
			IntegrationTest.Pass (gameObject);
	}
}
