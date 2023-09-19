using UnityEngine;
using BusinessAlgorithm.BaseAction;

namespace BusinessAlgorithm.DrawGraph {
    public class DrawSector : DrawGraphBase {
        public Vector3 start;               // 扇形圆心
        public float radius;                // 扇形的半径
        public float angle;                 // 扇形的夹角
        public float direction;             // 扇形中轴线角度
        int segments = 50;                  // 扇形的线段数

        public override void InitLineRenderer() {
            lineRenderer.useWorldSpace = true;
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.material = material;
            lineRenderer.positionCount = segments + 3;
        }

        public override void InitArgs(DrawGraphArgs args) {
            this.start = args.pos;
            this.radius = args.radius;
            this.angle = args.angle;
            this.direction = args.direction;
            this.DrawGraphType = args.drawGraphType;
        }

        public override void Draw3D() {

            InitLineRenderer();

            // 根据 direction 调整起始角度
            float startAngle = -angle / 2.0f - direction + 90;
            float step = angle / segments;
            Vector3 onePoint = Vector3.zero;
            for (int i = segments; i >= 0; i--) {
                float currentAngle = startAngle + step * i;
                float x = start.x + Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius;
                float z = start.z + Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius;
                Vector3 point = new Vector3(x, start.y, z);
                lineRenderer.SetPosition(i, point);
                if (i == 0) {
                    onePoint = point;
                }
            }

            // 添加扇形的中心点作为最后一个顶点，以封闭扇形
            lineRenderer.SetPosition(segments + 1, start);

            lineRenderer.SetPosition(segments + 2, onePoint);
        }

        public override void Draw2D() {
            InitLineRenderer();

            // 根据 direction 调整起始角度
            float startAngle = -angle / 2.0f - direction + 90;
            float step = angle / segments;
            Vector3 onePoint = Vector3.zero;
            for (int i = segments; i >= 0; i--) {
                float currentAngle = startAngle + step * i;
                float x = start.x + Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius;
                float y = start.y + Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius;
                Vector3 point = new Vector3(x, y, start.z);
                lineRenderer.SetPosition(i, point);
                if (i == 0) {
                    onePoint = point;
                }
            }

            // 添加扇形的中心点作为最后一个顶点，以封闭扇形
            lineRenderer.SetPosition(segments + 1, start);

            lineRenderer.SetPosition(segments + 2, onePoint);
        }
    }
}
