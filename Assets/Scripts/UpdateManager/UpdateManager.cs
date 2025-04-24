using System.Collections.Generic;
using UnityEngine;

public static class UpdateManager
{
    static readonly List<ICustomUpdatable> updatables = new();
    static readonly List<ICustomPhysicsUpdatable> physicsUpdatables = new();

    static readonly HashSet<object> started = new();

    static readonly List<ICustomUpdatable> pendingAdd = new();
    static readonly List<ICustomUpdatable> pendingRemove = new();

    static readonly List<ICustomPhysicsUpdatable> pendingPhysicsAdd = new();
    static readonly List<ICustomPhysicsUpdatable> pendingPhysicsRemove = new();

    static bool isUpdating = false;
    static bool isPhysicsUpdating = false;

    public static void Register(object obj)
    {
        if (obj is ICustomUpdatable updatable)
        {
            if (!started.Contains(obj))
            {
                updatable.OnCustomStart();
                started.Add(obj);
            }

            if (isUpdating)
                pendingAdd.Add(updatable);
            else
                updatables.Add(updatable);
        }

        if (obj is ICustomPhysicsUpdatable physicsUpdatable)
        {
            if (isPhysicsUpdating)
                pendingPhysicsAdd.Add(physicsUpdatable);
            else
                physicsUpdatables.Add(physicsUpdatable);
        }
    }

    public static void Unregister(ICustomUpdatable updatable)
    {
        if (isUpdating)
            pendingRemove.Add(updatable);
        else
            updatables.Remove(updatable);
    }

    public static void Unregister(ICustomPhysicsUpdatable physicsUpdatable)
    {
        if (isPhysicsUpdating)
            pendingPhysicsRemove.Add(physicsUpdatable);
        else
            physicsUpdatables.Remove(physicsUpdatable);
    }

    public static void MyUpdate()
    {
        isUpdating = true;

        for (int i = updatables.Count - 1; i >= 0; i--)
        {
            var sys = updatables[i];

            if (sys == null)
            {
                updatables.RemoveAt(i);
                continue;
            }

            try
            {
                sys.OnCustomUpdate();
            }
            catch (MissingReferenceException)
            {
                updatables.RemoveAt(i);
            }
        }

        isUpdating = false;

        foreach (var add in pendingAdd)
            updatables.Add(add);
        pendingAdd.Clear();

        foreach (var rem in pendingRemove)
            updatables.Remove(rem);
        pendingRemove.Clear();
    }

    public static void MyFixedUpdate()
    {
        isPhysicsUpdating = true;

        for (int i = physicsUpdatables.Count - 1; i >= 0; i--)
        {
            var sys = physicsUpdatables[i];

            if (sys == null)
            {
                physicsUpdatables.RemoveAt(i);
                continue;
            }

            try
            {
                sys.OnFixedUpdate(Time.deltaTime);
            }
            catch (MissingReferenceException)
            {
                physicsUpdatables.RemoveAt(i);
            }
        }

        isPhysicsUpdating = false;

        foreach (var add in pendingPhysicsAdd)
            physicsUpdatables.Add(add);
        pendingPhysicsAdd.Clear();

        foreach (var rem in pendingPhysicsRemove)
            physicsUpdatables.Remove(rem);
        pendingPhysicsRemove.Clear();
    }
}
