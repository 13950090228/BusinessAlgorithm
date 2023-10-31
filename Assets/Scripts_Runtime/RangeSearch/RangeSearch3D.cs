using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BusinessAlgorithm.RangeSearch {

    public static class RangeSearch3D {

        /// <summary>
        /// 计算起始点与目标点的距离,包含目标点的体积
        /// </summary>
        /// <param name="start">起始点</param>
        /// <param name="target">目标点</param>
        /// <param name="targetSizeRadius">目标体积</param>
        /// <returns></returns>
        public static float GetStartCenterToTargetDisWithBodySize(Vector3 start, Vector3 target, float targetSizeRadius) {
            float dis = Vector3.Distance(start, target);
            return dis - targetSizeRadius;
        }

        /// <summary>
        /// 计算起始点与目标点的距离,包含双方的体积
        /// </summary>
        /// <param name="start">起始点</param>
        /// <param name="startBodySize">起始点体积</param>
        /// <param name="target">目标点</param>
        /// <param name="targetSizeRadius">目标体积</param>
        /// <returns></returns>
        public static float GetStartToTargetDisWithBodySize(Vector3 start, float startBodySize, Vector3 target, float targetSizeRadius) {
            float dis = Vector3.Distance(start, target);
            return dis - (startBodySize + targetSizeRadius);
        }

        /// <summary>
        /// 判断起始点和目标点的距离是否在指定范围内（包含目标体积）
        /// </summary>
        public static bool CheckTargetInRangeWithBodySize(Vector3 start, Vector3 target, float targetSizeRadius, float radius) {

            float dis = GetStartCenterToTargetDisWithBodySize(start, target, targetSizeRadius);
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
        public static Vector3 GetPosByDirAndDis(Vector3 start, float angle, float length) {
            Vector3 forward = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            return start + forward * length;
        }

        /// <summary>
        /// 获取点A到线段BC的最短距离
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static float GetShortestDistanceToLineSegment(Vector3 A, Vector3 B, Vector3 C) {
            Vector3 BA = A - B;
            Vector3 BC = C - B;
            float DotProduct = Vector3.Dot(BA, BC);
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
        /// <param name="targetSizeRadius">目标体积</param>
        /// <param name="radius">扇形半径</param>
        /// <param name="angle">扇形角度</param>
        /// <param name="direction">扇形方向（顺时针）</param>
        /// <returns></returns>
        public static bool CheckInSectorRangeOfDirectionWithBodySize(Vector3 start, Vector3 target, float targetSizeRadius, float radius,
            float angle, float direction = 0) {
            Vector3 dirBase = target - start;
            Vector3 forward = Quaternion.Euler(0, direction, 0) * Vector3.forward;
            float curAngle = Vector3.Angle(forward, dirBase.normalized);

            float curDis = GetStartCenterToTargetDisWithBodySize(start, target, targetSizeRadius);
            if (curDis <= radius) {
                if (curAngle <= angle * 0.5f) {
                    return true;
                } else {
                    Vector3 pos1 = GetPosByDirAndDis(start, direction - angle * 0.5f, radius);
                    Vector3 pos2 = GetPosByDirAndDis(start, direction + angle * 0.5f, radius);
                    return GetShortestDistanceToLineSegment(target, start, pos1) <= targetSizeRadius || GetShortestDistanceToLineSegment(target, start, pos2) <= targetSizeRadius;
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
        public static bool CheckInSectorRangeOfDirection(Vector3 start, Vector3 target, float radius,
            float angle, float direction = 0) {
            Vector3 dirBase = target - start;
            Vector3 forward = Quaternion.Euler(0, direction, 0) * Vector3.forward;
            float curAngle = Vector3.Angle(forward, dirBase.normalized);

            float curDis = Vector3.Distance(start, target);
            if (curDis <= radius) {
                if (curAngle <= angle * 0.5f) {
                    return true;
                } else {
                    Vector3 pos1 = GetPosByDirAndDis(start, direction - angle * 0.5f, radius);
                    Vector3 pos2 = GetPosByDirAndDis(start, direction + angle * 0.5f, radius);
                    return GetShortestDistanceToLineSegment(target, start, pos1) <= 0 || GetShortestDistanceToLineSegment(target, start, pos1) <= 0;
                }
            }

            return false;
        }

        /// <summary>
        /// 判断目标点是否在矩形范围内（包含目标体积）
        /// </summary>
        /// <param name="start">起始点</param>
        /// <param name="target">目标点</param>
        /// <param name="targetSizeRadius">目标体积</param>
        /// <param name="direction">矩形方向</param>
        /// <param name="length">长</param>
        /// <param name="width">宽</param>
        /// <param name="rectRangType">矩形范围类型</param>
        /// <param name="rectCenterRangType">矩形中心类型</param>
        /// <returns></returns>
        public static bool CheckTargetInRectangleWithBodySize(Vector3 start, Vector3 target, float targetSizeRadius, float direction, float length, float width, RectCenterRangType rectCenterRangType = RectCenterRangType.Center) {
            start = GetRectangleCenterPoint(start, direction, length, rectCenterRangType);
            width = width * 0.5f;
            float targetToStartDis = GetStartCenterToTargetDisWithBodySize(start, target, targetSizeRadius);
            
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

            Vector3 forward = Quaternion.Euler(0, direction, 0) * Vector3.forward;
            Vector3 right = Quaternion.Euler(0, direction, 0) * Vector3.right;
            Vector3 dir = target - start;
            float lengthDic = Vector3.Dot(dir, forward);

            if (!isPassMaxDis) {
                float rightDistance = Vector3.Dot(dir, right);
                if (0 <= rightDistance) {
                    rightDistance = Mathf.Min(Mathf.Abs(rightDistance),
                        Mathf.Abs(rightDistance - targetSizeRadius));
                } else {
                    rightDistance = Mathf.Min(Mathf.Abs(rightDistance),
                        Mathf.Abs(rightDistance + targetSizeRadius));
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
        public static bool CheckTargetInRectangle(Vector3 start, Vector3 target, float direction, float length, float width, RectCenterRangType rectCenterRangType = RectCenterRangType.Center) {
            start = GetRectangleCenterPoint(start, direction, length, rectCenterRangType);
            Vector3 forward = Quaternion.Euler(0, direction, 0) * Vector3.forward;
            Vector3 right = Quaternion.Euler(0, direction, 0) * Vector3.right;
            width = width * 0.5f;

            Vector3 dir = target - start;
            float lengthDic = Vector3.Dot(dir, forward);

            if (Mathf.Abs(lengthDic) <= length) {
                float rightDistance = Vector3.Dot(dir, right);
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
        public static Vector3 GetRectangleCenterPoint(Vector3 start, float direction, float length, RectCenterRangType rectCenterRangType) {
            if (rectCenterRangType == RectCenterRangType.Bottom) {
                return GetPosByDirAndDis(start, direction, length * 0.5f);
            }

            return start;
        }

    }
}
