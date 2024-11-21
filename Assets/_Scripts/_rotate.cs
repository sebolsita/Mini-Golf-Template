using UnityEngine;

namespace starskyproductions.minigolf.demo
{
    public class ObjectRotator : MonoBehaviour
    {
        [Header("Rotation Settings")]
        [SerializeField] private Vector3 rotationAxis = Vector3.up; // Default to Y-axis rotation
        [SerializeField] private float rotationSpeed = 10f; // Speed in degrees per second

        void Update()
        {
            // Rotate the object around the chosen axis
            transform.Rotate(rotationAxis.normalized * rotationSpeed * Time.deltaTime);
        }
    }
}
