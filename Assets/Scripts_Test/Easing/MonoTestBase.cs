using BusinessAlgorithm.Easing;
using UnityEngine;

namespace BusinessAlgorithm.Test {
    public class MonoTestBase : MonoBehaviour {
        public float duration = 2.0f;                           // 持续时间
        public EasingType easingType = EasingType.Quad;         // 缓动函数类型
        public EasingMode easingMode = EasingMode.None;         // 缓动模式
        public EasingWaveType waveType = EasingWaveType.Spring; // 波形类型
        public bool isWave = false;

        protected float currentTime = 0.0f;
        protected bool isAnimating = false;

    }
}