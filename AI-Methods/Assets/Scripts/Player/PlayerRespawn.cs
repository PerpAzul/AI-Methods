using System;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private GameObject teleport;
    [SerializeField] private GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var controller = player.GetComponent<CharacterController>();
            controller.enabled = false;
            player.transform.position = teleport.transform.position;
            controller.enabled = true;
        }
    }
}
