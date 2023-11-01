using UnityEngine;
using BusinessAlgorithm.Easing;
using PlasticPipe.PlasticProtocol.Messages;

namespace BusinessAlgorithm.Test {
    public class QuaternionTest : MonoTestBase {
        Quaternion startRotation;
        Quaternion targetRotation;

        public Vector3 startEuler;
        public Vector3 endEuler;


        void Start() {
            // 设置起始和目标旋转
            startRotation = Quaternion.Euler(startEuler);
            targetRotation = Quaternion.Euler(endEuler);
        }

        void Update() {
            if (isAnimating) {
                // 更新当前时间
                currentTime += Time.deltaTime;

                // 根据缓动方法获取当前插值的旋转
                Quaternion interpolatedRotation = transform.rotation;
                if (isWave) {
                    interpolatedRotation = EasingHelper.EasingWaveQuaternion(startRotation, targetRotation, currentTime, duration, easingType, easingMode, waveType);
                } else {
                    if (currentTime <= duration) {
                        interpolatedRotation = EasingHelper.EasingQuaternion(startRotation, targetRotation, currentTime, duration, easingType, easingMode);
                    }
                }
                // 应用插值的旋转
                transform.rotation = interpolatedRotation;
            }

            // 按空格键开始动画
            if (Input.GetKeyDown(KeyCode.Space) && !isAnimating) {
                startRotation = Quaternion.Euler(startEuler);
                targetRotation = Quaternion.Euler(endEuler);
                currentTime = 0.0f;
                isAnimating = true;
            }
        }
    }
}
