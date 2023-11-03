using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BusinessAlgorithm.Encode {

    public static class EncodeFunction {
        /// <summary>
        /// 对 int 类型进行 zigzag 编码
        /// </summary>
        public static int RawInt32ToZigZag(Int32 value) {
            return (value << 1) ^ (value >> 31);
        }

        /// <summary>
        /// 对 long 类型进行 zigzag 编码
        /// </summary>
        public static long RawInt64ToZigZag(Int64 value) {
            return (value << 1) ^ (value >> 63);
        }

        /// <summary>
        /// 获取 uint 长度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetUInt32Size(uint value) {
            int result = 0;
            do {
                result++;
                value = Int32LogicalRightMove(value, 7);
            }
            while (value != 0);

            return result;
        }

        /// <summary>
        /// 获取 ulong 长度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetUInt64Size(ulong value) {
            int result = 0;
            do {
                result++;
                value = Int64LogicalRightMove(value, 7);
            }
            while (value != 0);

            return result;
        }

        static uint Int32LogicalRightMove(uint value, int bit) {
            if (bit != 0) {
                value >>= 1;
                value &= UInt32.MaxValue;
                value >>= bit - 1;
            }

            return value;
        }

        static ulong Int64LogicalRightMove(ulong value, int bit) {
            if (bit != 0) {
                value >>= 1;
                value &= UInt64.MaxValue;
                value >>= bit - 1;
            }

            return value;
        }

        /// <summary>
        /// long 可变长度编码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] Int64Encode(long value) {
            ulong zigzagValue = (ulong)RawInt64ToZigZag(value); // 对有符号长整型进行 zigzag 编码
            return UInt64Encode(zigzagValue);
        }

        /// <summary>
        /// long 可变长度解码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long Int64Decode(byte[] data) {
            ulong zigzagValue = UInt64Decode(data);
            // 对 zigzag 编码进行反转，得到原始有符号长整型值
            return (long)(zigzagValue >> 1) ^ -(long)(zigzagValue & 1);
        }

        /// <summary>
        /// ulong 可变长度编码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] UInt64Encode(ulong value) {
            byte[] buffer = new byte[GetUInt64Size(value)]; // 最多需要10个字节存储
            int index = 0;
            do {
                byte b = (byte)(value & 0x7f); // 取低7位
                value >>= 7; // 右移7位，去掉已取出的7位
                if (value != 0) {
                    b |= 0x80; // 标记高位为1，表示还有后续字节
                }
                buffer[index++] = b;
            } while (value != 0); // 直到所有字节都写入完成

            byte[] result = new byte[index];
            Array.Copy(buffer, result, index); // 将实际写入的字节复制到一个新数组中
            return result;
        }

        /// <summary>
        /// ulong 可变长度解码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ulong UInt64Decode(byte[] bytes) {
            ulong result = 0;
            int shift = 0;
            int offset = 0;

            for (int i = 0; i < bytes.Length; i++) {
                byte b = bytes[i];
                result |= (ulong)(b & 0x7f) << shift;
                shift += 7;
                offset++;

                if ((b & 0x80) == 0) {
                    break; // 遇到最后一个字节，结束解码
                }
            }

            return result;
        }

        /// <summary>
        /// int 可变长度编码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] Int32Encode(int value) {
            uint zigzagValue = (uint)RawInt32ToZigZag(value); // 对有符号长整型进行 zigzag 编码
            return UInt32Encode(zigzagValue);
        }

        /// <summary>
        /// int 可变长度解码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int Int32Decode(byte[] data) {
            uint zigzagValue = UInt32Decode(data);
            // 对 zigzag 编码进行反转，得到原始有符号长整型值
            return (int)(zigzagValue >> 1) ^ -(int)(zigzagValue & 1);
        }

        /// <summary>
        /// uint 可变长度解码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] UInt32Encode(uint value) {
            byte[] buffer = new byte[GetUInt32Size(value)]; // 最多需要5个字节存储
            int index = 0;
            do {
                byte b = (byte)(value & 0x7f); // 取低7位
                value >>= 7; // 右移7位，去掉已取出的7位
                if (value != 0) {
                    b |= 0x80; // 标记高位为1，表示还有后续字节
                }
                buffer[index++] = b;
            } while (value != 0); // 直到所有字节都写入完成

            return buffer;
        }

        /// <summary>
        /// uint 可变长度解码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static uint UInt32Decode(byte[] data) {
            uint result = 0;
            int shift = 0;
            int offset = 0;
            while (true) {
                byte b = data[offset++];
                result |= (uint)(b & 0x7f) << shift; // 取低7位，并左移相应的位数
                if ((b & 0x80) == 0) // 高位为0表示已经解码完成
                {
                    break;
                }
                shift += 7; // 继续解码下一个字节
            }
            return result;
        }
    }
}