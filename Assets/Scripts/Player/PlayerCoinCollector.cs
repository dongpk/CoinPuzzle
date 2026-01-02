using UnityEngine;

public class PlayerCoinCollector : MonoBehaviour
{
    Player player;
    [SerializeField] GameObject coinCollectFX;
    private void Start()
    {
        player = GetComponent<Player>();
    }
    void CreateCoinCollectFX(Vector3 position)
    {
        Instantiate(coinCollectFX, position, Quaternion.identity);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            CreateCoinCollectFX(other.transform.position + Vector3.up * 0.7f);
            //nhat dong xu
            Destroy(other.gameObject);

            player.coinCollected.Invoke();
        }
    }
}
