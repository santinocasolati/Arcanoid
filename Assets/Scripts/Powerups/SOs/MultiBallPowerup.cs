using UnityEngine;

[CreateAssetMenu(fileName = "New Power Up", menuName = "PowerUps/MultiBallPowerUp")]
public class MultiBallPowerup : BasePowerUPSO
{
    [SerializeField] private int ballsToSpawn;
    public override void OnGrab(PlayerController player)
    {
        for (int i = 0; i < ballsToSpawn; i++)
        {
            player.SpawnBall();
            player.LaunchAttachedBall();
        }
    }
}
