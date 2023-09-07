using UnityEngine;
using BusinessAlgorithm.BaseAction;

namespace BusinessAlgorithm.DrawGraph {
    public class DrawSector : DrawGraphBase {
        public Vector3 start;               // 扇形圆心
        public int segments = 50;           // 扇形的线段数
        public float radius;                // 扇形的半径
        public float angle;                 // 扇形的夹角
        public float direction;             // 扇形中轴线角度


        public override void Draw() {
            int numSegments = Mathf.Max(segments, 3); // 至少需要3个顶点来绘制三角形

            // 将角度转换为弧度
            float angleRad = Mathf.Deg2Rad * angle;

            lineRenderer.positionCount = numSegments + 2;
            Vector3[] positions = new Vector3[numSegments + 2];

            // 计算扇形的中心点
            Vector3 center = start;

            // 计算每个顶点的位置
            for (int i = 0; i <= numSegments; i++) {
                float currentAngle = direction + (angleRad / numSegments) * i;
                float x = center.x + Mathf.Cos(currentAngle) * radius;
                float z = center.z + Mathf.Sin(currentAngle) * radius;
                positions[i] = new Vector3(x, center.y, z);
            }

            lineRenderer.SetPositions(positions);
        }

        public override void Reset() {
            lineRenderer.positionCount = 0;
        }
    }
}
