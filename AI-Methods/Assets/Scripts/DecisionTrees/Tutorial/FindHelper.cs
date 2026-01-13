using UnityEngine;
public class FindHelper : MonoBehaviour
{
    // time in seconds to spawn arrow
    private float hintTime = 45.0f;
    // saves whether the player has found: book_blue_energy, book_dangerous, plant, crate, bin, radioactive barrel, explosive barrel, gas tank, can
    private bool[] hasFound = {false, false, false, false, false, false, false, false, false};
    [SerializeField] private GameObject arrowCanvas;
    private BounceEffect bounceEffect;
    private Vector3[] arrowTransforms = {new Vector3(49.6f, 7.9f, -13.6f), new Vector3(-22.7f, 4f, -4.1f), new Vector3(3.1f, 4, -1.5f), new Vector3(9.5f, 4, -0.5f), new Vector3(-6.6f, 4, 9.4f), new Vector3(16.4f, 4, 3.9f), new Vector3(8.5f, 4, 0.7f), new Vector3(9.9f, 4, 6.3f), new Vector3(1.1f, 4, -13.9f)};
    void Start()
    {
        
    }

    void Update()
    {
        hintTime -= Time.deltaTime;
        if (hintTime <= 0.0f)
        {
            for (int i = 0; i < hasFound.Length; i++)
            {
                if (!hasFound[i])
                {
                    arrowCanvas.SetActive(true);
                    bounceEffect = GameObject.Find("findArrow").GetComponent<BounceEffect>();
                    arrowCanvas.transform.position = arrowTransforms[i];
                    bounceEffect.startPos = arrowTransforms[i];
                    hintTime = 45.0f;
                    return;
                }
            }
        }
    }

    public void find(int index)
    {
        hintTime = 45.0f;
        arrowCanvas.SetActive(false);
        hasFound[index] = true;
    }
}
