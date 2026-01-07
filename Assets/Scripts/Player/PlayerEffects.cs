using System.Collections;
using UnityEngine;


public class PlayerEffects : MonoBehaviour
{
    [SerializeField] private GameObject landingFX;
    [SerializeField] private ParticleSystem sprintStartFX;
    [SerializeField] private TrailRenderer[] sprintTrail;


    private void Start()
    {
        SetSprintTrailActive(false);
    }
    public void OnLanded()
    {
        Instantiate(landingFX, transform.position, Quaternion.identity);
    }
    public void OnSprintStarted()
    {
        sprintStartFX.Play();
        SetSprintTrailActive(true);
    }
    public void OnSprintFinished()
    {
        SetSprintTrailActive(false);
    }
    void SetSprintTrailActive(bool active)
    {
        foreach (var trail in sprintTrail)
        {
            trail.emitting = active;
        }
    }
}
