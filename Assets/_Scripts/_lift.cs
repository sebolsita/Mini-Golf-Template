using System.Collections;
using UnityEngine;

namespace starskyproductions.minigolf.demo
{
    public class LiftController : MonoBehaviour
    {
        #region INSPECTOR PROPERTIES

        [Header("Lift Settings")]
        [SerializeField] private Transform liftTransform;
        [SerializeField] private Vector3 liftEndPosition;
        [SerializeField] private float liftMoveDuration = 5f;
        [SerializeField] private float activationDelay = 2f;
        [SerializeField] private float stayDuration = 5f;

        [Header("Player Detection")]
        [SerializeField] private Collider liftTrigger;

        [Header("Audio Settings")]
        [SerializeField] private AudioSource movingAudio;
        [SerializeField] private AudioSource slowingDownAudio;

        #endregion

        #region PRIVATE FIELDS

        private Vector3 liftStartPosition;
        private Coroutine liftCoroutine;

        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            if (liftTransform != null)
            {
                // Set the lift's start position to its current local position
                liftStartPosition = liftTransform.localPosition;
                Debug.Log("Lift start position set to: " + liftStartPosition);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Trigger entered by: " + other.name);

            if (other.CompareTag("Player"))
            {
                Debug.Log("Player detected in lift trigger.");
                if (liftCoroutine == null)
                {
                    Debug.Log("Starting lift coroutine.");
                    liftCoroutine = StartCoroutine(ActivateLift());
                }
                else
                {
                    Debug.Log("Lift coroutine is already running.");
                }
            }
        }

        #endregion

        #region COROUTINES

        private IEnumerator ActivateLift()
        {
            yield return new WaitForSeconds(activationDelay);

            Debug.Log("Lift moving up.");

            // Play moving audio in a loop
            if (movingAudio != null)
            {
                movingAudio.loop = true;
                movingAudio.Play();
            }

            // Move the lift up with easing
            float elapsedTime = 0f;
            while (elapsedTime < liftMoveDuration)
            {
                float t = elapsedTime / liftMoveDuration;
                t = t * t * (3f - 2f * t); // Smoothstep easing function
                liftTransform.localPosition = Vector3.Lerp(liftStartPosition, liftEndPosition, t);
                Debug.Log("Lift position during upward movement: " + liftTransform.localPosition);

                // Stop moving audio and play slowing down audio when nearing the top
                if (elapsedTime >= liftMoveDuration * 0.8f && slowingDownAudio != null && !slowingDownAudio.isPlaying)
                {
                    movingAudio?.Stop();
                    slowingDownAudio.Play();
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            liftTransform.localPosition = liftEndPosition;
            Debug.Log("Lift reached the top at: " + liftEndPosition);

            // Stop all audio
            if (movingAudio != null)
            {
                movingAudio.Stop();
                movingAudio.loop = false;
            }

            // Wait at the top
            yield return new WaitForSeconds(stayDuration);
            Debug.Log("Lift staying at the top.");

            // Play moving audio again for going down
            if (movingAudio != null)
            {
                movingAudio.loop = true;
                movingAudio.Play();
            }

            // Move the lift down
            elapsedTime = 0f;
            while (elapsedTime < liftMoveDuration)
            {
                float t = elapsedTime / liftMoveDuration;
                t = t * t * (3f - 2f * t); // Smoothstep easing function
                liftTransform.localPosition = Vector3.Lerp(liftEndPosition, liftStartPosition, t);
                Debug.Log("Lift position during downward movement: " + liftTransform.localPosition);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            liftTransform.localPosition = liftStartPosition;
            Debug.Log("Lift returned to start position at: " + liftStartPosition);

            // Stop all audio
            if (movingAudio != null)
            {
                movingAudio.Stop();
                movingAudio.loop = false;
            }

            // Reactivate the lift
            liftCoroutine = null;
            Debug.Log("Lift coroutine ended, ready for next activation.");
        }

        #endregion
    }
}