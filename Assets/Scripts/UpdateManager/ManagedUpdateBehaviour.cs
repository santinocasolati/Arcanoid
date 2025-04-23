using UnityEngine;

public abstract class ManagedUpdateBehaviour : MonoBehaviour
{
    protected virtual void Awake()
    {
        UpdateManager.Instance.RegisterComponent(this);
    }

    protected void DestroyObject()
    {
        UpdateManager.Instance.UnregisterComponent(this);
        Destroy(gameObject);
    }

    public abstract void CustomUpdate(float deltaTime);
}
