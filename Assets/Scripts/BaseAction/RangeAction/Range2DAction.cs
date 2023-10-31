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
        /// <param name="targetSizeRadius">目标体积</param>
        /// <returns></returns>
        public static float GetStartCenterToTargetDisWithBodySize(Vector2 start, Vector2 target, float targetSizeRadius) {
            float dis = Vector2.Distance(start, target);
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
        public static float GetStartToTargetDisWithBodySize(Vector2 start, float startBodySize, Vector2 target, float targetSizeRadius) {
            float dis = Vector2.Distance(start, target);
            return dis - (startBodySize + targetSizeRadius);
        }

        /// <summary>
        /// 判断起始点和目标点的距离是否在指定范围内（包含目标体积）
        /// </summary>
        public static bool CheckTargetInRangeWithBodySize(Vector2 start, Vector2 target, float targetSizeRadius, float radius) {

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
        /// <param name="targetSizeRadius">目标体积</param>
        /// <param name="radius">扇形半径</param>
        /// <param name="angle">扇形角度</param>
        /// <param name="direction">扇形方向（顺时针）</param>
        /// <returns></returns>
        public static bool CheckInSectorRangeOfDirectionWithBodySize(Vector2 start, Vector2 target, float targetSizeRadius, float radius, float angle, float direction) {
            Vector2 dirBase = target - start;
            Vector2 forward = Quaternion.Euler(0, 0, direction) * Vector2.up;
            float curDis = GetStartCenterToTargetDisWithBodySize(start, target, targetSizeRadius);
            bool includedAngle = false; ;
            if (curDis <= radius) {
                float relativeAngle = CalculateClockwiseAngle(start, target);
                includedAngle = IsAngleBetween(relativeAngle, direction - angle * 0.5f, direction + angle * 0.5f);
                if (includedAngle) {
                    return true;
                } else {
                    // 求出扇形的两个点
                    Vector2 pos1 = GetPosByDirAndDis(start, direction - angle * 0.5f, radius);
                    Vector2 pos2 = GetPosByDirAndDis(start, direction + angle * 0.5f, radius);
                    // 求出圆形是否与扇形的两条边相交
                    bool condition1 = IsCircleIntersectingLine(target, targetSizeRadius, pos1, start);
                    bool condition2 = IsCircleIntersectingLine(target, targetSizeRadius, pos2, start);
                    return condition1 || condition2;
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
        public static bool CheckInSectorRangeOfDirection(Vector2 start, Vector2 target, float radius, float angle, float direction) {
            Vector2 dirBase = target - start;
            Vector2 forward = Quaternion.Euler(0, 0, direction) * Vector2.up;
            float relativeAngle = CalculateClockwiseAngle(start, target);
            float curDis = Vector2.Distance(start, target);
            if (curDis <= radius) {
                if (relativeAngle <= direction + angle * 0.5f && relativeAngle >= direction - angle * 0.5f) {
                    return true;
                } else {
                    Vector2 pos1 = GetPosByDirAndDis(start, direction - angle * 0.5f, radius);
                    Vector2 pos2 = GetPosByDirAndDis(start, direction + angle * 0.5f, radius);
                    float vertical1 = GetShortestDistanceToLineSegment(target, start, pos1);
                    float vertical2 = GetShortestDistanceToLineSegment(target, start, pos2);
                    return vertical1 <= 0 || vertical2 <= 0;
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
        // 判断目标点是否在矩形范围内（包含目标体积）
        public static bool CheckTargetInRectangleWithBodySize(Vector2 start, Vector2 target, float targetSizeRadius, float direction, float length, float width, RectCenterRangType rectCenterRangType = RectCenterRangType.Center) {
            start = GetRectangleCenterPoint(start, direction, length, rectCenterRangType);

            // 对比对角线和目标点的距离长度
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

            Vector2 forward = Quaternion.Euler(0, 0, direction) * Vector2.up;
            Vector2 right = Quaternion.Euler(0, 0, direction) * Vector2.right;

            Vector2 dir = target - start;
            float lengthDic = Vector2.Dot(dir, forward);

            if (!isPassMaxDis) {
                float rightDistance = Vector2.Dot(dir, right);
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
        /// 让点以某一点为圆心旋转指定角度(顺时针)
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

        /// <summary>
        /// 计算点B在点A的哪个角度（顺时针）
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static float CalculateClockwiseAngle(Vector2 A, Vector2 B) {
            // 计算从向量A到向量B的差向量
            Vector2 difference = B - A;

            // 使用Mathf.Atan2来计算逆时针方向的角度（弧度）
            float angleRadians = Mathf.Atan2(difference.x, difference.y);

            // 将弧度转化为角度（顺时针方向）
            float angleDegrees = angleRadians * Mathf.Rad2Deg;

            if (angleDegrees < 0) {
                angleDegrees += 360f;
            }

            return angleDegrees;
        }

        /// <summary>
        /// 判断一个圆与一条线段是否相交
        /// </summary>
        /// <param name="circlePos">圆心</param>
        /// <param name="radius">半径</param>
        /// <param name="lineStart">线段起始点</param>
        /// <param name="lineEnd">线段终点</param>
        /// <returns></returns>
        public static bool IsCircleIntersectingLine(Vector2 circlePos, float radius, Vector2 lineStart, Vector2 lineEnd) {
            // 计算线段的向量
            Vector2 lineVector = lineEnd - lineStart;

            // 计算线段的长度的平方
            float lineLengthSquared = lineVector.sqrMagnitude;

            // 计算从圆心到线段起点的向量
            Vector2 circleToLineStart = lineStart - circlePos;

            // 计算圆心到线段的最短距离
            float closestDistance = Vector2.Dot(circleToLineStart, lineVector) / lineLengthSquared;

            // 计算最短距离上的点
            Vector2 closestPoint = lineStart + closestDistance * lineVector;

            // 计算圆心到最短距离点的距离的平方
            float distanceToClosestPointSquared = (circlePos - closestPoint).sqrMagnitude;

            // 如果圆心到最短距离点的距离小于圆的半径的平方，相交
            return distanceToClosestPointSquared < (radius * radius);
        }

        /// <summary>
        /// 判断一个角度是否处于两个角度之间
        /// </summary>
        /// <param name="targetAngle">目标角度</param>
        /// <param name="startAngle">起始角度</param>
        /// <param name="endAngle">结束角度</param>
        /// <returns></returns>
        public static bool IsAngleBetween(float targetAngle, float startAngle, float endAngle) {
            // 将所有角度限制在0到360度之间
            if (targetAngle < 0) {
                targetAngle += 360;
            }

            if (startAngle < 0 || endAngle < 0) {
                startAngle += 360;
                endAngle += 360;
            }

            if (startAngle < endAngle) {
                // 如果起始角度小于结束角度，则检查角度是否在两者之间
                return targetAngle >= startAngle && targetAngle <= endAngle;
            } else if (startAngle > endAngle) {
                // 如果起始角度大于结束角度，则检查角度是否在两者之外
                return targetAngle >= startAngle || targetAngle <= endAngle;
            } else {
                // 如果起始角度等于结束角度，则角度范围无效
                return false;
            }
        }
    }
}