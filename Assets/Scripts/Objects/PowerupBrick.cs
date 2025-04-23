using UnityEngine;

public class PowerupBrick : Brick
{
    [SerializeField] private GameObject powerupPrefab;

    public override void OnHit()
    {
        Instantiate(powerupPrefab, transform.position, Quaternion.identity);

        base.OnHit();
    }
}
