using System.Collections;
using UnityEngine;

namespace starskyproductions.minigolf.demo
{
    public class LiftAndBoatController : MonoBehaviour
    {
        #region INSPECTOR PROPERTIES

        [Header("Boat Settings")]
        [SerializeField] private Transform boatTransform;
        [SerializeField] private Vector3 boatStartPosition;
        [SerializeField] private Vector3 boatEndPosition;
        [SerializeField] private float boatMoveDuration = 5f;

        [Header("Lift Settings")]
        [SerializeField] private Transform lift1Transform;
        [SerializeField] private Transform lift2Transform;
        [SerializeField] private Vector3 liftStartPosition;
        [SerializeField] private Vector3 liftEndPosition;
        [SerializeField] private float liftMoveDuration = 3f;
        [SerializeField] private float liftStayDuration = 10f;

        [Header("Player Detection")]
        [SerializeField] private Collider boatTrigger;
        [SerializeField] private Collider lift1Trigger;
        [SerializeField] private Collider lift2Trigger;
        [SerializeField] private float activationDelay = 2f;

        #endregion

        #region PRIVATE FIELDS

        private Coroutine boatCoroutine;
        private Coroutine lift1Coroutine;
        private Coroutine lift2Coroutine;

        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            if (boatTransform != null)
            {
                // Set the boatTransform's position to the serialized start position
                boatTransform.position = boatStartPosition;
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (boatTrigger != null && other == boatTrigger)
                {
                    if (boatCoroutine == null)
                        boatCoroutine = StartCoroutine(BoatSequence());
                }
                else if (lift1Trigger != null && other == lift1Trigger)
                {
                    if (lift1Coroutine == null)
                        lift1Coroutine = StartCoroutine(LiftSequence(lift1Transform));
                }
                else if (lift2Trigger != null && other == lift2Trigger)
                {
                    if (lift2Coroutine == null)
                        lift2Coroutine = StartCoroutine(LiftSequence(lift2Transform));
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // Optional: Reset timers if the player exits the trigger zone prematurely.
        }

        #endregion

        #region PRIVATE METHODS

        // Separate methods for functionality

        #endregion

        #region COROUTINES

        private IEnumerator BoatSequence()
        {
            yield return new WaitForSeconds(activationDelay);

            yield return StartCoroutine(MoveObject(boatTransform, boatStartPosition, boatEndPosition, boatMoveDuration));

            boatTransform.position = boatEndPosition;
        }

        private IEnumerator LiftSequence(Transform liftTransform)
        {
            yield return new WaitForSeconds(activationDelay);

            yield return StartCoroutine(MoveObject(liftTransform, liftStartPosition, liftEndPosition, liftMoveDuration));

            liftTransform.position = liftEndPosition;

            yield return new WaitForSeconds(liftStayDuration);

            yield return StartCoroutine(MoveObject(liftTransform, liftEndPosition, liftStartPosition, liftMoveDuration));

            liftTransform.position = liftStartPosition;
        }

        private IEnumerator MoveObject(Transform objTransform, Vector3 startPos, Vector3 endPos, float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                objTransform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        #endregion
    }
}
