using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : UpdatableComponent
{
    public static UIManager Instance;

    [SerializeField] private int menuSceneIndex;
    [SerializeField] private GameObject winScreen, loseScreen;

    private bool ended = false;

    public override void OnCustomStart()
    {
        if (Instance != null)
            Destroy(Instance);

        Instance = this;

        winScreen.gameObject.SetActive(false);
        loseScreen.gameObject.SetActive(false);
    }

    public void Lose()
    {
        if (ended) return;

        loseScreen.SetActive(true);
        ended = true;
    }

    public void Win()
    {
        if (ended) return;

        winScreen.SetActive(true);
        ended = true;
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
