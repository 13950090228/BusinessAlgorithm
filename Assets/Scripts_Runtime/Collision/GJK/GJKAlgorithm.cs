using UnityEngine;

namespace BusinessAlgorithm.Collision {
    public static class GJKAlgorithm {

        // 使用GJK算法检测两个多边形是否相交
        public static bool ArePolygonsIntersecting(Vector2[] polygonA, Vector2[] polygonB) {
            Vector2 initialDirection = Vector2.right; // 初始搜索方向

            // 使用单纯形（Simplex）以初始搜索方向初始化
            Simplex simplex = new Simplex();
            simplex.AddSupportPoint(polygonA[0] - polygonB[0], polygonA[0], polygonB[0]);

            // 开始GJK迭代
            while (true) {
                Vector2 support = Support(polygonA, polygonB, simplex.direction);
                float dot = Vector2.Dot(support, simplex.direction);

                // 如果支持点不朝向原点，多边形不相交
                if (dot < 0) {
                    return false;
                }

                simplex.AddSupportPoint(support, polygonA[simplex.indexA], polygonB[simplex.indexB]);
                if (simplex.ContainsOrigin(ref simplex.direction)) {
                    return true;
                }
            }
        }

        // 计算两个多边形在给定方向上的支持点
        private static Vector2 Support(Vector2[] polygonA, Vector2[] polygonB, Vector2 direction) {
            // 在给定方向上找到两个多边形的最远点
            Vector2 pointA = GetFarthestPointInDirection(polygonA, direction);
            Vector2 pointB = GetFarthestPointInDirection(polygonB, -direction);

            // 返回闵可夫斯基差集的支持点
            return pointA - pointB;
        }

        // 在给定方向上找到多边形的最远点
        private static Vector2 GetFarthestPointInDirection(Vector2[] polygon, Vector2 direction) {
            float maxDot = float.MinValue;
            Vector2 farthestPoint = Vector2.zero;

            for (int i = 0; i < polygon.Length; i++) {
                float dot = Vector2.Dot(polygon[i], direction);
                if (dot > maxDot) {
                    maxDot = dot;
                    farthestPoint = polygon[i];
                }
            }

            return farthestPoint;
        }
    }
}
