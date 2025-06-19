using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Power Up", menuName = "PowerUps/BallLauncherPowerUp")]
public class BallLauncherPowerUp : BasePowerUPSO
{
    [SerializeField] private int ballsToSpawn;
    public override void OnGrab(PlayerController player)
    {
        player.StartCoroutine(LaunchBallsInDifferentTime(player));
    }

    private IEnumerator LaunchBallsInDifferentTime(PlayerController player)
    {
        for (int i = 0; i < ballsToSpawn; i++)
        {
            player.SpawnBall();
            player.LaunchAttachedBallStraight();
            yield return new WaitForSeconds(2);
        }
    }
}
