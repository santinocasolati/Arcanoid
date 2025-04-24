using System.Collections.Generic;
using UnityEngine;

public class PhysicsManager : UpdatableComponent,ICustomUpdatable
{
    public static PhysicsManager Instance;

    [SerializeField] private PlayerController player;
    [SerializeField] private Transform leftWall, rightWall, topWall;
    [SerializeField] private float wallThickness, minY;

    private List<Ball> balls = new();
    private List<Brick> bricks = new();
    private List<IPowerUp> powerups = new();

    public override void OnCustomStart()
    {
        if (Instance != null)
            UpdateManager.Unregister(Instance);

        Instance = this;

        //base.OnCustomStart();
    }

    public void RegisterBall(Ball ball) => balls.Add(ball);
    public void UnregisterBall(Ball ball)
    {
        balls.Remove(ball);
        CheckLose();
    }

    public void RegisterBrick(Brick brick) => bricks.Add(brick);
    public void UnregisterBrick(Brick brick) => bricks.Remove(brick);

    public void RegisterPowerup(IPowerUp powerup) => powerups.Add(powerup);
    public void UnregisterPowerup(IPowerUp powerup) => powerups.Remove(powerup);

    public override void OnCustomUpdate()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            Ball ball = balls[i];
            CheckWallCollision(ball);
            CheckPlayerCollision(ball);
            CheckBrickCollisions(ball);
        }

        for (int i = 0; i < powerups.Count; i++)
        {
            IPowerUp powerup = powerups[i];
            CheckPowerupCollisions(powerup);
        }
    }

    private void CheckWallCollision(Ball ball)
    {
        Vector2 pos = ball.Position;
        float r = ball.Radius;

        float leftLimit = leftWall.position.x + wallThickness * 0.5f;
        float rightLimit = rightWall.position.x - wallThickness * 0.5f;
        float topLimit = topWall.position.y - wallThickness * 0.5f;

        if (pos.x - r < leftLimit || pos.x + r > rightLimit)
            ball.BounceX();

        if (pos.y + r > topLimit)
            ball.BounceY();

        if (pos.y - r < minY)
        {
            ball.OnMissed();
        }
    }

    private void CheckPlayerCollision(Ball ball)
    {
        if (AABB(ball.GetBounds(), player.GetBounds()))
        {
            float relativeHit = (ball.Position.x - player.transform.position.x) / player.size.x;
            Vector2 newDir = new Vector2(relativeHit, 1).normalized;
            ball.SetDirection(newDir);
        }
    }

    private void CheckBrickCollisions(Ball ball)
    {
        foreach (Brick brick in bricks.ToArray())
        {
            if (AABB(ball.GetBounds(), brick.GetBounds()))
            {
                ball.BounceY();
                brick.OnHit();
                break;
            }
        }
    }

    private void CheckPowerupCollisions(IPowerUp powerup)
    {
        if (AABB(powerup.GetBounds(), player.GetBounds()))
        {
            powerup.OnGrab(player);
        }
    }

    private bool AABB(Rect a, Rect b)
    {
        return a.xMin < b.xMax && a.xMax > b.xMin &&
               a.yMin < b.yMax && a.yMax > b.yMin;
    }

    private void CheckLose()
    {
        if (balls.Count > 0) return;

        UIManager.Instance.Lose();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawCube(new Vector3(leftWall.position.x + wallThickness * 0.5f, leftWall.position.y, leftWall.position.z), new Vector3(wallThickness, 10f, 1f));
        Gizmos.DrawCube(new Vector3(rightWall.position.x - wallThickness * 0.5f, rightWall.position.y, rightWall.position.z), new Vector3(wallThickness, 10f, 1f));
        Gizmos.DrawCube(new Vector3(topWall.position.x, topWall.position.y - wallThickness * 0.5f, topWall.position.z), new Vector3(10f, wallThickness, 1f));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(leftWall.position.x, minY, 0), new Vector3(rightWall.position.x, minY, 0));
    }

}
