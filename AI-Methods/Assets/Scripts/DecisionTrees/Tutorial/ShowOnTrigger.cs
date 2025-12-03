using UnityEngine;

public class ShowOnTrigger : MonoBehaviour
{
    public GameObject toShow;
    public bool initial;

    private void Awake()
    {
        toShow.SetActive(initial);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            toShow.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            toShow.SetActive(false);
        }
    }
}
