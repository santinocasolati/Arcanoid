using System;
using System.Collections.Generic;
using UnityEngine;

public class BricksController : UpdatableComponent
{
    [SerializeField] private List<Brick> bricks = new List<Brick>();

    private List<Brick> currentBricks = new List<Brick>();

    public Action OnBrickCountModified;

    public int BricksCount {  get { return currentBricks.Count; } }

    public override void OnCustomStart()
    {
        base.OnCustomStart();
        currentBricks = new List<Brick>(bricks);

        foreach (Brick brick in bricks)
        {
            brick.OnBrickDestroy += () =>
            {
                currentBricks.Remove(brick);

                OnBrickCountModified?.Invoke();

                CheckWin();
            };
        }
    }

    private void CheckWin()
    {
        if (currentBricks.Count > 0) return;

        UIManager.Instance.Win();
    }
}
