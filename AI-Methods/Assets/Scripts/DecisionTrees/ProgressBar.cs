using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProgressBar : MonoBehaviour
{
    public Image filler;
    float max = 8.0f;
    public float curr = 0.0f; 
    public GameObject levelFinished;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        setFill();
        if (curr >= 8.0f)
        {
            levelFinished.SetActive(true);
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
