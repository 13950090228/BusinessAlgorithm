using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BusinessAlgorithm.Collision;

namespace BusinessAlgorithm.Test {
    public class GJKTest : MonoBehaviour {
        public List<GJKPolygon> polygons;
        public List<Vector2> simplexs;

        void Update() {
            for (int i = 0; i < polygons.Count; i++) {
                RenderLine(polygons[i].points, polygons[i].color);
            }

            for (int i = 0; i < simplexs.Count; i++) {
                RenderLine(simplexs.ToArray(), Color.white);
            }

            RenderLine(new Vector2[] { new Vector2(10, 0), new Vector2(-10, 0) }, Color.black);
            RenderLine(new Vector2[] { new Vector2(0, 10), new Vector2(0, -10) }, Color.black);


            // 按下空格出发测试
            if (Input.GetKeyDown(KeyCode.Space)) {
                Test();
            }
        }

        void RenderLine(Vector2[] vector2s, Color color) {
            for (int i = 0; i < vector2s.Length; i++) {
                if (i + 1 == vector2s.Length) {
                    Debug.DrawLine(vector2s[i], vector2s[0], color);
                } else {
                    Debug.DrawLine(vector2s[i], vector2s[i + 1], color);
                }
            }
        }


        void Test() {
            // 检测两个多边形是否相交
            bool IsIntersecting = GJKAlgorithm.GJK(polygons[0].points, polygons[1].points);
            Debug.Log("[lyq]是否相交：" + IsIntersecting);
        }
    }
}
