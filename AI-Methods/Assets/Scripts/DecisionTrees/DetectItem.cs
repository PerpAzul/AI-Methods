using System.Collections.Generic;
using UnityEngine;

public class DetectItem : MonoBehaviour
{
    Collider[] intersects;
    Vector3 detectSpot = new Vector3(-5.26f, 1.2f, 0f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private EvaluateDecisionTree evaluateDecisionTree;
    private GameManager gameManager;
    void Start()
    {
        evaluateDecisionTree = GameObject.Find("Holy Machine").GetComponent<EvaluateDecisionTree>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        intersects = Physics.OverlapSphere(detectSpot, 0.4f);
        foreach (var collider in intersects)
        {
            GameObject obj = collider.gameObject;
            Pickup pickup = obj.GetComponent<Pickup>();

            // nur blaue energie
            if (obj.name.StartsWith("decorative_plant"))
            {
                Destroy(obj);
                evaluate(false, false, true, 0);
            }
            // nichts
            else if (obj.name.StartsWith("Trashbin"))
            {
                Destroy(obj);
                evaluate(false, false, false, 1);
            }
            // metall + blaue energie
            else if (obj.name.StartsWith("Beer Can"))
            {
                Destroy(obj);
                evaluate(true, false, true, 2);
            }
            // nur metall
            else if (obj.name.StartsWith("Crate"))
            {
                Destroy(obj);
                evaluate(true, false, false, 3);
            }
            // schädlich + metall
            else if (obj.name.StartsWith("OilDrum"))
            {
                Destroy(obj);
                evaluate(true, true, false, 4);
            }
            // schädlich
            else if (obj.name.StartsWith("Barrel"))
            {
                Destroy(obj);
                evaluate(false, true, false, 5);
            }
            // schädlich + blaue energie
            else if (obj.name.StartsWith("FuelTank"))
            {
                Destroy(obj);
                evaluate(false, true, true, 6);
            }
            // alles
            else if (obj.name.StartsWith("crystal"))
            {
                Destroy(obj);
                evaluate(true, true, true, 7);
            }
        }
    }

    public void evaluate(bool metallic, bool dangerous, bool blueEnergy, int index)
    {
        evaluateDecisionTree.metallic = metallic;
        evaluateDecisionTree.dangerous = dangerous;
        evaluateDecisionTree.blueEnergy = blueEnergy;
        EvaluateDecisionTree.Result res = evaluateDecisionTree.Evaluate();

        switch (res)
        {
            case EvaluateDecisionTree.Result.Good:
                gameManager.check_classification(index, true);
                break;
            case EvaluateDecisionTree.Result.Bad:
                gameManager.check_classification(index, false);
                break;
            default:
                return;
        }
    }
}
