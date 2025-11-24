using UnityEngine;

public class RootPlatformScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool PlatformAnswer;

    public ChangeGlowColorScript PlatformOne;
    public ChangeGlowColorScript PlatformTwo;
    public ChangeGlowColorScript PlatformThree;
    public ChangeGlowColorScript PlatformFour;
    public ChangeGlowColorScript PlatformFive;
    public ChangeGlowColorScript PlatformSix;
    public ChangeGlowColorScript PlatformSeven;
    /*public ChangeGlowColorScript PlatformEight;
    public ChangeGlowColorScript PlatformNine;
    public ChangeGlowColorScript PlatformTen;*/
    void Start()
    {
        PlatformAnswer = false;
        PlatformOne.nextPlatform = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlatformAnswer)
        { 
            PlatformAnswer = false;
            //nächstes in PlatformReihenfolge auf true setzen
            /*if (PlatformNine.nextPlatform)
            {
                PlatformTen.nextPlatform = true;
            }
            else if (PlatformEight.nextPlatform)
            {
                PlatformNine.nextPlatform = true;
            }
            else if (PlatformSeven.nextPlatform)
            {
                PlatformEight.nextPlatform = true;
            }*/
            if (PlatformSix.nextPlatform)
            {
                PlatformSeven.nextPlatform = true;
            }
            else if (PlatformFive.nextPlatform)
            {
                PlatformSix.nextPlatform = true;
            }
            else if (PlatformFour.nextPlatform)
            {
                PlatformFive.nextPlatform = true;
            }
            else if (PlatformThree.nextPlatform)
            {
                PlatformFour.nextPlatform = true;
            }
            else if (PlatformTwo.nextPlatform)
            {
                PlatformThree.nextPlatform = true;
            }
            else if (PlatformOne.nextPlatform)
            {
                PlatformTwo.nextPlatform = true;
            }
        }
    }
}
