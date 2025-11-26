using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*
     * Musterlösung: Akzeptiert wird: alles mit blauer Energie, metal + dangerous
     */
    private bool[] isGood = {true, false, true, false, true, false, true, true};
    private bool[] wasCorrectlyClassified = {false, false, false, false, false, false, false, false};
    [SerializeField] GameObject greenLight;
    [SerializeField] GameObject redLight;
    [SerializeField] GameObject levelComplete;
    private float lightTimer = 5.0f;
    private bool lightActive = false;
    private bool isGreen = false;
    private ProgressBar progressBar;
    private int correctClassifications = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        progressBar = GameObject.Find("Progress Bar").GetComponent<ProgressBar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lightActive)
        {
            lightTimer -= Time.deltaTime;

            if (lightTimer <= 0.0f)
            {
                turnOffLight();
            }
        }
    }

    public void check_classification(int item, bool useful)
    {
        if (item >= 8)
        {
            return;
        }

        if (isGood[item] == useful)
        {
            // correct
            lightTimer = 5.0f;
            lightActive = true;
            isGreen = true;
            greenLight.SetActive(true);
            if (!wasCorrectlyClassified[item])
            {
                wasCorrectlyClassified[item] = true;
                correctClassifications++;
                progressBar.curr = (float) correctClassifications;
                if (correctClassifications >= 8)
                {
                    levelFinished();
                }
            }
        } else
        {
            //incorrect 
            lightTimer = 5.0f;
            lightActive = true;
            isGreen = false;
            redLight.SetActive(true);
        }
    }

    void turnOffLight()
    {
        if (isGreen)
        {
            lightActive = false;
            greenLight.SetActive(false);
        } else
        {
            lightActive = false;
            redLight.SetActive(false);
        }
    }

    void levelFinished()
    {
        levelComplete.SetActive(true);
    }

    public void resetProgress()
    {
        correctClassifications = 0;
        progressBar.curr = 0.0f;
        wasCorrectlyClassified = new bool[]{false, false, false, false, false, false, false, false};
    }
}
