using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectBauteilScript : MonoBehaviour
{

    public ChangeGlowColorScript bauteilPlatformScript;
    public GameObject geschafftScreen;
    //bool completedAllPlatforms;
    [SerializeField] private string nextScene;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //completedAllPlatforms = false;
        geschafftScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bauteilPlatformScript.nextPlatform)
        {
            //Debug.Log("found Bauteil!!");
            geschafftScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            //put "win screen"
        }
        
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(nextScene);
    }
}
