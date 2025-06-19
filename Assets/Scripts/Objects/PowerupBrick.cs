using UnityEngine;

public class PowerupBrick : Brick
{
    [SerializeField] private GameObject powerupPrefab;

    public override void OnHit()
    {
        hitsToDestroy--;

        if (hitsToDestroy == 0)
        {
            PhysicsManager.Instance.UnregisterBrick(this);

            OnBrickDestroy?.Invoke();
            Instantiate(powerupPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
