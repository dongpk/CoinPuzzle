using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject GameOverMenu;
    public HealthDisplay healthDisplay;
    public ProgressUI progressUI;



    public static UIManager Instance { get; private set;  }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        
        GameOverMenu.SetActive(false);
    }
    [ContextMenu("Show Game Over Menu")]
    public void ShowGameOverMenu()
    {
        GameOverMenu.SetActive(true);

        ClearCameraTarget();
    }

    [ContextMenu("Restart Level")]
    public void RestartLevel()
    {
        GameOverMenu.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ClearCameraTarget()
    {
        var cam = (CinemachineCamera) CinemachineBrain.GetActiveBrain(0).ActiveVirtualCamera;
        cam.Target.TrackingTarget = null;
    }
}
