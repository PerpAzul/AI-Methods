using UnityEngine;

public class DetectItem : MonoBehaviour
{
    Collider[] intersects;
    Vector3 detectSpot = new Vector3(-5.26f, 1.2f, 0f);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private EvaluateDecisionTree evaluateDecisionTree;
    void Start()
    {
        evaluateDecisionTree = GameObject.Find("Holy Machine").GetComponent<EvaluateDecisionTree>();
    }

    // Update is called once per frame
    void Update()
    {
        intersects = Physics.OverlapSphere(detectSpot, 0.4f);

        foreach (var collider in intersects)
        {
            GameObject obj = collider.gameObject;

            // nur blaue energie
            if (obj.name.StartsWith("decorative_plant"))
            {
                Destroy(obj);
                evaluate(false, false, true);
            }
            // nichts
            else if (obj.name.StartsWith("Trashbin"))
            {
                Destroy(obj);
                evaluate(false, false, false);
            }
            // metall + blaue energie
            else if (obj.name.StartsWith("Beer Can"))
            {
                Destroy(obj);
                evaluate(true, false, true);
            }
            // nur metall
            else if (obj.name.StartsWith("Crate"))
            {
                Destroy(obj);
                evaluate(true, false, false);
            }
            // schädlich + metall
            else if (obj.name.StartsWith("OilDrum"))
            {
                Destroy(obj);
                evaluate(true, true, false);
            }
            // schädlich
            else if (obj.name.StartsWith("Barrel"))
            {
                Destroy(obj);
                evaluate(false, true, false);
            }
            // schädlich + blaue energie
            else if (obj.name.StartsWith("FuelTank"))
            {
                Destroy(obj);
                evaluate(false, true, true);
            }
            // alles
            else if (obj.name.StartsWith("crystal"))
            {
                Destroy(obj);
                evaluate(true, true, true);
            }
        }
    }

    public void evaluate(bool metallic, bool dangerous, bool blueEnergy)
    {
        evaluateDecisionTree.metallic = metallic;
        evaluateDecisionTree.dangerous = dangerous;
        evaluateDecisionTree.blueEnergy = blueEnergy;
        evaluateDecisionTree.Evaluate();
    }
}
