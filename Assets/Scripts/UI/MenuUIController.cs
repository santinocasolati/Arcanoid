using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void Play()
    {
        SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
    }
    public void PlayLevel(int level)
    {
        
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
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
