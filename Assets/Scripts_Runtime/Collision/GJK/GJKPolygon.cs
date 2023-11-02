using System;
using UnityEngine;

namespace BusinessAlgorithm.Collision {
    [Serializable]
    public class GJKPolygon {
        public string sign;
        public Color color;
        public Vector2[] points;
        public bool isSearchOther;

        public Vector2 Center {
            get {
                Vector2 sum = Vector2.zero;
                foreach (var vertex in points) {
                    sum += vertex;
                }
                return sum / points.Length;
            }
        }

        public Vector2 SupportFunction(Vector2 direction) {
            float maxDot = float.MinValue;
            Vector2 supportVertex = Vector2.zero;

            foreach (var vertex in points) {
                float dot = Vector2.Dot(vertex, direction);
                if (dot > maxDot) {
                    maxDot = dot;
                    supportVertex = vertex;
                }
            }

            return supportVertex;
        }
    }
}
