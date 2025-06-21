using System;
using System.Collections;
using UnityEngine;

public class Brick : UpdatableComponent
{
    [SerializeField] private Vector2 size = new Vector2(1f, 0.5f);
    [SerializeField] public MeshRenderer meshRenderer;

    protected int hitsToDestroy = 1;
    public Action OnBrickDestroy;

    public override void OnCustomStart()
    {
        base.OnCustomStart();
        StartCoroutine(SusribeToPhysics());
    }

    private IEnumerator SusribeToPhysics()
    {
        yield return null;
        if (PhysicsManager.Instance == null)
        {
            Debug.LogError($"[{name}] PhysicsManager.Instance is NULL! Did you initialize it?");
        }
        else
        {
            PhysicsManager.Instance.RegisterBrick(this);
        }
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
            DestroyBrick();
    }

    public void DestroyBrick()
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
