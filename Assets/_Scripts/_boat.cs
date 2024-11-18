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

        [Header("Audio Settings")]
        [SerializeField] private AudioSource movingAudio;
        [SerializeField] private AudioSource slowingDownAudio;

        #endregion

        #region PRIVATE FIELDS

        private Vector3 boatStartPosition;
        private Vector3 boatEndPosition = new Vector3(8.22f, 1.624444f, 20.752f);
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

            // Play moving audio in a loop
            if (movingAudio != null)
            {
                movingAudio.loop = true;
                movingAudio.Play();
            }

            // Move the boat from start to end position in local space with easing
            float elapsedTime = 0f;
            while (elapsedTime < boatMoveDuration)
            {
                float t = elapsedTime / boatMoveDuration;
                t = t * t * (3f - 2f * t); // Smoothstep easing function
                boatTransform.localPosition = Vector3.Lerp(boatStartPosition, boatEndPosition, t);

                // Stop moving audio and play slowing down audio when nearing the end
                if (elapsedTime >= boatMoveDuration * 0.8f && slowingDownAudio != null && !slowingDownAudio.isPlaying)
                {
                    movingAudio?.Stop();
                    slowingDownAudio.Play();
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            boatTransform.localPosition = boatEndPosition;

            // Stop all audio
            if (movingAudio != null)
            {
                movingAudio.Stop();
                movingAudio.loop = false;
            }

            // Deactivate this script
            this.enabled = false;

            boatCoroutine = null;
        }

        #endregion
    }
}
