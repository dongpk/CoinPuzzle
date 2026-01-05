using UnityEditor.UIElements;
using UnityEngine;

public class Blink : MonoBehaviour
{
    [SerializeField] GameObject[] Targets;
    [SerializeField] float OnDuration = .2f;
    [SerializeField] float OffDuration = .4f;

    float timer=0;
    bool state = true;
    float blinkStopTime = 0;

    public void AcivateBlink(float duration)
    {
        blinkStopTime = Time.time + duration;
    }
    private void Update()
    {
        if(Time.time <= blinkStopTime)
        {
            UpdateBlink();
        }
        else
        {
            SetTargetActive(true);
        }


    }

    private void UpdateBlink()
    {
        if (state == true)
        {
            SetTargetActive(true);
            if (timer > OnDuration)
            {
                timer = 0;
                state = false;
            }
        }
        else
        {
            SetTargetActive(false);
            if (timer > OffDuration)
            {
                timer = 0;
                state = true;
            }
        }

        timer += Time.deltaTime;
    }

    void SetTargetActive(bool active)
    {
        for(int i = 0; i < Targets.Length; i++)
        {
            Targets[i].SetActive(active);
        }
    }
}
