using System.Collections.Generic;
using UnityEngine;

public enum ETrapActivation
{
    Pressure,
    Looping
}
[SelectionBase]
public class SpikeTrap : MonoBehaviour
{
    public ETrapActivation activation = ETrapActivation.Pressure;
    [SerializeField] bool startActive = false;

    [Header("Spike Trap Settings")]
    [Tooltip("Thời gian chờ trước khi spike hoạt động")]
    [SerializeField] float activationDelay = 1f;
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

    private float timer;
    [Space]
    private List<IDamageable> damageables = new List<IDamageable>();
    #region FSM
    // Các trạng thái của spike trap
    enum EState
    {
        Idle,         // Đang ẩn
        Wait,
        TransitionToActive, // Đang chuyển từ ẩn sang hiển thị
        Active,            // Đang hiển thị
        TransitionToIdle   // Đang chuyển từ hiển thị sang ẩn
    }

    private EState state = EState.Idle;

    /// <summary>
    /// Thay đổi trạng thái hiện tại của spike trap
    /// </summary>
    void ChangeState(EState newState)
    {
        state = newState;
        timer = 0f;
        //spikeMesh.SetActive(true);
        if (state == EState.Idle)
        {

            spikeMesh.SetActive(false);
        }
        else
        {
            spikeMesh.SetActive(true);
        }

        if (state == EState.Wait)
        {
            if (activation == ETrapActivation.Pressure)
            {
                ActivationEffect.Play();
            }

        }
        else if (state == EState.Active)
        {
            TryDamage();
        }
        else if (state == EState.TransitionToActive)
        {
            ActivationEffect.Play();

            audioSource.PlayOneShot(spikeActivateSound);
        }
    }
    private void UpdateFSM()
    {
        if (state == EState.Wait)
        {
            spikeMesh.transform.localPosition = SpikeIdlePosition;


            if (timer >= activationDelay)
            {
                ChangeState(EState.TransitionToActive);
            }
        }
        // Xử lý chuyển đổi từ ẩn sang hiển thị
        else if (state == EState.TransitionToActive)
        {
            // Nội suy vị trí spike từ vị trí ẩn đến vị trí hoạt động
            Vector3 p = Vector3.Lerp(SpikeIdlePosition, SpikeActivePosition, timer / transitionDuration);
            spikeMesh.transform.localPosition = p;



            // Nếu thời gian chuyển đổi kết thúc, chuyển sang trạng thái Active
            if (timer >= transitionDuration)
            {
                ChangeState(EState.Active);
            }
        }
        // Xử lý chuyển đổi từ hiển thị sang ẩn
        else if (state == EState.TransitionToIdle)
        {
            // Nội suy vị trí spike từ vị trí hoạt động đến vị trí ẩn
            Vector3 p = Vector3.Lerp(SpikeActivePosition, SpikeIdlePosition, timer / transitionDuration);
            spikeMesh.transform.localPosition = p;

            // Nếu thời gian chuyển đổi kết thúc, chuyển sang trạng thái Idle
            if (timer >= transitionDuration)
            {
                var targetState = activation == ETrapActivation.Looping ? EState.Wait : EState.Idle;

                ChangeState(targetState);
            }
        }
        // Xử lý khi spike đang ở trạng thái hoạt động
        else if (state == EState.Active)
        {

            // Nếu thời gian hoạt động kết thúc, bắt đầu chuyển về ẩn
            if (timer >= activeDuration)
            {
                ChangeState(EState.TransitionToIdle);
            }
        }
        // Cập nhật bộ đếm thời gian
        timer += Time.deltaTime;
    }
    #endregion


    #region Unity Methods
    private void Start()
    {
        if (activation == ETrapActivation.Pressure)
        {
            ChangeState(startActive ? EState.Active : EState.Idle);
        }
        else
        {
            ChangeState(startActive ? EState.Active : EState.Wait);
        }
    }

    private void Update()
    {
        UpdateFSM();


    }

    private void OnDrawGizmos()
    {
        if (activation == ETrapActivation.Pressure)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, Vector3.one);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        IDamageable health = other.GetComponentInParent<IDamageable>();
        if (health != null && !damageables.Contains(health))
        {
            damageables.Add(health);

        }
        if (health != null && state == EState.Idle && activation == ETrapActivation.Pressure)
        {
            ChangeState(EState.Wait);
        }
        if (health != null && state == EState.Active)
        {
            health.Damage(1);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        IDamageable health = other.GetComponentInParent<IDamageable>();
        if (health != null)
        {
            damageables.Remove(health);
            Debug.Log("Removing damageable:" + health);

        }
    }
    #endregion
    void TryDamage()
    {
        foreach (IDamageable dame in damageables)
        {
            dame.Damage(1);
        }
    }



    //public void Activate()
    //{
    //    // Chỉ kích hoạt nếu spike đang ở trạng thái Idle
    //    if (state == EState.Idle)
    //        ChangeState(EState.TransitionToActive);
    //}

    /// <summary>
    /// Ẩn model spike (không sử dụng hiện tại)
    /// </summary>
    void HideSpikes()
    {
        spikeMesh.SetActive(false);
    }
}
