using System;
using UnityEngine;

public class Brick : UpdatableComponent
{
    [SerializeField] private Vector2 size = new Vector2(1f, 0.5f);
    [SerializeField] public MeshRenderer meshRenderer;

    private int hitsToDestroy = 1;
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

    public void SetHits(int hits)
    {
        hitsToDestroy = hits;
    }

    public virtual void OnHit()
    {
        hitsToDestroy--;
        if (hitsToDestroy == 0)
        {
            PhysicsManager.Instance.UnregisterBrick(this);

            OnBrickDestroy?.Invoke();

            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(size.x, size.y, 0f));
    }
}
