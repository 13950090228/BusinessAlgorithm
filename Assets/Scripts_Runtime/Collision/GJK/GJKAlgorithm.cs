using UnityEngine;

namespace BusinessAlgorithm.Collision {

    public static class GJKAlgorithm {
        /// <summary>
        /// GJK 算法入口
        /// </summary>
        /// <param name="polygonA">多边形A顶点数组（需要按顺序）</param>
        /// <param name="polygonB">多边形B顶点数组（需要按顺序）</param>
        /// <returns></returns>
        public static bool GJK(Vector2[] polygonA, Vector2[] polygonB) {

            // 初始化单纯形
            Simplex simplex = new Simplex();
            // 初始搜索方向
            simplex.SetDirection(Vector2.right);
            // 找到第一个闵可夫斯基差的支撑点,0阶单纯形
            Vector2 simplexPoint = Support(polygonA, polygonB, simplex.direction);
            simplex.SetPointB(simplexPoint);
            simplex.SetDirection(Vector2.zero - simplex.B);

            // 开始GJK迭代
            while (true) {
                // 如果点积不朝向原点，多边形不相交
                Vector2 supportA = Support(polygonA, polygonB, simplex.direction);
                if (Vector2.Dot(supportA, simplex.direction) < 0) {
                    return false;
                }

                simplex.AddPoint(supportA);

                // 返回 true，说明两个多边形相交
                bool isOrigin = ContainsOrigin(simplex);
                if (isOrigin) {
                    return true;
                }

            }
        }

        /// <summary>
        /// 计算两个多边形在给定方向上的支撑点
        /// </summary>
        /// <param name="polygonA"></param>
        /// <param name="polygonB"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Vector2 Support(Vector2[] polygonA, Vector2[] polygonB, Vector2 direction) {
            // 在给定方向上找到两个多边形的最远点
            Vector2 pointA = GetFarthestPointInDirection(polygonA, direction);
            Vector2 pointB = GetFarthestPointInDirection(polygonB, -direction);

            // 返回闵可夫斯基差集的支撑点
            return pointA - pointB;
        }

        /// <summary>
        /// 在给定方向上找到多边形的最远点
        /// </summary>
        /// <param name="polygon">图形</param>
        /// <param name="direction">方向</param>
        /// <returns></returns>
        public static Vector2 GetFarthestPointInDirection(Vector2[] polygon, Vector2 direction) {
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

        /// <summary>
        /// 迭代主方法，三角形是否包含原点
        /// </summary>
        /// <returns></returns>
        public static bool ContainsOrigin(Simplex simplex) {

            if (simplex.GetLength() == 2) {
                return LineCore(simplex);
            }
            return TriangleCore(simplex);
        }

        /// <summary>
        /// 当单纯形是一条线时候的逻辑
        /// </summary>
        /// <param name="supportA"></param>
        /// <param name="supportB"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static bool LineCore(Simplex simplex) {
            Vector2 dirAB = simplex.B - simplex.A;
            Vector2 dirAO = Vector2.zero - simplex.A;
            simplex.SetDirection(TripleCross(dirAB, dirAO, dirAB));

            return false;
        }

        /// <summary>
        /// 当单纯形是一个三角形时候的逻辑
        /// </summary>
        /// <param name="supportA"></param>
        /// <param name="supportB"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static bool TriangleCore(Simplex simplex) {
            Vector2 dirAB = simplex.B - simplex.A;
            Vector2 dirAC = simplex.C - simplex.A;
            Vector2 dirAO = Vector2.zero - simplex.A;

            Vector2 tripleAB = TripleCross(dirAC, dirAB, dirAB).normalized;
            Vector2 tripleAC = TripleCross(dirAB, dirAC, dirAC).normalized;

            Debug.Log("[lyq]tripleAB:" + tripleAB);
            Debug.Log("[lyq]tripleAC:" + tripleAC);
            Debug.Log("[lyq]dirAB:" + dirAB);
            Debug.Log("[lyq]dirAC:" + dirAC);
            Debug.Log("[lyq]dirAO:" + dirAO);



            if (Vector2.Dot(tripleAB, dirAO) > 0) {
                simplex.RemovePointC();
                simplex.SetDirection(tripleAB);
                Debug.Log("[lyq]移除c");
                return false;
            } else if (Vector2.Dot(tripleAC, dirAO) > 0) {
                simplex.RemovePointB();
                simplex.SetDirection(tripleAC);
                Debug.Log("[lyq]移除b");
                return false;
            }

            return true;

        }

        /// <summary>
        /// 计算两个方向向量的叉积值
        /// </summary>
        static float CrossValue(Vector2 a, Vector2 b) {
            return Vector3.Cross(a, b).z;
        }

        /// <summary>
        /// 计算矢量三重积
        /// </summary>
        static Vector2 TripleCross(Vector2 a, Vector2 b, Vector2 c) {
            return Cross(Cross(a, b), c);
        }

        /// <summary>
        /// 计算叉积
        /// </summary>
        static Vector2 Cross(Vector2 a, Vector2 b) {
            return new Vector2(0, a.x * b.y - a.y * b.x);
        }

    }


}
