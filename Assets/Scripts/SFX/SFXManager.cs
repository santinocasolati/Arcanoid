using UnityEngine;

public class SFXManager : UpdatableComponent
{
    public static SFXManager Instance;

    [SerializeField] private AudioClip bounceSFX;
    [SerializeField] private AudioSource audioSource;

    public override void OnCustomStart()
    {
        if (Instance != null)
            Destroy(Instance);

        Instance = this;
    }

    public void PlayBouce()
    {
        audioSource.PlayOneShot(bounceSFX);
    }
}
