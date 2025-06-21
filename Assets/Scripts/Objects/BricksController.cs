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
    [SerializeField] private List<BasePowerUPSO> powerUps;

    private List<Brick> currentBricks = new List<Brick>();

    public Action OnBrickCountModified;

    public int BricksCount => currentBricks.Count;

    public override void OnCustomStart()
    {
        var shuffled = bricks.OrderBy(_ => UnityEngine.Random.value).ToList();
        var powerUpSet = new HashSet<Brick>(shuffled.Take(Mathf.Min(powerUpBricks, bricks.Count)));

        currentBricks = bricks.Where(b => !powerUpSet.Contains(b)).ToList();

        for (int i = 0; i < bricks.Count; i++)
        {
            var original = bricks[i];
            Brick b;

            if (powerUpSet.Contains(original))
                b = EvaluatePowerUp(original);
            else
                b = original;

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

        base.OnCustomStart();
    }

    private Brick EvaluatePowerUp(Brick b)
    {
        if (powerUpPrefab == null)
            return b;

        Destroy(b.gameObject);

        var go = Instantiate(powerUpPrefab, b.transform.position, Quaternion.identity);
        var pb = go.GetComponent<PowerupBrick>();
        pb.powerUp = powerUps[UnityEngine.Random.Range(0, powerUps.Count)];

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

            float switchValue = 0f;
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
            else
            {
                switchValue = 2f;
                b.SetHits(20);
            }

            mpb.SetFloat(VariantSwitchID, switchValue);
            b.meshRenderer.SetPropertyBlock(mpb);
        }
    }
}
