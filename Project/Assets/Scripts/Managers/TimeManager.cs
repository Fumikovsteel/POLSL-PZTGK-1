using UnityEngine;
using System.Collections;
using System;

public class TimeManager : MonoBehaviour
{
    private IEnumerator DelayMethodExecution(float timeToWait, Action methodToRun)
    {
        yield return new WaitForSeconds(timeToWait);

        if (methodToRun != null)
            methodToRun();
    }

    public void _DelayMethodExecution(float timeToWait, Action methodToRun)
    {
        StartCoroutine(DelayMethodExecution(timeToWait, methodToRun));
    }
}
