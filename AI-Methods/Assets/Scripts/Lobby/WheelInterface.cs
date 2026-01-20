using UnityEngine;
using System.Collections;

public class WheelInterface : MonoBehaviour
{
    public GameObject wheelSearch;
    public GameObject wheelSemantic;
    public GameObject wheelDecision;
    public ParticleSystem steamEffect;
    public GameObject congratsMessage;

    [SerializeField] GameObject TextSemantic;
    [SerializeField] GameObject TextSearch;
    [SerializeField] GameObject TextDecision;
    [SerializeField] GameObject Image;

    void Awake() {
        int count = 0;

        if (VariableStore.GetCurrentLevelSearch() >= 2) {
            CongratulationsText.congratulationsSearch++;
            if(CongratulationsText.congratulationsSearch == 1){
                StartCoroutine(CongratulationsText.ActivateImage(TextSearch, Image));
            }
            wheelSearch.SetActive(true);
            StartCoroutine(StartSteamEffect());
            count++;
        } else {
            wheelSearch.SetActive(false);
        }

        if (VariableStore.GetCurrentLevelSemantic() >= 2) {
            wheelSemantic.SetActive(true);
            CongratulationsText.congratulationsSemantic++;
            if(CongratulationsText.congratulationsSemantic == 1){
                StartCoroutine(CongratulationsText.ActivateImage(TextSemantic, Image));
            }
            count++;
        } else {
            wheelSemantic.SetActive(false);
        }

        if (VariableStore.GetCurrentLevelDecision() >= 2) {
            wheelDecision.SetActive(true);
            CongratulationsText.congratulationsDecision++;
            if(CongratulationsText.congratulationsDecision == 1){
                StartCoroutine(CongratulationsText.ActivateImage(TextDecision, Image));
            }
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