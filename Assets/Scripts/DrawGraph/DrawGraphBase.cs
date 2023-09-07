using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BusinessAlgorithm.DrawGraph {
    [RequireComponent(typeof(LineRenderer))]
    public class DrawGraphBase : MonoBehaviour {
        public LineRenderer lineRenderer;
        public Material material;
        public virtual void Draw() {

        }

        public virtual void Reset() {

        }
    }
}