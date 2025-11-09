using UnityEngine;
using System.Collections;

public class YellowButton : Interactable
{
    [SerializeField]
    private GameObject door;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void Interact()
    {
        door.GetComponent<Animator>().SetBool("isOpen", true);
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2.5f);
        door.GetComponent<Animator>().SetBool("isOpen", false);
    }
}
