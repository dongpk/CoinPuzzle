using UnityEngine;

public class CoinAnimator : MonoBehaviour
{
    [SerializeField] float argularSpeed = 50f;
    [SerializeField] float coinHeight = 1f;
    [SerializeField] float movementAmplitude = 0.5f;
    [SerializeField] float movementFrequency = 1f;
    [SerializeField] Transform coinMesh;

    // Update is called once per frame
    void Update()
    {
        //xoay dong xu
        coinMesh.Rotate(0f, argularSpeed * Time.deltaTime, 0f);
        //di chuyen dong xu len xuong
        float deltaY = movementAmplitude * Mathf.Sin(movementFrequency * Time.time);

        coinMesh.localPosition = new Vector3(0f, coinHeight + deltaY, 0f);
    }
}
