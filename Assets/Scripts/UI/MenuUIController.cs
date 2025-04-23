using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIController : MonoBehaviour
{
    [SerializeField] private int gameSceneIndex;

    public void Play()
    {
        SceneManager.LoadSceneAsync(gameSceneIndex, LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
