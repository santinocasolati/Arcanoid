using System;
using UnityEngine;

public class Brick : UpdatableComponent
{
    [SerializeField] private Vector2 size = new Vector2(1f, 0.5f);

    public Action OnBrickDestroy;

    public override void OnCustomStart()
    {
        base.OnCustomStart();
        PhysicsManager.Instance.RegisterBrick(this);
    }

    public Rect GetBounds()
    {
        Vector2 pos = transform.position;
        return new Rect(pos - size * 0.5f, size);
    }

    public virtual void OnHit()
    {
        PhysicsManager.Instance.UnregisterBrick(this);

        OnBrickDestroy?.Invoke();

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(size.x, size.y, 0f));
    }
}
