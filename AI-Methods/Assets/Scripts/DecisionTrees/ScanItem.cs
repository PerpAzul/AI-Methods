using System;
using UnityEngine;

public class ScanItem : MonoBehaviour
{
    public GameObject databaseConsole;
    public NPCGuide guide;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Pickup>() != null)
        {
            if (other.gameObject.name.Equals("crystal_17_2"))
            {
                if (!guide.canScanTutorial)
                {
                    return;
                }
                if (guide) guide.ContinueIfCurrentActionEquals("scan");
            }
            Pickup pickup = other.GetComponent<Pickup>();
            pickup.message = "";
            pickup.isEnabled = false;
            Destroy(other.gameObject);
            Database database = databaseConsole.GetComponent<Database>();
            database.AddNewItem(new(pickup.isMetal, pickup.isDangerous, pickup.isBlueEnergy, pickup.texture2d));
        }
    }
}
