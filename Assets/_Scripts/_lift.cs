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
                // Dynamically set the lift's start position to its current local position
                liftStartPosition = liftTransform.localPosition;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && liftCoroutine == null)
            {
                liftCoroutine = StartCoroutine(ActivateLift());
            }
        }

        #endregion

        #region COROUTINES

        private IEnumerator ActivateLift()
        {
            yield return new WaitForSeconds(activationDelay);

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

            // Stop all audio
            if (movingAudio != null)
            {
                movingAudio.Stop();
                movingAudio.loop = false;
            }

            // Wait at the top
            yield return new WaitForSeconds(stayDuration);

            // Move the lift down
            elapsedTime = 0f;
            while (elapsedTime < liftMoveDuration)
            {
                float t = elapsedTime / liftMoveDuration;
                t = t * t * (3f - 2f * t); // Smoothstep easing function
                liftTransform.localPosition = Vector3.Lerp(liftEndPosition, liftStartPosition, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            liftTransform.localPosition = liftStartPosition;

            // Reactivate the lift
            liftCoroutine = null;
        }

        #endregion
    }
}
