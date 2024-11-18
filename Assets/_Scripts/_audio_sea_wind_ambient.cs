using UnityEngine;

namespace starskyproductions.minigolf.demo
{
    public class PlayerHeightAudio : MonoBehaviour
    {
        #region INSPECTOR PROPERTIES

        [Header("Player Settings")]
        [SerializeField] private Transform playerTransform;

        [Header("Audio Settings")]
        [SerializeField] private AudioSource groundLevelAudio;
        [SerializeField] private AudioSource upperLevelAudio;

        [Header("Height Settings")]
        [SerializeField] private float groundLevel = 5f;
        [SerializeField] private float upperLevel = 29f;
        [SerializeField, Range(0f, 1f)] private float transitionSmoothness = 0.1f;

        #endregion

        #region PRIVATE FIELDS

        private float currentGroundVolume = 1f;
        private float currentUpperVolume = 0f;

        #endregion

        #region UNITY METHODS

        private void Update()
        {
            if (playerTransform == null || groundLevelAudio == null || upperLevelAudio == null)
                return;

            // Get the player's height
            float playerHeight = playerTransform.position.y;

            // Calculate the normalized weight for the upper and ground level audio
            float upperWeight = Mathf.Clamp01((playerHeight - groundLevel) / (upperLevel - groundLevel));
            float groundWeight = 1f - upperWeight;

            // Smoothly transition the volume levels
            currentGroundVolume = Mathf.Lerp(currentGroundVolume, groundWeight * 0.9f, transitionSmoothness);
            currentUpperVolume = Mathf.Lerp(currentUpperVolume, upperWeight * 0.9f, transitionSmoothness);

            // Apply volume levels to the audio sources
            groundLevelAudio.volume = currentGroundVolume;
            upperLevelAudio.volume = currentUpperVolume;

            // Ensure audio sources are playing
            if (!groundLevelAudio.isPlaying) groundLevelAudio.Play();
            if (!upperLevelAudio.isPlaying) upperLevelAudio.Play();
        }

        #endregion
    }
}
