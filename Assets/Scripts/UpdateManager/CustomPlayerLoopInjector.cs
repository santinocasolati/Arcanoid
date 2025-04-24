using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

public class CustomPlayerLoopInjector
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InjectCustomUpdate()
    {
        Debug.Log("Injecting custom PlayerLoop");
        PlayerLoopSystem playerLoop = PlayerLoop.GetCurrentPlayerLoop();

        // Inject into Update
        int updateIndex = FindSubSystemIndex(playerLoop.subSystemList, typeof(Update));
        if (updateIndex != -1)
        {
            PlayerLoopSystem updateSystem = playerLoop.subSystemList[updateIndex];
            AppendToSubSystem(ref updateSystem, typeof(UpdateManager), UpdateManager.MyUpdate);
            playerLoop.subSystemList[updateIndex] = updateSystem;
        }

        // Inject into FixedUpdate
        int fixedUpdateIndex = FindSubSystemIndex(playerLoop.subSystemList, typeof(FixedUpdate));
        if (fixedUpdateIndex != -1)
        {
            PlayerLoopSystem fixedUpdateSystem = playerLoop.subSystemList[fixedUpdateIndex];
            AppendToSubSystem(ref fixedUpdateSystem, typeof(UpdateManager), UpdateManager.MyFixedUpdate);
            playerLoop.subSystemList[fixedUpdateIndex] = fixedUpdateSystem;
        }

        PlayerLoop.SetPlayerLoop(playerLoop);
        Debug.Log("Custom PlayerLoop injected.");
    }

    static void AppendToSubSystem(ref PlayerLoopSystem system, System.Type type, PlayerLoopSystem.UpdateFunction func)
    {
        var oldList = system.subSystemList ?? new PlayerLoopSystem[0];
        var newList = new PlayerLoopSystem[oldList.Length + 1];

        for (int i = 0; i < oldList.Length; i++)
            newList[i] = oldList[i];

        newList[^1] = new PlayerLoopSystem
        {
            type = type,
            updateDelegate = func
        };

        system.subSystemList = newList;
    }


    static int FindSubSystemIndex(PlayerLoopSystem[] list, System.Type type)
    {
        for (int i = 0; i < list.Length; i++)
            if (list[i].type == type)
                return i;

        return -1;
    }
}
