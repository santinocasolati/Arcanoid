using UnityEngine;

public class PowerupBrick : Brick
{
    [SerializeField] private GameObject powerupPrefab;
    [HideInInspector] public BasePowerUPSO powerUp;

    public override void OnHit()
    {
        hitsToDestroy--;

        if (hitsToDestroy == 0)
        {
            PhysicsManager.Instance.UnregisterBrick(this);

            OnBrickDestroy?.Invoke();
            GameObject puBlock = Instantiate(powerupPrefab, transform.position, Quaternion.identity);
            puBlock.GetComponent<BasePowerUp>().powerUp = this.powerUp;
            Destroy(gameObject);
        }
    }
}
