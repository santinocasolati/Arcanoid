using UnityEngine;

public class PlayerController : ManagedUpdateBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float moveSpeed, minX, maxX;
    public Vector2 size = new Vector2(2f, 0.5f);

    private Ball attachedBall;

    private InputSystem_Actions inputs;

    private void Awake()
    {
        inputs = new InputSystem_Actions();
        inputs.Player.Enable();
        inputs.Player.Start.performed += _ => LaunchAttachedBall();

        UpdateManager.Instance.RegisterComponent(this);

        SpawnInitialBall();
    }

    private void SpawnInitialBall()
    {
        GameObject ballObj = Instantiate(ballPrefab);
        attachedBall = ballObj.GetComponent<Ball>();
        ballObj.transform.position = transform.position + Vector3.up * (size.y * 0.5f + attachedBall.Radius);
    }

    private void LaunchAttachedBall()
    {
        if (attachedBall == null) return;

        Vector2 launchDirection = new Vector2(Random.Range(-0.5f, 0.5f), 1f).normalized;
        attachedBall.Launch(launchDirection);
        attachedBall = null;
    }

    public override void CustomUpdate(float deltaTime)
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
                attachedBall.transform.position = transform.position + Vector3.up * (size.y * 0.5f + attachedBall.Radius);
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
