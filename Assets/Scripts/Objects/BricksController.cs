using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BricksController : UpdatableComponent
{
    private static readonly int VariantSwitchID = Shader.PropertyToID("_VariantSwitch");

    [SerializeField] private GameObject powerUpPrefab;
    [SerializeField] private List<Brick> bricks = new List<Brick>();
    [SerializeField] private List<Material> variant;
    [SerializeField] private int powerUpBricks;
    private int spawnedPowerUps;
    [SerializeField] private List<BasePowerUPSO> powerUps;

    private List<Brick> currentBricks = new List<Brick>();

    public Action OnBrickCountModified;

    public int BricksCount {  get { return currentBricks.Count; } }

    public override void OnCustomStart()
    {
        if (powerUpPrefab == null)
            Debug.LogError("BricksController.powerUpPrefab is null!");

        if (powerUps == null || powerUps.Count == 0)
            Debug.LogError("BricksController.powerUps is empty—no power‑ups to assign!");

        if (variant == null || variant.Count == 0)
            Debug.LogError("BricksController.variant materials list is empty!");

        base.OnCustomStart();
        var shuffled = bricks.OrderBy(_ => UnityEngine.Random.value).ToList();
        var powerUpSet = new HashSet<Brick>(shuffled.Take(Mathf.Min(powerUpBricks, bricks.Count)));

        currentBricks = bricks.Where(b => !powerUpSet.Contains(b)).ToList();

        for (int i = 0; i < bricks.Count; i++)
        {
            var original = bricks[i];

            Brick b = powerUpSet.Contains(original)
                ? EvaluatePowerUp(original)
                : original;

            if (!powerUpSet.Contains(original))
                currentBricks.Add(b);
            else
                EvaluateChangeOfMaterial(b);

            Brick captured = b;

            if (captured != null)
            {
                captured.OnBrickDestroy += () =>
                {
                    currentBricks.Remove(captured);
                    OnBrickCountModified?.Invoke();
                    if (currentBricks.Count == 0)
                        UIManager.Instance.Win();
                };
            }

            bricks[i] = b;
        }

    }

    private Brick EvaluatePowerUp(Brick b)
    {
        if (powerUpPrefab == null) return b;

        var go = Instantiate(powerUpPrefab);
        go.transform.position = b.transform.position;

        var pb = go.GetComponent<PowerupBrick>();
        if (pb == null)
        {
            Debug.LogError($"powerUpPrefab is missing PowerupBrick on its root!");
            return b;
        }

        if (pb.powerUpBlockPrefab == null)
        {
            Debug.LogError($"PowerupBrick.powerUpBlockPrefab is not assigned in the Inspector!");
            return b;
        }

        var basePU = pb.powerUpBlockPrefab.GetComponent<BasePowerUp>();
        if (basePU == null)
        {
            Debug.LogError($"powerUpBlockPrefab is missing a BasePowerUp component!");
            return b;
        }

        if (powerUps.Count == 0)
        {
            Debug.LogError("No entries in the powerUps list to choose from!");
        }
        else
        {
            basePU.powerUp = powerUps[UnityEngine.Random.Range(0, powerUps.Count)];
        }

        b.DestroyBrick();
        spawnedPowerUps++;

        return go.GetComponent<Brick>();
    }

    private void EvaluateChangeOfMaterial(Brick b)
    {
        float rnd = UnityEngine.Random.value;

        if (rnd < 0.65f)
        {
            b.meshRenderer.SetMaterials(variant);

            var mpb = new MaterialPropertyBlock();
            b.meshRenderer.GetPropertyBlock(mpb);

            float switchValue = 0;
            if (rnd < 0.25f)
            {
                switchValue = 0f;
                b.SetHits(2);
            }
            else if (rnd < 0.5f)
            {
                switchValue = 1f;
                b.SetHits(4);
            }
            else if (rnd < 0.65f)
            {
                switchValue = 2f;
                b.SetHits(20);
            }

            mpb.SetFloat(VariantSwitchID, switchValue);

            b.meshRenderer.SetPropertyBlock(mpb);
        }
    }
}
