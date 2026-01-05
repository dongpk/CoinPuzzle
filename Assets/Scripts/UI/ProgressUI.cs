using TMPro;
using UnityEngine;

public class ProgressUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI remaingCoinsLabel;
    [SerializeField] GameObject MainPanel;
    [SerializeField] GameObject SuccessPanel;

    private void Start()
    {
        MainPanel.SetActive(true);
        SuccessPanel.SetActive(false);
    }

    public void SetRemaingCoinLabel(int amount)
    {
        remaingCoinsLabel.text = amount.ToString();

        if(amount > 0)
        {
            MainPanel.SetActive(true);
            SuccessPanel.SetActive(false);
        }
        else
        {
            MainPanel.SetActive(false);
            SuccessPanel.SetActive(true);
        }
    }
}
