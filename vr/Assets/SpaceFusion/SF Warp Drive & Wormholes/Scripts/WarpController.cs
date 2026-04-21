using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace SpaceFusion.SF_Warp.Scripts {
    public class WarpController : MonoBehaviour {
        public VisualEffect warpVFX;
        public MeshRenderer[] wormholeEffects;
        [Header("delay time of the wormhole effects, since the warpVFX already needs a small warmup time")]
        public float wormholeDelay;

        [Header("Time until warp reaches its max speed")]
        public float startDuration;

        [Header("Time that max speed is active before decreasing again")]
        public float warpHoldDuration;

        [Header("Time from max speed until complete stop")]
        public float stopDuration;

        [Header("Camera Shake (0 = no shake)")]
        public float magnitude = 0.005f;
        

        private const float IntensityChangeCooldown = 0.1f;
        private float _increment;
        private float _decrement;


        private bool _isWarpEffectActive;
        private bool _isWormHoleActive;

        private const string VFXIntensity = "Intensity";
        private const string ShaderIntensity = "_Intensity";
        private static readonly int Intensity = Shader.PropertyToID(ShaderIntensity);

        void Start() {
            warpVFX.Stop();
            // also consider the waitTime between increments, so that we exactly need the defined amount of seconds until we reach max effect of the warp
            // we also only need half of the effectDuration since we increase it 
            _increment = 1f / (startDuration / IntensityChangeCooldown);
            _decrement = 1f / (stopDuration / IntensityChangeCooldown);
            warpVFX.SetFloat(VFXIntensity, 0);
            foreach (var effect in wormholeEffects) {
                effect.material.SetFloat(Intensity, 0);
            }

            if (wormholeDelay > warpHoldDuration) {
                throw new Exception("wormhole delay time should not be greater than the warpHoldDuration");
            }
        }

        void Update() {
            if (Input.GetKey(KeyCode.Space)) {
                StartEffect();
            }
        }

        public void StartEffect() {
            if (_isWarpEffectActive || _isWormHoleActive) {
                return;
            }

            _isWarpEffectActive = true;
            _isWormHoleActive = true;
            StartCoroutine(StartWarpVFX());
            StartCoroutine(StartWormholeEffect());

            var holdDuration = warpHoldDuration - wormholeDelay;
            CameraShaker.instance.Shake(magnitude,
                wormholeDelay,
                startDuration,
                holdDuration,
                stopDuration * 0.8f);
        }

        private IEnumerator StartWarpVFX() {
            // if already active should not do anything
            if (_isWarpEffectActive) {
                yield return null;
            }

            // warp buildup
            warpVFX.Play();
            var amount = 0f;
            while (amount < 1) {
                amount += _increment;
                warpVFX.SetFloat(VFXIntensity, amount);
                yield return new WaitForSeconds(IntensityChangeCooldown);
            }

            // max warp speed reached, hold for defined amount of time
            yield return new WaitForSeconds(warpHoldDuration);

            while (amount > 0) {
                amount -= _decrement;
                if (amount <= 0 + _decrement) {
                    amount = 0;
                }

                warpVFX.SetFloat(VFXIntensity, amount);
                yield return new WaitForSeconds(IntensityChangeCooldown);
            }

            warpVFX.Stop();
            _isWarpEffectActive = false;
        }

        private IEnumerator StartWormholeEffect() {
            // if already active should not do anything
            if (_isWormHoleActive) {
                yield return null;
            }

            yield return new WaitForSeconds(wormholeDelay);
            var amount = 0f;
            while (amount < 1) {
                amount += _increment;
                foreach (var effect in wormholeEffects) {
                    effect.material.SetFloat(Intensity, amount);
                }

                yield return new WaitForSeconds(IntensityChangeCooldown);
            }

            // max warp speed reached, hold for defined amount of time
            // since we started with a potential delay, we should subtract the delay time from the hold duration,
            // so we can finish the effect at the same time as the VFX
            yield return new WaitForSeconds(warpHoldDuration - wormholeDelay);

            while (amount > 0) {
                amount -= _decrement;
                if (amount <= 0 + _decrement) {
                    amount = 0;
                }

                foreach (var effect in wormholeEffects) {
                    effect.material.SetFloat(Intensity, amount);
                }

                yield return new WaitForSeconds(IntensityChangeCooldown);
            }

            _isWormHoleActive = false;
        }
    }
}