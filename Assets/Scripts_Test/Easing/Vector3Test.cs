using BusinessAlgorithm.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BusinessAlgorithm.Test {
    public class Vector3Test : MonoTestBase {
        public Vector3 startPosition;
        public Vector3 targetPosition;

        private bool isMoving = false;

        void Update() {
            if (isMoving) {

                // 更新当前时间
                currentTime += Time.deltaTime;

                // 根据缓动方法获取当前插值的位置
                Vector3 interpolatedPosition = transform.position;
                if (isWave) {
                    interpolatedPosition = EasingHelper.EasingWave3D(startPosition, targetPosition, currentTime, duration, easingType, easingMode, waveType);
                } else {
                    if (currentTime <= duration) {
                        interpolatedPosition = EasingHelper.Easing3D(startPosition, targetPosition, currentTime, duration, easingType, easingMode);
                    }
                }

                // 应用插值的位置
                transform.position = interpolatedPosition;
            }

            // 按空格键开始动画
            if (Input.GetKeyDown(KeyCode.Space) && !isMoving) {
                currentTime = 0.0f;
                isMoving = true;
            }
        }
    }
}
