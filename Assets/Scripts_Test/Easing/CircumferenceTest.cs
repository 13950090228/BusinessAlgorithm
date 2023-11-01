using BusinessAlgorithm.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BusinessAlgorithm.Test {

    public class CircumferenceTest : MonoTestBase {
        public GameObject obj;
        public float radius;
        public float angle;
        public bool isClockwise;

        private bool isMoving = false;
        private float initialAngle;
        void Start() {
            // 计算当前角度
            Vector3 direction = transform.position - obj.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            initialAngle = rotation.eulerAngles.y;
        }

        void Update() {
            if (isMoving) {

                // 更新当前时间
                currentTime += Time.deltaTime;

                // 根据缓动方法获取当前插值的位置
                Vector3 interpolatedPosition = transform.position;
                if (isWave) {
                    //interpolatedPosition = EasingHelper.EasingWave3D(startPosition, targetPosition, currentTime, duration, easingType, easingMode, waveType);
                } else {
                    if (currentTime <= duration) {
                        interpolatedPosition = EasingHelper.EasingCircularMotion(obj.transform.position, radius, duration, currentTime, initialAngle, angle, isClockwise, easingType, easingMode);
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
