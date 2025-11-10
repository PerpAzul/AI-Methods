using UnityEngine;

public class ChangeGlowColorScript : MonoBehaviour
{
    public Material blueGlow;
    public Material greenGlow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.GetComponent<MeshRenderer>().material = blueGlow;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("turns to green");
        this.GetComponent<MeshRenderer>().material = greenGlow;
    }
}
