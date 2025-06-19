using UnityEngine;

public class UpdatableComponent : MonoBehaviour, ICustomUpdatable
{
    private void Awake() => UpdateManager.Register(this);

    private void OnApplicationQuit() => UpdateManager.Unregister(this);
    public virtual void OnCustomStart() { Debug.Log($"Registering: {this.name}");   }

    public virtual void OnCustomUpdate(float deltaTime) { }
}
