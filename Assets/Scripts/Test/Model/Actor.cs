using UnityEngine;
using System;

namespace BusinessAlgorithm.Test {
    [Serializable]
    public struct Actor {
        public Vector3 pos;
        public float radius;
    }

    [Serializable]
    public struct Actor2D {
        public Vector2 pos;
        public float radius;
    }
}
