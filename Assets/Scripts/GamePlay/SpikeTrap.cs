using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] float activeDuration = 2f; 
    [SerializeField] float transitionDuration = 0.1f;

    [SerializeField] Vector3 SpikeActivePosition = Vector3.zero;
    [SerializeField] Vector3 SpikeIdlePosition = new Vector3(0, -.5f, 0);

    [Header("Sounds")]
    [SerializeField] AudioClip spikeActivateSound;
    [SerializeField] AudioSource audioSource;

    [Header("Components")]
    [SerializeField] ParticleSystem ActivationEffect;
    [SerializeField] GameObject spikeMesh;

    private float timer = 0f;
    enum EState
    {
        Idle,
        TransitionToActive,
        Active,
        TransitionToIdle
    }

    EState state = EState.Idle;
    void ChangeState(EState newState)
    {
        state = newState;
        timer = 0f;

        //if (state == EState.Idle)
        //{
        //    spikeMesh.SetActive(true);
        //}
        //else
        //{
        //    spikeMesh.SetActive(true);
        //}

        if(state == EState.TransitionToActive)
        {
            audioSource.PlayOneShot(spikeActivateSound);
            
        }
    }
    private void Start()
    {
        //spikeMesh.SetActive(false);
    }
    private void Update()
    {

        if (state == EState.TransitionToActive)
        {
            Vector3 p = Vector3.Lerp(SpikeIdlePosition, SpikeActivePosition, timer / transitionDuration);
            

            spikeMesh.transform.localPosition = p;
            ActivationEffect.Play();
            if (timer >= transitionDuration)
            {
                ChangeState(EState.Active); 
            }
        }
        else if (state == EState.TransitionToIdle)
        {
            Vector3 p = Vector3.Lerp(SpikeActivePosition, SpikeIdlePosition, timer / transitionDuration);

            spikeMesh.transform.localPosition = p;
            if (timer >= transitionDuration)
            {
                ChangeState(EState.Idle);
            }
        }
        else if(state == EState.Active)
        {
            if (timer >= transitionDuration)
            {
                ChangeState(EState.TransitionToIdle);
            }
        }

        timer += Time.deltaTime;
    }

    [ContextMenu("Activate Spike Trap")]
    void Activate()
    {
        if (state == EState.Idle)
            ChangeState(EState.TransitionToActive );


        
    }
    void HideSpikes()
    {
        spikeMesh.SetActive(false);
    }
}
