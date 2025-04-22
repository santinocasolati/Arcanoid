using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private Vector2 size = new Vector2(1f, 0.5f);

    private void Awake()
    {
        PhysicsManager.Instance.RegisterBrick(this);
    }

    public Rect GetBounds()
    {
        Vector2 pos = transform.position;
        return new Rect(pos - size * 0.5f, size);
    }

    public void OnHit()
    {
        PhysicsManager.Instance.UnregisterBrick(this);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(size.x, size.y, 0f));
    }
}
