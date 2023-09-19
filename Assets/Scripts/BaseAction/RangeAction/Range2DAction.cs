using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BusinessAlgorithm.BaseAction {
    public static class Range2DAction {

        /// <summary>
        /// 计算起始点与目标点的距离,包含目标点的体积
        /// </summary>
        /// <param name="start">起始点</param>
        /// <param name="target">目标点</param>
        /// <param name="targetBodySize">目标体积</param>
        /// <returns></returns>
        public static float GetStartCenterToTargetDisWithBodySize(Vector2 start, Vector2 target, float targetBodySize) {
            float dis = Vector2.Distance(start, target);
            return dis - targetBodySize;
        }

        /// <summary>
        /// 计算起始点与目标点的距离,包含双方的体积
        /// </summary>
        /// <param name="start">起始点</param>
        /// <param name="startBodySize">起始点体积</param>
        /// <param name="target">目标点</param>
        /// <param name="targetBodySize">目标体积</param>
        /// <returns></returns>
        public static float GetStartToTargetDisWithBodySize(Vector2 start, float startBodySize, Vector2 target, float targetBodySize) {
            float dis = Vector2.Distance(start, target);
            return dis - (startBodySize + targetBodySize);
        }

        /// <summary>
        /// 判断起始点和目标点的距离是否在指定范围内（包含目标体积）
        /// </summary>
        public static bool CheckTargetInRangeWithBodySize(Vector2 start, Vector2 target, float targetBodySize, float radius) {

            float dis = GetStartCenterToTargetDisWithBodySize(start, target, targetBodySize);
            if (dis <= radius) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取指定起始点位置，指定朝向和距离的目标点
        /// </summary>
        /// <param name="start">起始点</param>
        /// <param name="angle">角度</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static Vector2 GetPosByDirAndDis(Vector2 start, float angle, float length) {
            // 将角度转换为弧度
            float radians = angle * Mathf.Deg2Rad;

            // 使用预先计算的余弦和正弦值来计算目标点的位置
            float cosValue = Mathf.Sin(radians);
            float sinValue = Mathf.Cos(radians);

            float x = start.x + length * cosValue;
            float y = start.y + length * sinValue;

            return new Vector2(x, y);
        }

        /// <summary>
        /// 获取点A到线段BC的最短距离
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static float GetShortestDistanceToLineSegment(Vector2 A, Vector2 B, Vector2 C) {
            Vector2 BA = A - B;
            Vector2 BC = C - B;
            float DotProduct = Vector2.Dot(BA, BC);
            float BCLengthSquared = BC.sqrMagnitude;
            float ShortestDistanceSquared = BA.sqrMagnitude - (DotProduct * DotProduct / BCLengthSquared);
            float ShortestDistance = Mathf.Sqrt(ShortestDistanceSquared);
            return ShortestDistance;
        }

        /// <summary>
        /// 判断目标点是否处于指定角度的扇形范围内（包含目标点体积）
        /// </summary>
        /// <param name="start">起始点</param>
        /// <param name="target">目标点</param>
        /// <param name="targetBodySize">目标体积</param>
        /// <param name="radius">扇形半径</param>
        /// <param name="angle">扇形角度</param>
        /// <param name="direction">扇形方向（顺时针）</param>
        /// <returns></returns>
        public static bool CheckInSectorRangeOfDirectionWithBodySize(Vector2 start, Vector2 target, float targetBodySize, float radius,
            float angle, float direction = 0) {
            Vector2 dirBase = target - start;
            Vector2 forward = Quaternion.Euler(0, 0, direction) * Vector2.up;
            float curAngle = Vector2.Angle(forward, dirBase.normalized);

            float curDis = GetStartCenterToTargetDisWithBodySize(start, target, targetBodySize);
            if (curDis <= radius) {
                if (curAngle <= angle * 0.5f) {
                    return true;
                } else {
                    Vector2 pos1 = GetPosByDirAndDis(start, direction - angle * 0.5f, radius);
                    Vector2 pos2 = GetPosByDirAndDis(start, direction + angle * 0.5f, radius);
                    return GetShortestDistanceToLineSegment(target, start, pos1) <= targetBodySize || GetShortestDistanceToLineSegment(target, start, pos2) <= targetBodySize;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断目标点是否处于指定角度的扇形范围内
        /// </summary>
        /// <param name="start">起始点</param>
        /// <param name="target">目标点</param>
        /// <param name="radius">扇形半径</param>
        /// <param name="angle">扇形角度</param>
        /// <param name="direction">扇形方向（顺时针）</param>
        /// <returns></returns>
        public static bool CheckInSectorRangeOfDirection(Vector2 start, Vector2 target, float radius,
            float angle, float direction = 0) {
            Vector2 dirBase = target - start;
            Vector2 forward = Quaternion.Euler(0, 0, direction) * Vector2.up;
            float curAngle = Vector2.Angle(forward, dirBase.normalized);

            float curDis = Vector2.Distance(start, target);
            if (curDis <= radius) {
                if (curAngle <= angle * 0.5f) {
                    return true;
                } else {
                    Vector2 pos1 = GetPosByDirAndDis(start, direction - angle * 0.5f, radius);
                    Vector2 pos2 = GetPosByDirAndDis(start, direction + angle * 0.5f, radius);
                    return GetShortestDistanceToLineSegment(target, start, pos1) <= 0 || GetShortestDistanceToLineSegment(target, start, pos2) <= 0;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断目标点是否在矩形范围内（包含目标体积）
        /// </summary>
        /// <param name="start">起始点</param>
        /// <param name="target">目标点</param>
        /// <param name="targetBodySize">目标体积</param>
        /// <param name="direction">矩形方向</param>
        /// <param name="length">长</param>
        /// <param name="width">宽</param>
        /// <param name="rectRangType">矩形范围类型</param>
        /// <param name="rectCenterRangType">矩形中心类型</param>
        /// <returns></returns>
        // 判断目标点是否在矩形范围内（包含目标体积）
        public static bool CheckTargetInRectangleWithBodySize(Vector2 start, Vector2 target, float targetBodySize, float direction, float length, float width, RectCenterRangType rectCenterRangType = RectCenterRangType.Center) {
            start = GetRectangleCenterPoint(start, direction, length, rectCenterRangType);

            // 对比对角线和目标点的距离长度
            width = width * 0.5f;
            float targetToStartDis = GetStartCenterToTargetDisWithBodySize(start, target, targetBodySize);
            if (targetToStartDis <= 0) {
                return true;
            }

            bool isPassMaxDis = false;
            float diagonalLength;
            length = length * 0.5f;
            diagonalLength = Mathf.Sqrt(length * length + width * width);
            isPassMaxDis = targetToStartDis > diagonalLength;

            if (isPassMaxDis) {
                return false;
            }

            Vector2 forward = Quaternion.Euler(0, 0, direction) * Vector2.up;
            Vector2 right = Quaternion.Euler(0, 0, direction) * Vector2.right;

            Vector2 dir = target - start;
            float lengthDic = Vector2.Dot(dir, forward);

            if (!isPassMaxDis) {
                float rightDistance = Vector2.Dot(dir, right);
                if (0 <= rightDistance) {
                    rightDistance = Mathf.Min(Mathf.Abs(rightDistance),
                        Mathf.Abs(rightDistance - targetBodySize));
                } else {
                    rightDistance = Mathf.Min(Mathf.Abs(rightDistance),
                        Mathf.Abs(rightDistance + targetBodySize));
                }
                if (Mathf.Abs(rightDistance) <= width) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 判断目标点是否在矩形范围内
        /// </summary>
        /// <param name="start">起始点</param>
        /// <param name="target">目标点</param>
        /// <param name="direction">矩形方向</param>
        /// <param name="length">长</param>
        /// <param name="width">宽</param>
        /// <param name="rectRangType">矩形范围类型</param>
        /// <param name="rectCenterRangType">矩形中心类型</param>
        /// <returns></returns>
        public static bool CheckTargetInRectangle(Vector2 start, Vector2 target, float direction, float length, float width, RectCenterRangType rectCenterRangType = RectCenterRangType.Center) {
            start = GetRectangleCenterPoint(start, direction, length, rectCenterRangType);
            Vector2 forward = Quaternion.Euler(0, 0, direction) * Vector2.up;
            Vector2 right = Quaternion.Euler(0, 0, direction) * Vector2.right;
            width = width * 0.5f;

            Vector2 dir = target - start;
            float lengthDic = Vector2.Dot(dir, forward);

            if (Mathf.Abs(lengthDic) <= length) {
                float rightDistance = Vector2.Dot(dir, right);
                if (Mathf.Abs(rightDistance) <= width) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取矩形中心点
        /// </summary>
        /// <param name="start">起始点</param>
        /// <param name="direction">朝向角度</param>
        /// <param name="length">矩形长（与朝向平行的那边）</param>
        /// <param name="rectCenterRangType">矩形中心点枚举</param>
        /// <returns></returns>
        public static Vector3 GetRectangleCenterPoint(Vector2 start, float direction, float length, RectCenterRangType rectCenterRangType) {
            if (rectCenterRangType == RectCenterRangType.Bottom) {
                return GetPosByDirAndDis(start, direction, length * 0.5f);
            }

            return start;
        }

        /// <summary>
        /// 让点以某一点为圆心旋转指定角度
        /// </summary>
        /// <param name="point">旋转点</param>
        /// <param name="center">中心点</param>
        /// <param name="angle">角度</param>
        /// <returns></returns>
        public static Vector2 RotatePointAroundCenter(Vector2 point, Vector2 center, float angle) {
            // 将角度转换为弧度
            float radians = angle * Mathf.Deg2Rad;

            // 计算旋转后的点坐标
            float cos = Mathf.Cos(radians);
            float sin = Mathf.Sin(radians);

            float x = cos * (point.x - center.x) - sin * (point.y - center.y) + center.x;
            float y = sin * (point.x - center.x) + cos * (point.y - center.y) + center.y;

            return new Vector2(x, y);
        }

    }
}