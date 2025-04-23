using UnityEngine;

public interface IPowerUp : IPhysicsObject
{
    void OnGrab(PlayerController player);
}
