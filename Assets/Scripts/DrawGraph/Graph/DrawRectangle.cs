using UnityEngine;
using BusinessAlgorithm.BaseAction;

namespace BusinessAlgorithm.DrawGraph {
    public class DrawRectangle : DrawGraphBase {
        public Vector3 start;                       // 矩形的起始点
        public float direction;                         // 矩形的方向
        public float length;                        // 矩形的长度
        public float width;                         // 矩形的宽度
        public RectCenterRangType centerRangType;   // 矩形中心点


        public override void InitLineRenderer() {
            lineRenderer.positionCount = 5;
            lineRenderer.useWorldSpace = true;
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.material = material;
        }

        public override void InitArgs(DrawGraphArgs args) {
            this.start = args.pos;
            this.direction = args.direction;
            this.length = args.length;
            this.width = args.width;
            this.centerRangType = args.centerRangType;
            this.DrawGraphType = args.drawGraphType;
        }

        public override void Draw() {

            Reset();

            InitLineRenderer();

            // 计算矩形的四个顶点
            Vector3 halfTopLeft = new Vector3(-width * 0.5f, 0, -length * 0.5f);
            Vector3 halfTopRight = new Vector3(width * 0.5f, 0, -length * 0.5f);
            Vector3 halfBottomLeft = new Vector3(-width * 0.5f, 0, length * 0.5f);
            Vector3 halfBottomRight = new Vector3(width * 0.5f, 0, length * 0.5f);

            // 根据矩形中心点类型调整矩形的位置
            Vector3 centerOffset = Vector3.zero;

            if (centerRangType == RectCenterRangType.Bottom) {
                centerOffset = new Vector3(0, 0, length * 0.5f);
            }

            transform.position = start;

            // 将四个顶点平移
            Vector3 topLeft = halfTopLeft + centerOffset + start;
            Vector3 topRight = halfTopRight + centerOffset + start;
            Vector3 bottomLeft = halfBottomLeft + centerOffset + start;
            Vector3 bottomRight = halfBottomRight + centerOffset + start;

            // 四个点根据中心点旋转
            topLeft = RotatePointAroundPivot(topLeft, start, new Vector3(0, direction, 0));
            topRight = RotatePointAroundPivot(topRight, start, new Vector3(0, direction, 0));
            bottomLeft = RotatePointAroundPivot(bottomLeft, start, new Vector3(0, direction, 0));
            bottomRight = RotatePointAroundPivot(bottomRight, start, new Vector3(0, direction, 0));

            Vector3[] posArray = new Vector3[5]{
                topLeft,
                topRight,
                bottomRight,
                bottomLeft,
                topLeft
            };

            lineRenderer.SetPositions(posArray);
        }

        private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
            Vector3 direction = point - pivot;
            direction = Quaternion.Euler(angles) * direction;
            return direction + pivot;
        }

    }
}



