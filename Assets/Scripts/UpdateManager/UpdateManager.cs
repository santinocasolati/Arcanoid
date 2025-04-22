using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    public static UpdateManager Instance;

    private List<ManagedUpdateBehaviour> updatableComponents = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterComponent(ManagedUpdateBehaviour component)
    {
        updatableComponents.Add(component);
    }

    public void UnregisterComponent(ManagedUpdateBehaviour component)
    {
        updatableComponents.Remove(component);
    }

    private void Update()
    {
        for (int i = 0; i < updatableComponents.Count; i++)
        {
            updatableComponents[i].CustomUpdate(Time.deltaTime);
        }
    }
}
