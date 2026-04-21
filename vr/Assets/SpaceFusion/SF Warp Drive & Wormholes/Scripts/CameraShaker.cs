using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpaceFusion.SF_Warp.Scripts {
    /// <summary>
    /// Simple camera shaker script that "shakes" the camera by randomly changing the x and y value of the camera object.
    /// in order to make the script work properly please assign the Camera object as child to a CameraHolder object and set the child transform values to zero
    /// you can freely move the cameraHolder in your scene, but you will ensure that the underlying child with he attached camera
    /// will always be reset to position (0,0,0) after each shake cycle, ensuring all future cycles to work as expected 
    /// </summary>
    public class CameraShaker : MonoBehaviour {

        private bool isActive;
        private float magnitude = 0.005f;

        public static CameraShaker instance;

        private void Awake() {
            isActive = false;
            if (instance == null) {
                instance = this;
            }
        }

        public void Shake(float magnitude, float delay, float startDuration, float holdDuration, float stopDuration) {
            if (holdDuration < 0f) {
                throw new Exception("HoldDuration for camera shake should not be below zero!");
            }

            this.magnitude = magnitude;
            if (isActive) {
                Debug.Log("Shake Effect already active!");
                return;
            }

            isActive = true;
            StartCoroutine(ShakeEffect(delay, startDuration, holdDuration, stopDuration));
        }

        private IEnumerator ShakeEffect(float delay, float startDuration, float holdDuration, float stopDuration) {

            const float waitInterval = 0.01f;
            var originalPosition = transform.localPosition;
            var elapsed = 0f;


            yield return new WaitForSeconds(delay);

            var totalDuration = startDuration + holdDuration;
            while (elapsed < totalDuration) {
                var durationInPercentage = elapsed / startDuration;
                var smoothnessFactor = durationInPercentage >= 1 ? 1 : durationInPercentage;
                var x = GetRandomValue(smoothnessFactor);
                var y = GetRandomValue(smoothnessFactor);
                var z = GetRandomValue(smoothnessFactor);
                transform.localPosition = new Vector3(x, y, originalPosition.z);
                 yield return new WaitForSeconds(waitInterval);
                elapsed += waitInterval;
            }
            elapsed = 0f;
            while (elapsed < stopDuration) {
                var durationInPercentage = elapsed / startDuration;
                var smoothnessFactor = 1f - durationInPercentage;
                var x = GetRandomValue(smoothnessFactor);
                var y = GetRandomValue(smoothnessFactor);
                var z = GetRandomValue(smoothnessFactor);
                transform.localPosition = new Vector3(x, y, originalPosition.z);
                elapsed += waitInterval;
                 yield return new WaitForSeconds(waitInterval);
            }
            
            transform.localPosition = originalPosition;
            isActive = false;
        }

        private float GetRandomValue(float smoothnessFactor = 1f) {
            return Random.Range(-0.5f, 0.5f) * magnitude * smoothnessFactor;
        }

    }
}