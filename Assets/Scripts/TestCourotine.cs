using System.Collections;
using UnityEngine;

public class TestCourotine : MonoBehaviour
{
    Coroutine myCoroutine;
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject[] shotsCameras;

    private void Start()
    {
        playerCamera.SetActive(false);

        foreach (GameObject cam in shotsCameras)
        {
            cam.SetActive(false);
        }

        StartCoroutine(CameraSequence());
    }
    IEnumerator CameraSequence()
    {
        foreach (GameObject cam in shotsCameras)
        {
            cam.SetActive(true);
            yield return new WaitForSeconds(5f);
            cam.SetActive(false);
        }
        playerCamera.SetActive(true);
    }
}
