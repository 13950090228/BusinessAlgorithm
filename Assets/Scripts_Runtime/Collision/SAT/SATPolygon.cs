using UnityEngine;
using System;

namespace BusinessAlgorithm.Collision {
    [Serializable]
    public struct SATPolygon {
        public string sign;
        public Vector2[] points;
        public Color color;
        public bool isSearchOther;
    }
}
