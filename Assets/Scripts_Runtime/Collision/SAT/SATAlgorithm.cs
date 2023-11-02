using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BusinessAlgorithm.Collision {
    public static class SATAlgorithm {

        /// <summary>
        /// 计算多边形在给定轴上的投影
        /// </summary>
        public static void ProjectOntoAxis(Vector2[] points, Vector2 axis, out float min, out float max) {
            float dotProduct = axis.x * points[0].x + axis.y * points[0].y;
            min = dotProduct;
            max = dotProduct;

            for (int i = 1; i < points.Length; i++) {
                dotProduct = axis.x * points[i].x + axis.y * points[i].y;
                if (dotProduct < min) {
                    min = dotProduct;
                } else if (dotProduct > max) {
                    max = dotProduct;
                }
            }
        }

        /// <summary>
        /// 根据两组多边形定点判断是否相交
        /// </summary>
        public static bool IsIntersecting(Vector2[] curPoints, Vector2[] otherPoints) {
            for (int i = 0; i < curPoints.Length; i++) {
                int nextIndex = (i + 1) % curPoints.Length;
                Vector2 edge = new Vector2() { x = curPoints[nextIndex].x - curPoints[i].x, y = curPoints[nextIndex].y - curPoints[i].y };

                Vector2 normal = new Vector2() { x = -edge.y, y = edge.x };
                float minA, maxA;
                ProjectOntoAxis(curPoints, normal, out minA, out maxA);

                float minB, maxB;
                ProjectOntoAxis(otherPoints, normal, out minB, out maxB);

                if (maxA < minB || maxB < minA) {
                    return false;
                }
            }

            return true;
        }
    }
}
