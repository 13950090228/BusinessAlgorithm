using BusinessAlgorithm.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BusinessAlgorithm.Test {

    public class ColorTest : MonoTestBase {
        public Color32 startColor;
        public Color32 endColor;
        public MeshRenderer meshRenderer;

        Color32 meshColor => meshRenderer.material.color;


        void Start() {
        }

        void Update() {
            if (isAnimating) {
                // 更新当前时间
                currentTime += Time.deltaTime;

                // 根据缓动方法获取当前插值的旋转
                Color32 materialColor = meshColor;
                if (isWave) {
                    materialColor = EasingHelper.EasingWaveColor32(startColor, endColor, currentTime, duration, easingType, easingMode, waveType);
                } else {
                    if (currentTime <= duration) {
                        materialColor = EasingHelper.EasingColor32(startColor, endColor, currentTime, duration, easingType, easingMode);
                    }
                }
                // 应用插值的旋转
                meshRenderer.material.color = materialColor;
            }

            // 按空格键开始动画
            if (Input.GetKeyDown(KeyCode.Space) && !isAnimating) {
                currentTime = 0.0f;
                isAnimating = true;
            }
        }
    }
}