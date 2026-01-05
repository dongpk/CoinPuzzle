using UnityEngine;
using UnityEngine.Events;

public class PlayerHeath : MonoBehaviour
{
    [SerializeField] int points = 6;
    public int Points
    {
        get { return points; }
        set
        {
            bool wasAlive = points > 0;
            points = value;
            bool isNowDead = points <= 0;

            if (wasAlive && isNowDead)
            {
                Debug.Log("Player died!");
                Died.Invoke();
            }
        }
    }
    [SerializeField] float BottomLessY = -5f;
    [SerializeField] bool DrawGizmos = true;
    public UnityEvent Died;
    public UnityEvent Damaged;
    public bool Alive => points > 0;

    private void OnDrawGizmosSelected()
    {
        if(!DrawGizmos) return;

        Gizmos.color = Color.red;
        Vector3 CubeSize = new Vector3(10f, 1f, 10f);
        Vector3 CubeCenter = new Vector3(transform.position.x, BottomLessY, transform.position.z);
        CubeCenter.y -= CubeSize.y / 2f;

        Gizmos.DrawCube(CubeCenter, CubeSize);
    }
    private void Update()
    {
        if (Alive)
        {
            if(transform.position.y <= BottomLessY)
            {
                Kill();
            }
        }
        UIManager.Instance.healthDisplay.Points = this.points;
    }
    public void Kill()
    {
        Damage(points);
    }
    public void Damage(int damage)
    {
        points -= damage;
        if (Alive)
        {
            Damaged.Invoke();
        }

        // Kiểm tra nếu chết
        if (points <= 0)
        {

            Died.Invoke();

            UIManager.Instance.ShowGameOverMenu();
        }
    }
}
