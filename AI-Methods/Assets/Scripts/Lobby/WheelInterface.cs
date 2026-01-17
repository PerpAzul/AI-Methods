using UnityEngine;
using System.Collections;

public class WheelInterface : MonoBehaviour
{
    public GameObject wheelSearch;
    public GameObject wheelSemantic;
    public GameObject wheelDecision;
    public ParticleSystem steamEffect;
    public GameObject congratsMessage;

    void Awake() {
        int count = 0;

        if (VariableStore.GetCurrentLevelSearch() >= 2) {
            wheelSearch.SetActive(true);
            StartCoroutine(StartSteamEffect());
            count++;
        } else {
            wheelSearch.SetActive(false);
        }

        if (VariableStore.GetCurrentLevelSemantic() >= 2) {
            wheelSemantic.SetActive(true);
            StartCoroutine(StartSteamEffect());
            count++;
        } else {
            wheelSemantic.SetActive(false);
        }

        if (VariableStore.GetCurrentLevelDecision() >= 2) {
            wheelDecision.SetActive(true);
            StartCoroutine(StartSteamEffect());
            count++;
        } else {
            wheelDecision.SetActive(false);
        }

        // show game finished message once
        if (count == 3) {
            congratsMessage.SetActive(true);
        }
    }

    IEnumerator StartSteamEffect() {
        yield return new WaitForSeconds(4f);
        steamEffect.Play();
    }

    void Update()
    {
        if (!wheelDecision.activeSelf || !wheelSemantic.activeSelf || !wheelSearch.activeSelf) {
            return;
        }
        wheelSearch.transform.Rotate(Vector3.forward, 20 * Time.deltaTime);
        wheelSemantic.transform.Rotate(Vector3.forward, -20 * Time.deltaTime, Space.World);
        wheelDecision.transform.Rotate(Vector3.forward, 15 * Time.deltaTime, Space.Self);
        steamEffect.Play();
    }
}