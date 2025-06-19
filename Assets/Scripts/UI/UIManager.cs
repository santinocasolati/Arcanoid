using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : UpdatableComponent
{
    public static UIManager Instance;

    [SerializeField] private int menuSceneIndex;
    [SerializeField] private GameObject winScreen, loseScreen;
    [SerializeField] private TMP_Text lives;
    [SerializeField] private TMP_Text hits;
    [SerializeField] private TMP_Text blocks;
    [SerializeField] private PhysicsManager physicsManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private BricksController bricksController;

    private bool ended = false;

    private int hitCount = 0;

    public override void OnCustomStart()
    {
        if (Instance != null)
            Destroy(Instance);

        Instance = this;

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
