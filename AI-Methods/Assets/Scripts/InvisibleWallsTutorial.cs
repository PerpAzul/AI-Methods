using System;
using UnityEngine;

public class InvisibleWallsTutorial : MonoBehaviour
{
    [SerializeField] private GameObject text1;
    [SerializeField] private GameObject text2;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text1.SetActive(false);
            text2.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
