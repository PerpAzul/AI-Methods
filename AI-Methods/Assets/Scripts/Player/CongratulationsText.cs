using UnityEngine;
using System.Collections;

public static class CongratulationsText
{
    public static int congratulationsSemantic = 0;
    public static int congratulationsSearch = 0;
    public static int congratulationsDecision = 0;

   public static IEnumerator ActivateImage(GameObject obj, GameObject Image)
    {
        Image.SetActive(true);
        obj.SetActive(true);
        yield return new WaitForSeconds(10f);
        obj.SetActive(false);
        Image.SetActive(false);
    }
}
