using BusinessAlgorithm.BaseAction;
using UnityEngine;

namespace BusinessAlgorithm.DrawGraph {
    [RequireComponent(typeof(LineRenderer))]
    public class DrawGraphBase : MonoBehaviour {
        public LineRenderer lineRenderer;
        public Material material;

        public virtual void InitLineRenderer() {
        }

        /// <summary>
        /// 初始化扇形参数
        /// </summary>
        public virtual void InitSectorArgs(Vector3 start, float radius, float angle, float direction) {

        }

        /// <summary>
        /// 初始化圆形参数
        /// </summary>
        public virtual void InitCircleArgs(Vector3 start, float radius) {

        }

        /// <summary>
        /// 初始化矩形参数
        /// </summary>
        public virtual void InitRectangleAgrs(Vector3 start, float angle, float length, float width, RectCenterRangType centerRangType) {

        }

        public virtual void Draw() {
        }

        public virtual void Reset() {
            lineRenderer.positionCount = 0;
        }
    }
}