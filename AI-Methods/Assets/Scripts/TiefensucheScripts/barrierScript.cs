using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class barrierScript : MonoBehaviour
{
    public GameObject leftPillar;
    public GameObject rightPillar;
    public GameObject durchgang;

    public Material redGlowPillar;
    public Material greenGlowPillar;

    private bool isRed;
    public RootPlatformScript PlatformRoot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leftPillar.GetComponent<Renderer>().material = greenGlowPillar;
        rightPillar.GetComponent<Renderer>().material = greenGlowPillar;
        isRed = false;
        durchgang.SetActive(false);

        StartCoroutine(timeredChange());
    }

    // Update is called once per frame
    /*void Update()
    {
        float random = Random.Range(0.0f, 10.0f);
        if (random < 0.01f)
        {
            isRed = !isRed;

            if (isRed)
            {
                leftPillar.GetComponent<Renderer>().material = redGlowPillar;
                rightPillar.GetComponent<Renderer>().material = redGlowPillar;
                durchgang.SetActive(true);
            }
            else
            {
                leftPillar.GetComponent<Renderer>().material = greenGlowPillar;
                rightPillar.GetComponent<Renderer>().material = greenGlowPillar;
                durchgang.SetActive(false);
            }
        }        
    }*/

    IEnumerator timeredChange()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(5);

            isRed = !isRed;
            if (isRed)
            {
                leftPillar.GetComponent<Renderer>().material = redGlowPillar;
                rightPillar.GetComponent<Renderer>().material = redGlowPillar;
                durchgang.SetActive(true);
            }
            else
            {
                leftPillar.GetComponent<Renderer>().material = greenGlowPillar;
                rightPillar.GetComponent<Renderer>().material = greenGlowPillar;
                durchgang.SetActive(false);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isRed)
        {
            //string currentSceneName = SceneManager.GetActiveScene().name;
            //SceneManager.LoadScene(currentSceneName);
            PlatformRoot.counterWrongPlatforms++;
        }
    }
}
