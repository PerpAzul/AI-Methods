using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToNextLevel : MonoBehaviour
{
    public void finishLevel()
    {
        Debug.Log("Click");
        SceneManager.LoadScene("Lobby German");
    }
}
