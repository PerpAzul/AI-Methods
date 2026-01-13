using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToNormal : MonoBehaviour
{
    public void finishLevel()
    {
        Debug.Log("Click");
        SceneManager.LoadScene("DecisionTreeSmall");
    }
}
