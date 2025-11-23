using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToNextLevel : MonoBehaviour
{
    public Button button;
    void Start()
    {
        button.onClick.AddListener(() => Debug.Log("Clicked"));
        Debug.Log("Add listener");
    }
    void OnDestroy()
    {
        button.onClick.RemoveListener(NextLevel);
    }
    public void NextLevel()
    {
        Debug.Log("Done");
        SceneManager.LoadScene("Lobby");
    }
}
