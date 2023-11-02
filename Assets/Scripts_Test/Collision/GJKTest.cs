using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BusinessAlgorithm.Collision;

namespace BusinessAlgorithm.Test {
    public class GJKTest : MonoBehaviour {
        public List<GJKPolygon> polygons;

        void Update() {
            for (int i = 0; i < polygons.Count; i++) {
                RenderLine(polygons[i].points, polygons[i].color);
            }

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
            bool IsIntersecting = GJKAlgorithm.ArePolygonsIntersecting(polygons[0].points, polygons[1].points);
            Debug.Log("是否相交：" + IsIntersecting);
            // for (int i = 0; i < polygons.Count; i++) {
            //     if (polygons[i].isSearchOther) {
            //         for (int j = i + 1; j < polygons.Count; j++) {
            //             bool IsIntersecting = GJKAlgorithm.GJKIntersect(polygons[i], polygons[j]);
            //             if (IsIntersecting) {
            //                 Debug.Log($"{polygons[i].sign} 和 {polygons[j].sign} 相交");
            //             }
            //         }
            //     }
            // }
        }
    }
}
