using UnityEngine;

public class ItemPickup : InteractableI
{
    protected override void Interact()
    {
        Destroy(gameObject);
    }
}
