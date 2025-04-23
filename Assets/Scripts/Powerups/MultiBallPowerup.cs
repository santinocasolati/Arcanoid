using UnityEngine;

public class MultiBallPowerup : BasePowerup
{
    public override void OnGrab(PlayerController player)
    {
        for (int i = 0; i < 2; i++)
        {
            player.SpawnBall();
            player.LaunchAttachedBall();
        }

        base.OnGrab(player);
    }
}
