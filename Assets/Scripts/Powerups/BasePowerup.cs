using UnityEngine;

public class BasePowerup : ManagedUpdateBehaviour, IPowerUp
{
    [SerializeField] private Vector2 size = new Vector2(1f, 1f);
    [SerializeField] private float fallSpeed;

    private Vector2 position;

    protected override void Awake()
    {
        position = transform.position;

        PhysicsManager.Instance.RegisterPowerup(this);

        base.Awake();
    }

    public override void CustomUpdate(float deltaTime)
    {
        position.y -= fallSpeed * deltaTime;
        transform.position = position;
    }

    public virtual void OnGrab(PlayerController player)
    {
        PhysicsManager.Instance.UnregisterPowerup(this);
        DestroyObject();
    }

    public Rect GetBounds()
    {
        Vector2 pos = transform.position;
        return new Rect(pos - size * 0.5f, size);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(size.x, size.y, 0f));
    }
}
