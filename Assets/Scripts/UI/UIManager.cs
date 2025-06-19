using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class UIManager : UpdatableComponent
{
    public static UIManager Instance;

    [SerializeField] private int menuSceneIndex;
    [SerializeField] private GameObject winScreen, loseScreen, pauseScreen;
    [SerializeField] private TMP_Text lives;
    [SerializeField] private TMP_Text hits;
    [SerializeField] private TMP_Text blocks;
    [SerializeField] private PhysicsManager physicsManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private BricksController bricksController;
    [SerializeField] private AudioMixer audioMixer;

    private bool ended = false;
    private bool isPaused = false;

    private int hitCount = 0;

    private InputSystem_Actions inputs;

    public override void OnCustomStart()
    {
        if (Instance != null)
            Destroy(Instance);

        Instance = this;
        inputs = new InputSystem_Actions();
        inputs.Player.Enable();
        inputs.Player.Pause.performed += ctx => TogglePause();

        winScreen.gameObject.SetActive(false);
        loseScreen.gameObject.SetActive(false);

        playerController.OnLivesChanged += val => lives.text = val.ToString();
        physicsManager.OnPlayerBounce += () =>
        {
            hitCount++;
            hits.text = hitCount.ToString();
        };
        bricksController.OnBrickCountModified += () => blocks.text = bricksController.BricksCount.ToString();

        blocks.text = bricksController.BricksCount.ToString();
    }

    public void TogglePause()
    {
        if (pauseScreen.gameObject == null) return;

        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0 : 1;
        pauseScreen.gameObject.SetActive(isPaused);
    }

    public void SetAudioVolume(float value)
    {
        float db = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("Volume", db);
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
