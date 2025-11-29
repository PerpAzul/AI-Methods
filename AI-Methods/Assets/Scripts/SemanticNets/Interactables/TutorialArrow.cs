using UnityEngine;

public class TutorialArrow : MonoBehaviour
{
    [SerializeField]
    public GameObject target0;
    [SerializeField]
    public GameObject target1;
    [SerializeField]
    public GameObject target2;

    private GameObject[] targets;
    private int idx = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targets = new GameObject[] { target0, target1, target2 };
    }

    // Update is called once per frame
    void Update()
    {
        if (idx >= targets.Length) {
            this.gameObject.SetActive(false);
            return;
        }
        if (targets[idx].GetComponent<InteractableI>().hasAlreadyInteracted()) {
            Debug.Log("Tutorial arrow moving to next target");
            idx++;
            if (idx >= targets.Length) {
                return;
            }
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector3 dir = (targets[idx].transform.position - player.transform.position).normalized;
            this.transform.localRotation = Quaternion.LookRotation(new Vector3(dir.x, dir.y, dir.z));
            Vector3 angles = this.transform.localEulerAngles;
            this.transform.localEulerAngles = new Vector3(0f, angles.y, -90f);
        }
        float newY = targets[idx].transform.position.y + 1.15f + Mathf.Sin(Time.time * 3f) * 0.3f;
        this.transform.localPosition = new Vector3(targets[idx].transform.position.x, newY, targets[idx].transform.position.z);
    }
}
