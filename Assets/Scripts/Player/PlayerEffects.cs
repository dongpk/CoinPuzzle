using System.Collections;
using UnityEngine;


public class PlayerEffects : MonoBehaviour
{
    [SerializeField] private GameObject landingFX;


    public void OnLanded()
    {
        Instantiate(landingFX, transform.position, Quaternion.identity);
    }
   
}
