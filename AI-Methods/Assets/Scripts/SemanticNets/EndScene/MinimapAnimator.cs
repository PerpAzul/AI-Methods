using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MinimapAnimator : MonoBehaviour
{
    [Header("Elements that move")]
    [SerializeField] private Transform[] elementsToMove;

    [Header("Target positions (same order as above)")]
    [SerializeField] private Transform[] targetPositions;

    [Header("Animation Settings")]
    [SerializeField] private float duration = 1.0f;
    [SerializeField] private AnimationCurve easing = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public void AnimateElements()
    {
        StartCoroutine(AnimateRoutine());
    }

    private IEnumerator AnimateRoutine()
    {
        // Fehlercheck: gleiche Länge?
        if (elementsToMove.Length != targetPositions.Length)
        {
            Debug.LogError("elementsToMove und targetPositions haben unterschiedliche Längen!");
            yield break;
        }

        Vector3[] startPositions = new Vector3[elementsToMove.Length];
        Vector3[] endPositions   = new Vector3[targetPositions.Length];

        // Start- und Endpositionen sichern
        for (int i = 0; i < elementsToMove.Length; i++)
        {
            startPositions[i] = elementsToMove[i].position;
            endPositions[i]   = targetPositions[i].position;
        }

        float t = 0f;
        while (t < duration)
        {
            float normalized = t / duration;
            float eased = easing.Evaluate(normalized);

            for (int i = 0; i < elementsToMove.Length; i++)
            {
                elementsToMove[i].position =
                Vector3.Lerp(startPositions[i], endPositions[i], eased);
            }

            t += Time.deltaTime;
            yield return null;
        }

        // Finale Position sicherstellen
        for (int i = 0; i < elementsToMove.Length; i++)
            elementsToMove[i].position = endPositions[i];
    }
}
