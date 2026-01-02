using System.Collections;
using UnityEngine;


public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip coinCollectSound;
    [SerializeField] private AudioClip landSound;


    [Space]
    [SerializeField] private AudioSource audioSource;
    public void OnJumped()
    {
        audioSource.PlayOneShot(jumpSound);
    }
    public void OnCoinCollected()
    {
        audioSource.PlayOneShot(coinCollectSound);
    }
    public void OnLanded()
    {
        audioSource.PlayOneShot(landSound);
    }

}