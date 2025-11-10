using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public int NumberOfItems { get; private set; }
    public UnityEvent<PlayerInventory> OnItemCollected;

    public void ItemsCollected()
    {
        NumberOfItems++;
        OnItemCollected.Invoke(this);
    }
}
