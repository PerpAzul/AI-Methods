using UnityEngine;

public class ScanItem_Tut : MonoBehaviour
{
    public GameObject databaseConsole;
    public NPCGuide guide;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Pickup_Tut>() != null)
        {
            if (!guide.canScanTutorial)
            {
                return;
            }
            if (other.GetComponent<Pickup_Tut>().isRed && other.GetComponent<Pickup_Tut>().isFruit)
            {
                if (guide) guide.ContinueIfCurrentActionEquals("tomato_scan");
            }
            if (!other.GetComponent<Pickup_Tut>().isRed && other.GetComponent<Pickup_Tut>().isFruit)
            {
                if (guide) guide.ContinueIfCurrentActionEquals("banana_scan");
            }
            if (other.GetComponent<Pickup_Tut>().isRed && !other.GetComponent<Pickup_Tut>().isFruit)
            {
                if (guide) guide.ContinueIfCurrentActionEquals("watermelon_scan");
            }
            if (!other.GetComponent<Pickup_Tut>().isRed && !other.GetComponent<Pickup_Tut>().isFruit)
            {
                if (guide) guide.ContinueIfCurrentActionEquals("carrot_scan");
            }

            Pickup_Tut pickup = other.GetComponent<Pickup_Tut>();
            pickup.message = "";
            pickup.isEnabled = false;
            Destroy(other.gameObject);
            Database_Tut database = databaseConsole.GetComponent<Database_Tut>();
            guide.canPickup = true;
            database.AddNewItem(new(pickup.isRed, pickup.isFruit, pickup.texture2d));
        }
    }
}
