using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BusinessAlgorithm.BaseAction {

    public class Range3DAction {

        /// <summary>
        /// 计算起始点与目标点的距离,包含目标点的体积
        /// </summary>
        /// <param name="start">起始点</param>
        /// <param name="target">目标点</param>
        /// <param name="targetBodySize">目标体积</param>
        /// <returns></returns>
        public float GetStartCenterToTargetDisWithBodySize(Vector3 start, Vector3 target, float targetBodySize) {
            float dis = Vector3.Distance(start, target);
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
        public float GetStartToTargetDisWithBodySize(Vector3 start, float startBodySize, Vector3 target, float targetBodySize) {
            float dis = Vector3.Distance(start, target);
            return dis - (startBodySize + targetBodySize);
        }

        /// <summary>
        /// 判断起始点和目标点的距离是否在指定范围内（包含目标体积）
        /// </summary>
        public bool CheckTargetInRange(Vector3 start, Vector3 target, float targetBodySize, float range) {

            float dis = GetStartCenterToTargetDisWithBodySize(start, target, targetBodySize);
            if (dis <= range) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 判断方向是否处于指定角度的扇形范围内
        /// </summary>
        /// <param name="checkedDir">被检查的方向</param>
        /// <param name="checkedDis">被检查的距离</param>
        /// <param name="range">扇形范围</param>
        /// <param name="angle">扇形角度</param>
        /// <param name="dir">扇形中轴角度（顺时针）</param>
        /// <returns></returns>
        public bool CheckInSectorRangeOfDirection(Vector3 checkedDir, float checkedDis,
           float range, float angle, float dir) {
            Vector3 forward = Quaternion.AngleAxis(dir, Vector3.up) * Vector3.right;
            checkedDir.y = 0;
            float curAngle = Vector3.Angle(forward, checkedDir.normalized);
            if (checkedDis < range && curAngle <= angle) {
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
        public Vector3 GetPosByDirAndDis(Vector3 start, float angle, float length) {
            Vector3 forward = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            return start + forward * length;
        }

        /// <summary>
        /// 判断目标点是否处于指定角度的扇形范围内，包含目标点体积
        /// </summary>
        /// <param name="start">起始点</param>
        /// <param name="target">目标点</param>
        /// <param name="targetBodySize">目标体积</param>
        /// <param name="range">扇形范围</param>
        /// <param name="angle">扇形角度</param>
        /// <param name="dir">扇形中轴线角度（顺时针）</param>
        /// <returns></returns>
        public bool CheckInSectorRangeOfDirectionWithBodySize(Vector3 start, Vector3 target, float targetBodySize, float range,
            float angle, float dir = 0) {
            Vector3 dirBase = target - start;
            Vector3 forward = Quaternion.Euler(0, dir, 0) * Vector3.forward;
            float curAngle = Vector3.Angle(forward, dirBase.normalized);


            float curDis = GetStartCenterToTargetDisWithBodySize(start, target, targetBodySize);
            if (curDis <= range) {
                if (curAngle <= angle) {
                    return true;
                } else {
                    Vector3 pos1 = GetPosByDirAndDis(start, dir - angle, range);
                    Vector3 pos2 = GetPosByDirAndDis(start, dir + angle, range);
                    return GetStartCenterToTargetDisWithBodySize(pos1, target, targetBodySize) <= 0 || GetStartCenterToTargetDisWithBodySize(pos2, target, targetBodySize) <= 0;
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
        /// <param name="angle">角度</param>
        /// <param name="length">长</param>
        /// <param name="width">宽</param>
        /// <param name="rectRangType">矩形范围类型</param>
        /// <param name="rectCenterRangType">矩形中心类型</param>
        /// <returns></returns>
        public bool CheckTargetInRectWithBodySize(Vector3 start, Vector3 target, float targetBodySize, float angle, float length, float width, RectRangType rectRangType, RectCenterRangType rectCenterRangType) {
            Vector3 forward = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            Vector3 right = Quaternion.Euler(0, angle, 0) * Vector3.right;
            width = width * 0.5f;

            Vector3 dir = target - start;
            float lengthDic = Vector3.Dot(dir, forward);
            float forwardDistance = Mathf.Abs(lengthDic) - targetBodySize;
            bool isCanFind = true;
            if (rectCenterRangType == RectCenterRangType.bottom) {
                isCanFind = lengthDic > 0;
            }

            if (isCanFind && forwardDistance <= length) {
                float rightDistance = Vector3.Dot(dir, right);
                if (0 <= rightDistance) {
                    rightDistance = Mathf.Min(Mathf.Abs(rightDistance),
                        Mathf.Abs(rightDistance - targetBodySize));
                } else {
                    rightDistance = Mathf.Min(Mathf.Abs(rightDistance),
                        Mathf.Abs(rightDistance + targetBodySize));
                }

                if (rectRangType == RectRangType.Right) {
                    if (0 <= rightDistance && rightDistance <= width) {
                        return true;
                    }
                } else if (rectRangType == RectRangType.Left) {
                    if (rightDistance <= 0 && Mathf.Abs(rightDistance) <= width) {
                        return true;
                    }
                } else if (Mathf.Abs(rightDistance) <= width) {
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
        /// <param name="angle">角度</param>
        /// <param name="length">长</param>
        /// <param name="width">宽</param>
        /// <param name="rectRangType">矩形范围类型</param>
        /// <param name="rectCenterRangType">矩形中心类型</param>
        /// <returns></returns>
        public bool CheckTargetInRect(Vector3 start, Vector3 target, float angle, float length, float width, RectRangType rectRangType, RectCenterRangType rectCenterRangType) {
            Vector3 forward = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            Vector3 right = Quaternion.Euler(0, angle, 0) * Vector3.right;
            width = width * 0.5f;

            Vector3 dir = target - start;
            float lengthDic = Vector3.Dot(dir, forward);

            bool isCanFind = true;
            if (rectCenterRangType == RectCenterRangType.bottom) {
                isCanFind = lengthDic > 0;
            }

            if (isCanFind && Mathf.Abs(lengthDic) <= length) {
                float rightDistance = Vector3.Dot(dir, right);

                if (rectRangType == RectRangType.Right) {
                    if (0 <= rightDistance && rightDistance <= width) {
                        return true;
                    }
                } else if (rectRangType == RectRangType.Left) {
                    if (rightDistance <= 0 && Mathf.Abs(rightDistance) <= width) {
                        return true;
                    }
                } else if (Mathf.Abs(rightDistance) <= width) {
                    return true;
                }
            }

            return false;
        }

    }
}
