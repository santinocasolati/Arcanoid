using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    private ManagedUpdateBehaviour[] updatableComponents;

    private void Awake()
    {
        updatableComponents = FindObjectsByType<ManagedUpdateBehaviour>(FindObjectsSortMode.None);
    }

    private void Update()
    {
        for (int i = 0; i < updatableComponents.Length; i++)
        {
            updatableComponents[i].CustomUpdate(Time.deltaTime);
        }
    }
}
