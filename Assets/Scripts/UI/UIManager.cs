using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject HUD;
    [SerializeField] GameObject GameOverMenu;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject LoadingScreen;
    [SerializeField] Slider loadingScreenSlider;

    public HealthDisplay healthDisplay;
    public ProgressUI progressUI;
    Coroutine sceneLoadingCoroutine;



    public static UIManager Instance { get; private set; }


    #region unity methods
    private void Awake()
    {
        Instance = this;

        PauseMenu.SetActive(false);
        GameOverMenu.SetActive(false);
        MainMenu.SetActive(false);
        HUD.SetActive(false);
        LoadingScreen.SetActive(false);
    }
    private void Start()
    {

    }
    private void Update()
    {
        //if (sceneLoadingCoroutine == null && LoadingScreen.activeSelf)
        //{
        //    if (Keyboard.current.anyKey.wasPressedThisFrame)
        //    {
        //        LoadingScreen.SetActive(false);
        //    }
        //}
    }

    #endregion

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        TryLoadScene("MainMenu");
        
    }
    /// <summary>
    /// Bắt đầu trò chơi từ menu chính
    /// </summary>
    public void StartGame()
    {
        MainMenu.SetActive(false);
        //SceneManager.LoadSceneAsync("Level1");
        TryLoadScene("Level1");
    }
    IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        LoadingScreen.SetActive(true);

        while (!loadingOperation.isDone)
        {
            //Debug.Log($"loading: {loadingOperation.progress * 100f}%");
            loadingScreenSlider.value = loadingOperation.progress;

            yield return null;
        }

        loadingScreenSlider.value = 1f;
        LoadingScreen.SetActive(false);
        sceneLoadingCoroutine = null;
    }
    public void TryLoadScene(string targetScene)
    {
        if (sceneLoadingCoroutine == null)
        {
            sceneLoadingCoroutine = StartCoroutine(LoadScene(targetScene));
        }
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
        if (PauseMenu.activeSelf)
        {
            return;
        }
        GameOverMenu.SetActive(true);

        ClearCameraTarget();
    }
    void ShowPauseMenu()
    {
        if (GameOverMenu.activeSelf)
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
        if (PauseMenu.activeSelf)
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
