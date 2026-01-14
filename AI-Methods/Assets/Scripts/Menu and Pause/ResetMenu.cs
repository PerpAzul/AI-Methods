using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetMenu : MonoBehaviour
{
    public GameObject OptionsMenu;
    public GameObject ResetConfirmationMenu;

    public void Awake() {
        ResetConfirmationMenu.SetActive(false);
    }

    public void ResetButton() {
        OptionsMenu.SetActive(false);
        ResetConfirmationMenu.SetActive(true);
    }

    public void CancelResetButton() {
        ResetConfirmationMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }

    public void ConfirmResetButton() {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
