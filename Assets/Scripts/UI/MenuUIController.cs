using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class MenuUIController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void Play()
    {
        SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
    }
    public void PlayLevel(int level)
    {
        Addressables.LoadSceneAsync($"Assets/Scenes/Levels/L{level}.unity", LoadSceneMode.Single);
    }

    public void SetAudioVolume(float value)
    {
        float db = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("Volume", db);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
