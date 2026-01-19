using UnityEngine;

public class heartLivesScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject heartOne;
    public GameObject heartTwo;
    public GameObject heartThree;
    public RootPlatformScript PlatformRoot;
    void Start()
    {
        heartOne.SetActive(true);
        heartTwo.SetActive(true);
        heartThree.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlatformRoot.counterWrongPlatforms == 1)
        {
            heartThree.SetActive(false);
        }
        else if (PlatformRoot.counterWrongPlatforms == 2)
        {
            heartTwo.SetActive(false);
        }
    }
}
