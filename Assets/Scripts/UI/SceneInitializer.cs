using System.Collections;
using UnityEngine;


public class SceneInitializer : MonoBehaviour
{
    public enum SceneType
    {
        Gameplay,
        MainMenu
    }
    [SerializeField] SceneType sceneType=SceneType.Gameplay;
    [SerializeField] UIManager uiManagerPrefab;

    private void Awake()
    {
        if (UIManager.Instance == null)
        {
            Instantiate(uiManagerPrefab);   
            DontDestroyOnLoad(UIManager.Instance);
        }
    }

    private void Start()
    {
        if(sceneType==SceneType.Gameplay)
        {
            UIManager.Instance.ShowHUD();
        }
        else if(sceneType==SceneType.MainMenu)
        {
            UIManager.Instance.ShowMainMenu();
        }
    }

}