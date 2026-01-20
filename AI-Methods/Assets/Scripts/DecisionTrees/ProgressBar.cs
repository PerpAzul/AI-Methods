using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ProgressBar : MonoBehaviour
{
    public Image filler;
    float max = 8.0f;
    public float curr = 0.0f; 
    public GameObject levelFinished;
    public GameObject reset;
    public TextMeshProUGUI fillText;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI pointsEnd;
    private bool finished = false;
    public int points = 0;
    private float completionMultiplier = 1.0f;
    private float time = 0.0f;
    public bool[] hasPickedUp = {false, false, false, false, false, false, false, false};

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        setFill();
        setPoints();
        // make sure you can still interact with ui after pausing
        if (reset.activeSelf || levelFinished.activeSelf)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (curr >= 8.0f)
        {
            if (!finished)
            {
                levelFinished.SetActive(true);
                Debug.Log(time);
                // time-based completion bonus
                completionMultiplier = completionMultiplier - (0.0002f * time);
                points += Decimal.ToInt32(Math.Round(new decimal (60 * completionMultiplier), 1));
                points = points > 300 ? 300 : points;
                pointsEnd.text = $"Punkte: {points}";
            }
            finished = true;
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            reset.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void setFill()
    {
        Vector3 newScale = filler.rectTransform.localScale;
        newScale.x = curr / max;
        filler.rectTransform.localScale = newScale;
        if (fillText)
        {
            fillText.text = $"{curr}/8";
        }
    }

    void setPoints()
    {
        if (pointsText)
        {
            pointsText.text = $"Punkte: {points}";
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    Time.timeScale = 1f;
    }
}
