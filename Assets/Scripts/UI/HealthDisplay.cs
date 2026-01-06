using System.Collections.Generic;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] GameObject healthIconPrefab;
    [SerializeField] List<GameObject> icons;

    [SerializeField] private int points = 0;

    public int Points
    {
        get => points;
        set
        {
            int oldValue = points;
            points = Mathf.Max(value,0);
            
            ManageIcon(value-oldValue);
        }
    }
    void ManageIcon(int deltaPoint)
    {
        if (deltaPoint == 0)
        {
            return;
        }

        if (deltaPoint > 0)
        {
            for (int i = 0; i < deltaPoint; i++)
            {
                var newIcon = Instantiate(healthIconPrefab, transform);

                icons.Add(newIcon);
            }
        }
        else
        {
            for (int i = 0; i < -deltaPoint; i++)
            {
                var icon = icons[icons.Count - 1];

                icons.Remove(icon);

                Destroy(icon);
            }
        }




    }
}
