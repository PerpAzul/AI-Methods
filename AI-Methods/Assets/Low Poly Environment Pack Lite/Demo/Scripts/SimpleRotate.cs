namespace EliteIT.LPNaturePack.Demo
{
    using UnityEngine;

    public class SimpleRotate : MonoBehaviour
    {
        [SerializeField] private Vector3 rotateAxis;
        [SerializeField] private float rotateSpeed;

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(rotateAxis, rotateSpeed * Time.deltaTime);
        }
    }

}