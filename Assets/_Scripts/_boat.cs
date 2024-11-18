using System.Collections;
using UnityEngine;

namespace starskyproductions.minigolf.demo
{
    public class BoatController : MonoBehaviour
    {
        #region INSPECTOR PROPERTIES

        [Header("Boat Settings")]
        [SerializeField] private Transform boatTransform;
        [SerializeField] private float boatMoveDuration = 5f;
        [SerializeField] private float activationDelay = 2f;

        [Header("Player Detection")]
        [SerializeField] private Collider boatTrigger;

        #endregion

        #region PRIVATE FIELDS

        private Vector3 boatStartPosition;
        private Vector3 boatEndPosition = new Vector3(0f, 13.663f, 0f);
        private Coroutine boatCoroutine;

        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            if (boatTransform != null)
            {
                // Dynamically set the boat's start position to its current local position
                boatStartPosition = boatTransform.localPosition;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && boatCoroutine == null)
            {
                boatCoroutine = StartCoroutine(ActivateBoat());
            }
        }

        #endregion

        #region COROUTINES

        private IEnumerator ActivateBoat()
        {
            yield return new WaitForSeconds(activationDelay);

            // Move the boat from start to end position in local space
            float elapsedTime = 0f;
            while (elapsedTime < boatMoveDuration)
            {
                boatTransform.localPosition = Vector3.Lerp(boatStartPosition, boatEndPosition, elapsedTime / boatMoveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            boatTransform.localPosition = boatEndPosition;
            boatCoroutine = null;
        }

        #endregion
    }
}
