using System;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : UpdatableComponent
{
    [SerializeField] private List<SpriteRenderer> layers = new List<SpriteRenderer>();
    [SerializeField] private List<float> layerSpeeds = new List<float>();
    [SerializeField] private GameObject border;
    private List<Vector3> startingPos = new List<Vector3>();

    public override void OnCustomStart()
    {
        int count = Mathf.Min(layers.Count, layerSpeeds.Count);

        for (int i = 0; i < count; i++)
            startingPos.Add(layers[i].transform.position);
    }

    public override void OnCustomUpdate(float deltaTime)
    {
        for (int i = 0; i < startingPos.Count; i++)
        {
            var layer = layers[i];

            if (IsCloseToBorder(layer, i))
                layer.transform.position = startingPos[i];
            else
                SlowlyMoveToBorder(layer, i, deltaTime);
        }
    }

    private bool IsCloseToBorder(SpriteRenderer spriteRenderer, int index)
    {
        float rightEdgeX = spriteRenderer.transform.position.x
                           + spriteRenderer.bounds.extents.x;

        float borderX = border.transform.position.x;

        return rightEdgeX <= borderX;
    }

    private void SlowlyMoveToBorder(SpriteRenderer layer, int layerNumber, float deltaTime)
    {
        Vector3 newPos = layer.transform.position;
        newPos.x -= layerSpeeds[layerNumber] * deltaTime;
        layer.transform.position = newPos;
    }
}
