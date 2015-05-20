using System;
using System.Collections;
using UnityEngine;

[IntegrationTest.DynamicTestAttribute("ExampleIntegrationTests")]
// [IntegrationTest.Ignore]
[IntegrationTest.ExpectExceptions(false, typeof(ArgumentException))]
[IntegrationTest.SucceedWithAssertions]
[IntegrationTest.TimeoutAttribute(1)]
[IntegrationTest.ExcludePlatformAttribute(RuntimePlatform.Android, RuntimePlatform.LinuxPlayer)]
public class PassWhenGameObjectFound : MonoBehaviour {

	public string gameObjectName = "";

	void Update() 
	{
		GameObject gameObject = GameObject.Find (gameObjectName);
		if (gameObject != null)
			IntegrationTest.Pass (gameObject);
	}
}
