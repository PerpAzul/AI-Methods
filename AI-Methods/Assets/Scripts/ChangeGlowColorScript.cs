using UnityEngine;

public class ChangeGlowColorScript : MonoBehaviour
{
    public bool nextPlatform = false;

    public GameObject center;
    public GameObject cornerOne;
    public GameObject cornerTwo;
    public GameObject cornerThree;
    public GameObject cornerFour;

    public Material blueGlow;
    public Material greenGlow;
    public Material redGlow;

    public RootPlatformScript PlatformRoot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //nextPlatform = false;
        center.GetComponent<Renderer>().material = blueGlow;
        cornerOne.GetComponent<Renderer>().material = blueGlow;
        cornerTwo.GetComponent<Renderer>().material = blueGlow;
        cornerThree.GetComponent<Renderer>().material = blueGlow;
        cornerFour.GetComponent<Renderer>().material = blueGlow;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (nextPlatform)
        {
            center.GetComponent<Renderer>().material = greenGlow;
            cornerOne.GetComponent<Renderer>().material = greenGlow;
            cornerTwo.GetComponent<Renderer>().material = greenGlow;
            cornerThree.GetComponent<Renderer>().material = greenGlow;
            cornerFour.GetComponent<Renderer>().material = greenGlow;
            PlatformRoot.PlatformAnswer = true;
        }
        else
        {
            center.GetComponent<Renderer>().material = redGlow;
            cornerOne.GetComponent<Renderer>().material = redGlow;
            cornerTwo.GetComponent<Renderer>().material = redGlow;
            cornerThree.GetComponent<Renderer>().material = redGlow;
            cornerFour.GetComponent<Renderer>().material = redGlow;
        }
    }
}
