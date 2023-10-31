using UnityEngine;

namespace BusinessAlgorithm.Easing {

    public static class EasingHelper {

        delegate float EasingHandler(float start, float end, float current, float duration, EasingMode mode);

        public static Color EasingColor(Color start, Color end, float current, float duration, EasingType type, EasingMode mode = EasingMode.None) {
            var r = Easing(start.r, end.r, current, duration, type, mode);
            var g = Easing(start.g, end.g, current, duration, type, mode);
            var b = Easing(start.b, end.b, current, duration, type, mode);
            var a = Easing(start.a, end.a, current, duration, type, mode);
            return new Color(r, g, b, a);
        }

        public static Color EasingWaveColor(Color start, Color end, float current, float duration, EasingType type, EasingMode mode, EasingWaveType waveType) {
            var val = WaveTimeConversion(current, duration, waveType);
            return EasingColor(start, end, val, duration, type, mode);
        }

        public static Color32 EasingColor32(Color32 start, Color32 end, float current, float duration, EasingType type, EasingMode mode = EasingMode.None) {
            var r = EasingByte(start.r, end.r, current, duration, type, mode);
            var g = EasingByte(start.g, end.g, current, duration, type, mode);
            var b = EasingByte(start.b, end.b, current, duration, type, mode);
            var a = EasingByte(start.a, end.a, current, duration, type, mode);
            return new Color32(r, g, b, a);
        }

        public static Color EasingWaveColor32(Color start, Color end, float current, float duration, EasingType type, EasingMode mode, EasingWaveType waveType) {
            var val = WaveTimeConversion(current, duration, waveType);
            return EasingColor32(start, end, val, duration, type, mode);
        }

        public static Vector2 Easing2D(Vector2 start, Vector2 end, float current, float duration, EasingType type, EasingMode mode = EasingMode.None) {
            var x = Easing(start.x, end.x, current, duration, type, mode);
            var y = Easing(start.y, end.y, current, duration, type, mode);
            return new Vector2(x, y);
        }

        public static Vector2 EasingWave2D(Vector2 start, Vector2 end, float current, float duration, EasingType type, EasingMode mode, EasingWaveType waveType) {
            var val = WaveTimeConversion(current, duration, waveType);
            return Easing2D(start, end, val, duration, type, mode);
        }

        public static Vector3 Easing3D(Vector3 start, Vector3 end, float current, float duration, EasingType type, EasingMode mode = EasingMode.None) {
            var x = Easing(start.x, end.x, current, duration, type, mode);
            var y = Easing(start.y, end.y, current, duration, type, mode);
            var z = Easing(start.z, end.z, current, duration, type, mode);
            return new Vector3(x, y, z);
        }

        public static Vector2 EasingWave3D(Vector2 start, Vector2 end, float current, float duration, EasingType type, EasingMode mode, EasingWaveType waveType) {
            var val = WaveTimeConversion(current, duration, waveType);
            return Easing3D(start, end, val, duration, type, mode);
        }

        public static Quaternion EasingQuaternion(Quaternion start, Quaternion end, float current, float duration, EasingType type, EasingMode mode = EasingMode.None) {
            var x = Easing(start.x, end.x, current, duration, type, mode);
            var y = Easing(start.y, end.y, current, duration, type, mode);
            var z = Easing(start.z, end.z, current, duration, type, mode);
            var w = Easing(start.w, end.w, current, duration, type, mode);

            return new Quaternion(x, y, z, w);
        }

        public static Quaternion EasingWaveQuaternion(Quaternion start, Quaternion end, float current, float duration, EasingType type, EasingMode mode, EasingWaveType waveType) {
            var val = WaveTimeConversion(current, duration, waveType);
            return EasingQuaternion(start, end, val, duration, type, mode);
        }

        /// <summary>
        /// 圆周运动
        /// </summary>
        /// <param name="centerPosition">中心点位置</param>
        /// <param name="radius">圆周半径</param>
        /// <param name="duration">运动时间</param>
        /// <param name="currentTime">当前时间</param>
        /// <param name="initialAngle">运动开始时运动物体在世界坐标下相对于中心点的角度</param>
        /// <param name="angle">想要旋转的角度</param>
        /// <param name="isClockwise">是否顺时针</param>
        /// <returns></returns>
        public static Vector3 EasingCircularMotion(Vector3 centerPosition, float radius, float duration, float currentTime, float initialAngle, float angle, bool isClockwise, EasingType type, EasingMode mode) {
            // 角速度
            float angularSpeed = angle / duration;

            // 顺逆时针判断
            if (!isClockwise) {
                angularSpeed = -angularSpeed;
            }

            float easedTime = Easing(0f, duration, currentTime, duration, type, mode);

            float currentAngle = initialAngle + easedTime * angularSpeed;

            float x = centerPosition.x + radius * Mathf.Sin(currentAngle * Mathf.Deg2Rad);
            float y = centerPosition.y;
            float z = centerPosition.z + radius * Mathf.Cos(currentAngle * Mathf.Deg2Rad);

            return new Vector3(x, y, z);
        }


        public static float Easing(float start, float end, float current, float duration, EasingType type, EasingMode mode = EasingMode.None) {
            EasingHandler easingFunction = GetEasingFunction(type, mode);
            var t = current;
            var b = start;
            var c = end - start;
            var d = duration;
            return easingFunction(t, b, c, d, mode);
        }

        public static byte EasingByte(byte start, byte end, float current, float duration, EasingType type, EasingMode mode = EasingMode.None) {
            EasingHandler easingFunction = GetEasingFunction(type, mode);
            var t = current;
            var b = start;
            var c = end - start;
            var d = duration;
            return (byte)easingFunction(t, b, c, d, mode);
        }

        private static EasingHandler GetEasingFunction(EasingType type, EasingMode mode = EasingMode.None) {

            if (type == EasingType.Sine) {
                return EasingFunction.Sine;
            }
            if (type == EasingType.Quad) {
                return EasingFunction.Quad;
            }
            if (type == EasingType.Cubic) {
                return EasingFunction.Cubic;
            }
            if (type == EasingType.Quart) {
                return EasingFunction.Quart;
            }
            if (type == EasingType.Quint) {
                return EasingFunction.Quint;
            }
            if (type == EasingType.Expo) {
                return EasingFunction.Expo;
            }
            if (type == EasingType.Circ) {
                return EasingFunction.Circ;
            }
            if (type == EasingType.Back) {
                return EasingFunction.Back;
            }
            if (type == EasingType.Elastic) {
                return EasingFunction.Elastic;
            }
            if (type == EasingType.Bounce) {
                return EasingFunction.Bounce;
            }
            return EasingFunction.Linear;

        }

        private static float WaveTimeConversion(float current, float duration, EasingWaveType waveType) {
            float val = current;

            if (waveType == EasingWaveType.Spring) {
                if (current > duration) {
                    int num = (int)(current / duration);
                    if (num % 2 == 0) {
                        val = current % duration;
                    } else {
                        val = duration - (current % duration);
                    }
                }
            } else if (waveType == EasingWaveType.Reset) {
                if (current > duration) {
                    val = current % duration;
                }
            }

            return val;
        }

    }

}