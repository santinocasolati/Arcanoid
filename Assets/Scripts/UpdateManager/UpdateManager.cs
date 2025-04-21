using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    public static UpdateManager Instance;

    private readonly List<IUpdatable> updatableComponents = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddUpdatable(IUpdatable component)
    {
        if (!updatableComponents.Contains(component))
        {
            updatableComponents.Add(component);
        }
    }

    public void RemoveUpdatable(IUpdatable component)
    {
        if (updatableComponents.Contains(component))
        {
            updatableComponents.Remove(component);
        }
    }

    private void Update()
    {
        for (int i = 0; i < updatableComponents.Count; i++)
        {
            updatableComponents[i].CustomUpdate(Time.deltaTime);
        }
    }
}
