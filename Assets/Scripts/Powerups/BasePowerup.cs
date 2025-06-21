using UnityEngine;

public class BasePowerUp : UpdatableComponent, IPowerUp, ICustomPhysicsUpdatable
{
    [SerializeField] private Vector2 size = new Vector2(1f, 1f);
    [SerializeField] private float fallSpeed;
    
    [HideInInspector] public BasePowerUPSO powerUp;
    private Vector2 position;

    public override void OnCustomStart()
    {
        base.OnCustomStart();
        position = transform.position;

        PhysicsManager.Instance.RegisterPowerup(this);
    }

    public void OnFixedUpdate(float deltaTime)
    {
        position.y -= fallSpeed * deltaTime;
        transform.position = position;
    }

    public virtual void OnGrab(PlayerController player)
    {
        powerUp.OnGrab(player);
        PhysicsManager.Instance.UnregisterPowerup(this);
        Destroy(gameObject);
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
