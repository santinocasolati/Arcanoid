using UnityEngine;

public class UpdatableComponent : MonoBehaviour, ICustomUpdatable
{
    private void Awake() => UpdateManager.Register(this);

    public virtual void OnCustomStart() { Debug.Log($"Registering: {this.name}");   }

    public virtual void OnCustomUpdate()
    {
    }
}
