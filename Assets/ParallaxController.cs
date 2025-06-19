using System;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : UpdatableComponent
{
    [SerializeField] private List<SpriteRenderer> layers = new List<SpriteRenderer>();
    [SerializeField] private List<float> layerSpeeds = new List<float>();
    [SerializeField] private Transform player;

    private Vector3 previousPlayerPosition;

    public override void OnCustomStart()
    {
        previousPlayerPosition = player.position;
    }

    public override void OnCustomUpdate(float deltaTime)
    {
        Vector3 deltaMovement = player.position - previousPlayerPosition;

        for (int i = 0; i < layers.Count; i++)
        {
            if (layers[i] != null)
            {
                Transform layerTransform = layers[i].transform;
                Vector3 newPosition = layerTransform.position;
                newPosition.x += deltaMovement.x * layerSpeeds[i];
                newPosition.y += deltaMovement.y * layerSpeeds[i];
                layerTransform.position = newPosition;
            }
        }

        previousPlayerPosition = player.position;
    }
}
