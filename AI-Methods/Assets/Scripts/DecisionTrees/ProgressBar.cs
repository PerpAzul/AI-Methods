using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ProgressBar : MonoBehaviour
{
    public Image filler;
    float max = 8.0f;
    public float curr = 0.0f; 
    public GameObject levelFinished;
    public GameObject reset;

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

        if (curr >= 8.0f)
        {
            levelFinished.SetActive(true);
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
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    Time.timeScale = 1f;
    }
}
