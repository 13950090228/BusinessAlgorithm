using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BusinessAlgorithm.DrawGraph {
    public class DrawCircle : DrawGraphBase {
        public Vector3 start;    // 圆心
        public float radius;     // 半径
        int segments = 50; // 圆形的线段数

        public override void InitLineRenderer() {
            int numSegments = Mathf.Max(segments, 3); // 至少需要3个顶点来绘制三角形
            lineRenderer.useWorldSpace = true;
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.material = material;
            lineRenderer.positionCount = numSegments + 1;
        }

        public override void InitCircleArgs(Vector3 start, float radius) {
            this.start = start;
            this.radius = radius;
            Draw();
        }

        public override void Draw() {
            this.transform.position = start;
            int numSegments = Mathf.Max(segments, 3); // 至少需要3个顶点来绘制三角形

            InitLineRenderer();

            Vector3[] positions = new Vector3[numSegments + 1];

            // 计算圆形的每个顶点的位置
            for (int i = 0; i <= numSegments; i++) {
                float angle = 2f * Mathf.PI * i / numSegments;
                float x = start.x + radius * Mathf.Cos(angle);
                float y = start.y;
                float z = start.z + radius * Mathf.Sin(angle);
                positions[i] = new Vector3(x, y, z);
            }

            lineRenderer.SetPositions(positions);
        }
    }
}