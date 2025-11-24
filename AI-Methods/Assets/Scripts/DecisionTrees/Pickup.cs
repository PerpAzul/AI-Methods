using UnityEngine;

public class Pickup: MonoBehaviour
{
    bool canPickup;
    bool isPickingUp;
    public GameObject target;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canPickup && !isPickingUp)
        {
            isPickingUp = true;
            this.transform.parent = target.transform;
            this.transform.localEulerAngles = Vector3.zero;
            this.GetComponent<Rigidbody>().isKinematic = true;
        }

        else if (Input.GetKeyDown(KeyCode.F) && isPickingUp)
        {
            isPickingUp = false;
            this.transform.parent = null;
            this.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        canPickup = true;
    }

    void OnTriggerExit(Collider other)
    {
        canPickup = false;
    }
}
