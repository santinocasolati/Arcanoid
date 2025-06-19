using System;
using System.Collections.Generic;
using UnityEngine;

public class BricksController : UpdatableComponent
{
    private static readonly int VariantSwitchID = Shader.PropertyToID("_VariantSwitch");

    [SerializeField] private List<Brick> bricks = new List<Brick>();
    [SerializeField] private List<Material> variant;

    private List<Brick> currentBricks = new List<Brick>();

    public Action OnBrickCountModified;

    public int BricksCount {  get { return currentBricks.Count; } }

    public override void OnCustomStart()
    {
        base.OnCustomStart();
        currentBricks = new List<Brick>(bricks);

        foreach (var b in bricks)
        {
            float rnd = UnityEngine.Random.value;

            if (rnd <= 0.5f)
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
                else if (rnd <= 0.5f)
                {
                    switchValue = 1f;
                    b.SetHits(4);
                }

                mpb.SetFloat(VariantSwitchID, switchValue);

                b.meshRenderer.SetPropertyBlock(mpb);
            }

            b.OnBrickDestroy += () =>
            {
                currentBricks.Remove(b);
                OnBrickCountModified?.Invoke();

                if (currentBricks.Count <= 0)
                    UIManager.Instance.Win();
            };
        }
    }
}
