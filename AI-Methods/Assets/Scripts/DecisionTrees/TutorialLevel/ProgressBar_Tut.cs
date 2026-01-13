using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;

public class ProgressBar_Tut : MonoBehaviour
{
    public Image filler;
    float max = 4.0f;
    public float curr = 0.0f; 
    public GameObject levelFinished;
    public GameObject reset;
    public TextMeshProUGUI fillText;
    private bool finished = false;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        setFill();
        // make sure you can still interact with ui after pausing
        if (reset.activeSelf || levelFinished.activeSelf)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (curr >= 4.0f)
        {
            if (!finished)
            {
                levelFinished.SetActive(true);
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
            fillText.text = $"{curr}/4";
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    Time.timeScale = 1f;
    }
}
