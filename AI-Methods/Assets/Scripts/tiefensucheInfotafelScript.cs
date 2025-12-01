using UnityEngine;

public class tiefensucheInfotafelScript : MonoBehaviour
{
    public GameObject firstInfoScreen;
    public GameObject secondInfoScreen;
    public GameObject thirdInfoScreen;
    public GameObject animationOne;
    public GameObject animationTwo;
    public GameObject animationThree;
    public GameObject animationFour;
    public GameObject animationFive;
    public GameObject animationSix;
    public GameObject animationSeven;
    public GameObject animationEight;
    public GameObject animationNine;
    public GameObject fourthInfoScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        firstInfoScreen.SetActive(false);
        secondInfoScreen.SetActive(false);
        thirdInfoScreen.SetActive(false);
        fourthInfoScreen.SetActive(false);
        animationOne.SetActive(false);
        animationTwo.SetActive(false);
        animationThree.SetActive(false);
        animationFour.SetActive(false);
        animationFive.SetActive(false);
        animationSix.SetActive(false);
        animationSeven.SetActive(false);
        animationEight.SetActive(false);
        animationNine.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        firstInfoScreen.SetActive(true);
    }
}
