using UnityEngine;
using System.Collections;

public static class TransformExtensions
{
    /// <summary>
    /// Change parent of the transform and set zero vector to it's local position
    /// </summary>
    public static void SetParentResetLocal(this Transform transform, Transform newParent)
    {
        transform.parent = newParent;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;
    }
}
