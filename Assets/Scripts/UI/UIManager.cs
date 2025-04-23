using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private int menuSceneIndex;
    [SerializeField] private GameObject winScreen, loseScreen;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }

        Instance = this;
    }

    public void Lose()
    {
        loseScreen.SetActive(true);
    }

    public void Win()
    {
        winScreen.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void Menu()
    {
        SceneManager.LoadSceneAsync(menuSceneIndex, LoadSceneMode.Single);
    }
}
