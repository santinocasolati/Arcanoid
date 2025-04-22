using UnityEngine;

public class Ball : ManagedUpdateBehaviour
{
    [SerializeField] private float radius = 0.25f;
    [SerializeField] private float speed = 5f;

    public Vector2 Position => position;
    public float Radius => radius;

    private Vector2 velocity;
    private Vector2 position;

    private void Awake()
    {
        position = transform.position;
        velocity = Vector2.zero;

        UpdateManager.Instance.RegisterComponent(this);
        PhysicsManager.Instance.RegisterBall(this);
    }

    public void Launch(Vector2 direction)
    {
        velocity = direction.normalized * speed;
    }

    public override void CustomUpdate(float deltaTime)
    {
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
        UpdateManager.Instance.UnregisterComponent(this);
        PhysicsManager.Instance.UnregisterBall(this);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, Vector3.one * (radius * 2f));
    }
}
