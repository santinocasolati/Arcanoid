using UnityEngine;

public class PowerupBrick : Brick
{
    [SerializeField] public GameObject powerUpBlockPrefab;

    public override void OnHit()
    {
        hitsToDestroy--;

        if (hitsToDestroy == 0)
        {
            PhysicsManager.Instance.UnregisterBrick(this);

            OnBrickDestroy?.Invoke();
            Instantiate(powerUpBlockPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
