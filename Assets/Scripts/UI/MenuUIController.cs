using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuUIController : MonoBehaviour
{
    [SerializeField] private int gameSceneIndex;
    [SerializeField] private AudioMixer audioMixer;

    public void Play()
    {
        SceneManager.LoadSceneAsync(gameSceneIndex, LoadSceneMode.Single);
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
