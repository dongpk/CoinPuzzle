using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject HUD;
    [SerializeField] GameObject GameOverMenu;
    [SerializeField] GameObject PauseMenu;

    public HealthDisplay healthDisplay;
    public ProgressUI progressUI;



    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        PauseMenu.SetActive(false);
        GameOverMenu.SetActive(false);
        MainMenu.SetActive(false);
        HUD.SetActive(false);
    }
    private void Start()
    {
        
    }
    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");

    }
    /// <summary>
    /// Bắt đầu trò chơi từ menu chính
    /// </summary>
    public void StartGame()
    {
        MainMenu.SetActive(false);
        SceneManager.LoadScene("Level1");
    }
    public void ShowMainMenu()
    {
        MainMenu.SetActive(true);
        HUD.SetActive(false);
        PauseMenu.SetActive(false);
        GameOverMenu.SetActive(false);
        Time.timeScale = 1f;

    }
    public void ShowHUD()
    {
        MainMenu.SetActive(false);
        HUD.SetActive(true);
        PauseMenu.SetActive(false);
        GameOverMenu.SetActive(false);
    }

    [ContextMenu("Show Game Over Menu")]
    public void ShowGameOverMenu()
    {
        if(PauseMenu.activeSelf)
        {
            return;
        }
        GameOverMenu.SetActive(true);

        ClearCameraTarget();
    }
     void ShowPauseMenu()
    {
        if(GameOverMenu.activeSelf)
        {
            return;
        }
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;

    }
    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void TogglePause()
    {
        if(PauseMenu.activeSelf)
        {
            ResumeGame();
        }
        else
        {
            ShowPauseMenu();
        }
    }

    [ContextMenu("Restart Level")]
    public void RestartLevel()
    {
        GameOverMenu.SetActive(false);
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ClearCameraTarget()
    {
        var cam = (CinemachineCamera)CinemachineBrain.GetActiveBrain(0).ActiveVirtualCamera;
        cam.Target.TrackingTarget = null;
    }

    
}
