using UnityEngine;

public class Pickup: MonoBehaviour
{
    bool canPickup;
    [SerializeField] GameObject target;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canPickup)
        {
            this.transform.parent = target.transform;
            this.transform.localEulerAngles = Vector3.zero;
            this.GetComponent<Rigidbody>().isKinematic = true;
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            Debug.Log("Success");
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
