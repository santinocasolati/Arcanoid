using UnityEngine;

public class PlayerController : UpdatableComponent, IPhysicsObject
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float moveSpeed, minX, maxX;
    public Vector2 size = new Vector2(2f, 0.5f);

    private Ball attachedBall;
    private GenericPool<GameObject> ballPool;

    private InputSystem_Actions inputs;

    public override void OnCustomStart()
    {
        base.OnCustomStart();

        inputs = new InputSystem_Actions();
        inputs.Player.Enable();
        inputs.Player.Start.performed += _ => LaunchAttachedBall();

        ballPool = new GenericPool<GameObject>(parameters => Instantiate(ballPrefab));

        SpawnBall();
    }

    public void SpawnBall()
    {
        GameObject ball = ballPool.GetObject();
        ball.SetActive(true);
        ball.transform.position = transform.position + new Vector3(0, size.y * 1.5f, 0);

        attachedBall = ball.GetComponent<Ball>();

        attachedBall.OnBallMissed += () =>
        {
            ballPool.ReleaseObject(ball);
            ball.SetActive(false);
        };
    }

    public void LaunchAttachedBall()
    {
        if (attachedBall == null) return;

        Vector2 launchDirection = new Vector2(Random.Range(-0.5f, 0.5f), 1f).normalized;
        attachedBall.Launch(launchDirection);
        attachedBall = null;
    }

    public override void OnCustomUpdate(float deltaTime)
    {
        float moveDirection = inputs.Player.Move.ReadValue<float>();

        if (moveDirection != 0)
        {
            Vector3 pos = transform.position;

            pos.x += moveSpeed * moveDirection * deltaTime;
            pos.x = Mathf.Clamp(pos.x, minX, maxX);

            transform.position = pos;

            if (attachedBall != null)
            {
                attachedBall.transform.position = transform.position + new Vector3(0, size.y * 1.5f, 0);
            }
        }
    }

    public Rect GetBounds()
    {
        Vector2 pos = transform.position;
        return new Rect(pos - size * 0.5f, size);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(size.x, size.y, 0f));
    }
}
