using System.Collections.Generic;
using UnityEngine;

public class BricksController : MonoBehaviour
{
    [SerializeField] private List<Brick> bricks = new List<Brick>();

    private List<Brick> currentBricks = new List<Brick>();

    private void Awake()
    {
        currentBricks = new List<Brick>(bricks);

        foreach (Brick brick in bricks)
        {
            brick.OnDestroy += () =>
            {
                currentBricks.Remove(brick);
                CheckWin();
            };
        }
    }

    private void CheckWin()
    {
        if (currentBricks.Count > 0) return;

        //TODO: Win
    }
}
