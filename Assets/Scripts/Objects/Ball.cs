using UnityEngine;

public class Ball : ManagedUpdateBehaviour, IPhysicsObject
{
    [SerializeField] private float radius = 0.25f;
    [SerializeField] private float speed = 5f;

    public Vector2 Position => position;
    public float Radius => radius;

    private Vector2 velocity;
    private Vector2 position;

    private bool launched = false;

    protected override void Awake()
    {
        position = transform.position;
        velocity = Vector2.zero;

        PhysicsManager.Instance.RegisterBall(this);

        base.Awake();
    }

    public void Launch(Vector2 direction)
    {
        velocity = direction.normalized * speed;
        launched = true;
    }

    public override void CustomUpdate(float deltaTime)
    {
        if (!launched) return;

        position += velocity * deltaTime;
        transform.position = position;
    }

    public void BounceX()
    {
        velocity.x *= -1;
    }

    public void BounceY()
    {
        velocity.y *= -1;
    }

    public void SetDirection(Vector2 newDir)
    {
        velocity = newDir.normalized * speed;
    }

    public Rect GetBounds()
    {
        Vector2 size = Vector2.one * (radius * 2f);
        return new Rect(position - size * 0.5f, size);
    }

    public void OnMissed()
    {
        PhysicsManager.Instance.UnregisterBall(this);
        DestroyObject();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, Vector3.one * (radius * 2f));
    }
}
