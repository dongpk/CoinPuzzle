using UnityEngine;

[SelectionBase]
public class SpinningBladesTrap : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float spinningSpeed = 180f; // degrees per second
    [SerializeField] float waitDuration = 1f;
    [SerializeField] Vector3 destinationOffset = new Vector3(10f, 0, 10f);
    [SerializeField] int damageAmount = 1;

    [Space]
    [SerializeField] Transform meshTransform;

    private float timer;
    enum Estates
    {
        WaitStart,
        WaitEnd,
        MoveToStart,
        MoveToEnd
    }
    Estates state = Estates.MoveToEnd;
    Vector3 startPosition, endPosition;

    #region FSM methods
    void ChangeState(Estates newState)
    {
        state = newState;

        timer = 0f;
    }
    bool MoveTo(Vector3 destination)
    {
        float arriveDistance = 0.4f;
        Vector3 direction = destination - transform.position;
        direction.Normalize();

        transform.position += direction * moveSpeed * Time.deltaTime;

        return Vector3.Distance(transform.position, destination) <= arriveDistance;

    }
    void UpdateFSM()
    {
        if (state == Estates.MoveToEnd)
        {
            if (MoveTo(endPosition))
            {
                ChangeState(Estates.WaitEnd);
            }
        }
        else if (state == Estates.MoveToStart)
        {
            if (MoveTo(startPosition))
            {
                ChangeState(Estates.WaitStart);
            }
        }
        else if (state == Estates.WaitEnd)
        {
            if (timer >= waitDuration)
            {
                ChangeState(Estates.MoveToStart);
            }
        }
        else if (state == Estates.WaitStart)
        {
            if (timer >= waitDuration)
            {
                ChangeState(Estates.MoveToEnd);
            }
        }

        timer += Time.deltaTime;
    }
    #endregion

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            return;
        }
        Gizmos.color = Color.green;
        Vector3 offset = Vector3.up;
        Vector3 destinationPos = transform.position + destinationOffset + offset;
        Gizmos.DrawSphere(destinationPos, .1f);
        Gizmos.DrawLine(transform.position + offset, destinationPos);
        Gizmos.DrawLine(destinationPos, transform.position + destinationOffset);
    }
    private void Start()
    {
        startPosition = transform.position;
        endPosition = transform.position + destinationOffset;
    }
    private void Update()
    {
        UpdateMesh();
        UpdateFSM();
    }

    private void OnTriggerEnter(Collider other)
    {
        var health = other.GetComponentInParent<IDamageable>();
        if (health != null)
        {
            health.Damage(damageAmount);
        }
    }

    void UpdateMesh()
    {
        meshTransform.Rotate(0f, spinningSpeed * Time.deltaTime, 0f);
    }
}
