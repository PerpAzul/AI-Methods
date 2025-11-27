using System;
using UnityEngine;

public class ScanItem : MonoBehaviour
{
    public GameObject databaseConsole;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Pickup>() != null)
        {
            Destroy(other.gameObject);
            Pickup pickup = other.GetComponent<Pickup>();
            Database database = databaseConsole.GetComponent<Database>();
            database.DisplayNewItem(new(pickup.isMetal, pickup.isDangerous, pickup.isBlueEnergy, pickup.texture2d));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Pickup>() != null)
        {
            Pickup pickup = other.GetComponent<Pickup>();
            // pickup.infoCanvas.SetActive(false);
        }
    }
}
