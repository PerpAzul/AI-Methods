using UnityEngine;
using UnityEngine.SceneManagement;

public class barrierScript : MonoBehaviour
{
    public GameObject leftPillar;
    public GameObject rightPillar;

    public Material redGlowPillar;
    public Material greenGlowPillar;

    private bool isRed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leftPillar.GetComponent<Renderer>().material = greenGlowPillar;
        rightPillar.GetComponent<Renderer>().material = greenGlowPillar;
        isRed = false;
    }

    // Update is called once per frame
    void Update()
    {
        float random = Random.Range(0.0f, 10.0f);
        if (random < 0.01f)
        {
            isRed = !isRed;
        }

        if (isRed)
        {
            leftPillar.GetComponent<Renderer>().material = redGlowPillar;
            rightPillar.GetComponent<Renderer>().material = redGlowPillar;
        }
        else {
            leftPillar.GetComponent<Renderer>().material = greenGlowPillar;
            rightPillar.GetComponent<Renderer>().material = greenGlowPillar;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isRed)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }
}
