using UnityEngine;

public class PlayerTrapActivator : MonoBehaviour
{
    PlayerHeath playerHealth;
    private void Start()
    {
        playerHealth = GetComponent<PlayerHeath>();
        if(playerHealth == null)
        {
            Debug.LogError("PlayerHeath component not found on the player.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        SpikeTrap trap = other.GetComponent<SpikeTrap>();
        if (trap != null)
        {
            trap.Activate();
            DamagePlayer();
        }
    }
    void DamagePlayer()
    {
        playerHealth.Damage(1);

    }
}
