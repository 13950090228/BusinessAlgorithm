using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BusinessAlgorithm.Collision;

namespace BusinessAlgorithm.Test {
    public class SATTest : MonoBehaviour {
        public List<SATPolygon> polygons;

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
            for (int i = 0; i < polygons.Count; i++) {
                if (polygons[i].isSearchOther) {
                    for (int j = i + 1; j < polygons.Count; j++) {
                        bool IsIntersecting = SATAlgorithm.IsIntersecting(polygons[i].points, polygons[j].points);
                        if (IsIntersecting) {
                            Debug.Log($"{polygons[i].sign} 和 {polygons[j].sign} 相交");
                        }
                    }
                }
            }
        }
    }
}
