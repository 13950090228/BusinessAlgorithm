using BusinessAlgorithm.BaseAction;
using UnityEngine;
using System;
namespace BusinessAlgorithm.DrawGraph {
    public enum DrawGraphType {
        Circle,
        Sector,
        Rectangle,
    }
    
    [Serializable]
    public struct DrawGraphArgs{
        public DrawGraphType drawGraphType;
        public Vector3 pos;
        public float radius;
        public float angle;
        public float direction;
        public float length;
        public float width;
        public RectCenterRangType centerRangType;
    }

    [RequireComponent(typeof(LineRenderer))]
    public class DrawGraphBase : MonoBehaviour {
        public LineRenderer lineRenderer;
        public Material material;
        public DrawGraphType DrawGraphType { get; protected set; }

        public virtual void InitLineRenderer() {
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        public virtual void InitArgs(DrawGraphArgs args) {

        }

        public virtual void Draw() {
        }

        public virtual void Reset() {
            lineRenderer.positionCount = 0;
        }
    }
}