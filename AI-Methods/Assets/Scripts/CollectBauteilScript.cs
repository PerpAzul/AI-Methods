using System;
using UnityEngine;

public class CollectBauteilScript : MonoBehaviour
{

    public ChangeGlowColorScript bauteilPlatformScript;
    public GameObject geschafftScreen;
    //bool completedAllPlatforms;
    
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
            //put "win screen"
        }
        
    }
}
