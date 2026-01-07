using UnityEngine;

public class LevelExit : MonoBehaviour
{
    [SerializeField] Collider gateCollider;
    [SerializeField] Transform gateMesh;
    [SerializeField] float speedOpenGate = 2f;
    [SerializeField] string targetScene;
    bool openGate = false;

    public void OpenGate()
    {
        openGate = true;

        gateCollider.enabled = false;

    }
    public void ExitLevel()
    {
        UIManager.Instance.TryLoadScene(targetScene);

    }

    private void Update()
    {
        if (openGate)
        {
            Vector3 targetPosition = new Vector3(0, 0.75f, 0);
            gateMesh.localPosition = Vector3.Lerp(gateMesh.localPosition, targetPosition, speedOpenGate * Time.deltaTime);
        }
    }
}
