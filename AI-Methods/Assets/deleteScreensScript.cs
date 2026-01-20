using UnityEngine;

public class deleteScreensScript : MonoBehaviour
{
    public GameObject screenOne;
    public GameObject screenTwo;
    public GameObject screenThree;
    public GameObject screenFour;
    public GameObject screenFive;
    public GameObject screenSix;
    public GameObject screenSeven;
    public GameObject screenEight;
    public GameObject screenNine;
    public GameObject screenTen;
    public GameObject screenEleven;
    public GameObject screenTwelve;
    public GameObject screenThirteen;
    public GameObject screenFourteen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        screenOne.SetActive(false);
        screenTwo.SetActive(false);
        screenThree.SetActive(false);
        screenFour.SetActive(false);
        screenFive.SetActive(false);
        screenSix.SetActive(false);
        screenSeven.SetActive(false);
        screenEight.SetActive(false);
        screenNine.SetActive(false);
        screenTen.SetActive(false);
        screenEleven.SetActive(false);
        screenTwelve.SetActive(false);
        screenThirteen.SetActive(false);
        screenFourteen.SetActive(false);
    }
}
