using System.Collections.Generic;
using UnityEngine;

public class LevelProgressTracker : MonoBehaviour
{
    [SerializeField] List<GameObject> coins;
    [SerializeField] LevelExit levelExit;
    public int remainingCoins => coins.Count;

    private void Start()
    {
        coins = new List<GameObject>(GameObject.FindGameObjectsWithTag("Coin"));

    }

    private void Update()
    {
        int previousRemainingCoins = remainingCoins;

        //xóa null coin khỏi danh sách
        for (int i = coins.Count - 1; i >= 0; i--)
        {
            var coin = coins[i];
            if (coin == null)
            {
                coins.RemoveAt(i);
            }
        }

        //cap nhật UI
        UIManager.Instance.progressUI.SetRemaingCoinLabel(remainingCoins);


        if (previousRemainingCoins != remainingCoins && remainingCoins == 0)
        {
            levelExit.OpenGate();
        }
    }
}
