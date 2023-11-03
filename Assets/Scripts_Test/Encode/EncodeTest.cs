
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using BusinessAlgorithm.Encode;
using UnityEngine;

namespace BusinessAlgorithm.Test {

    public class EncodeTest : MonoBehaviour {
        Stopwatch watch = new Stopwatch();

        void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                UIntCheck();
                IntCheck();
                ULongCheck();
                LongCheck();
            }
        }

        void UIntCheck() {
            uint val;
            uint random0 = 0;
            val = EncodeFunction.UInt32Decode(EncodeFunction.UInt32Encode(random0));
            if (val != random0) {
                Debug.LogError("转码错误:" + random0);
            }

            uint randomMin = uint.MinValue;
            val = EncodeFunction.UInt32Decode(EncodeFunction.UInt32Encode(randomMin));
            if (val != randomMin) {
                Debug.LogError("转码错误:" + randomMin);
            }

            uint randomMax = uint.MaxValue;
            val = EncodeFunction.UInt32Decode(EncodeFunction.UInt32Encode(randomMax));
            if (val != randomMax) {
                Debug.LogError("转码错误:" + randomMax);
            }

            for (int i = 0; i < 10000; i++) {
                uint random = GenerateRandomUInt();
                val = EncodeFunction.UInt32Decode(EncodeFunction.UInt32Encode(random));
                Debug.Log($"值：val:{val},randow:{random}");

                if (val != random) {
                    Debug.LogError("转码错误:" + random);
                }
            }

            Debug.Log("运算完毕");
        }

        void ULongCheck() {
            ulong val;
            ulong random0 = 0;
            val = EncodeFunction.UInt64Decode(EncodeFunction.UInt64Encode(random0));
            if (val != random0) {
                Debug.LogError("转码错误:" + random0);
            }

            ulong randomMin = ulong.MinValue;
            val = EncodeFunction.UInt64Decode(EncodeFunction.UInt64Encode(randomMin));
            if (val != randomMin) {
                Debug.LogError("转码错误:" + randomMin);
            }

            ulong randomMax = ulong.MaxValue;
            val = EncodeFunction.UInt64Decode(EncodeFunction.UInt64Encode(randomMax));
            if (val != randomMax) {
                Debug.LogError("转码错误:" + randomMax);
            }

            for (int i = 0; i < 10000; i++) {
                ulong random = GenerateRandomULong();
                val = EncodeFunction.UInt64Decode(EncodeFunction.UInt64Encode(random));
                Debug.Log($"值：val:{val},randow:{random}");

                if (val != random) {
                    Debug.LogError("转码错误:" + random);
                }
            }

            Debug.Log("运算完毕");
        }

        void LongCheck() {
            long val;
            long random0 = 0;
            val = EncodeFunction.Int64Decode(EncodeFunction.Int64Encode(random0));
            if (val != random0) {
                Debug.LogError("转码错误:" + random0);
            }

            long randomMin = long.MinValue;
            val = EncodeFunction.Int64Decode(EncodeFunction.Int64Encode(randomMin));
            if (val != randomMin) {
                Debug.LogError("转码错误:" + randomMin);
            }

            long randomMax = long.MaxValue;
            val = EncodeFunction.Int64Decode(EncodeFunction.Int64Encode(randomMax));
            if (val != randomMax) {
                Debug.LogError("转码错误:" + randomMax);
            }

            for (int i = 0; i < 10000; i++) {
                long random = GenerateRandomLong();
                val = EncodeFunction.Int64Decode(EncodeFunction.Int64Encode(random));
                Debug.Log($"值：val:{val},randow:{random}");
                if (val != random) {
                    Debug.LogError("转码错误:" + random);
                }
            }

            Debug.Log("运算完毕");
        }

        void IntCheck() {
            int val;
            int random0 = 0;
            val = EncodeFunction.Int32Decode(EncodeFunction.Int32Encode(random0));
            if (val != random0) {
                Debug.LogError("转码错误:" + random0);
            }

            int randomMin = int.MinValue;
            val = EncodeFunction.Int32Decode(EncodeFunction.Int32Encode(randomMin));
            if (val != randomMin) {
                Debug.LogError("转码错误:" + randomMin);
            }

            int randomMax = int.MaxValue;
            val = EncodeFunction.Int32Decode(EncodeFunction.Int32Encode(randomMax));
            if (val != randomMax) {
                Debug.LogError("转码错误:" + randomMax);
            }

            for (int i = 0; i < 10000; i++) {
                int random = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
                val = EncodeFunction.Int32Decode(EncodeFunction.Int32Encode(random));
                Debug.Log($"值：val:{val},randow:{random}");
                if (val != random) {
                    Debug.LogError("转码错误:" + random);
                }
            }

            Debug.Log("运算完毕");
        }

        /// <summary>
        /// 创建 long 类型随机数
        /// </summary>
        /// <returns></returns>
        public long GenerateRandomLong() {
            byte[] buffer = new byte[8];
            System.Random random = new System.Random();
            random.NextBytes(buffer);
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>
        /// 创建 ulong 类型随机数
        /// </summary>
        /// <returns></returns>
        public ulong GenerateRandomULong() {
            byte[] buffer = new byte[8];
            System.Random random = new System.Random();
            random.NextBytes(buffer);
            ulong result = BitConverter.ToUInt64(buffer, 0);
            return result;
        }

        /// <summary>
        /// 创建 uint 类型随机数
        /// </summary>
        /// <returns></returns>
        public uint GenerateRandomUInt() {
            byte[] buffer = new byte[5];
            System.Random random = new System.Random();
            random.NextBytes(buffer);
            uint result = BitConverter.ToUInt32(buffer, 0);
            return result;
        }
    }
}