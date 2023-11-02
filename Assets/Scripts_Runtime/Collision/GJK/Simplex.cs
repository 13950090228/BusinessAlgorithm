using UnityEngine;
using System;

namespace BusinessAlgorithm.Collision {
    public class Simplex {
        public Vector2 direction;
        public int indexA;
        public int indexB;

        private Vector2[] points = new Vector2[3];
        private int count = 0;

        public void AddSupportPoint(Vector2 support, Vector2 pointA, Vector2 pointB) {
            points[count] = support;
            indexA = count;
            indexB = count;

            for (int i = 0; i < count; i++) {
                if (Vector2.Dot(support - points[i], support) > 0) {
                    indexA = count;
                    indexB = i;
                    direction = TripleCross(points[i] - support, support, support);
                    break;
                }
            }

            count++;
        }

        public bool ContainsOrigin(ref Vector2 newDirection) {
            Vector2 a = points[indexA];
            Vector2 b = points[indexB];
            Vector2 ao = -a;
            Vector2 ab = b - a;

            if (count == 3) {
                Vector2 c = points[3 - indexA - indexB];
                Vector2 ac = c - a;
                Vector2 bc = c - b;
                Vector2 tripleCross = TripleCross(ab, ac, ao);

                if (Vector2.Dot(tripleCross, bc) > 0) {
                    indexA = indexB;
                    indexB = 3 - indexA - indexB;
                    direction = tripleCross;
                    return false;
                }
            }

            direction = DoubleCross(ab, ao);

            return Vector2.Dot(direction, ao) <= 0;
        }

        private Vector2 TripleCross(Vector2 a, Vector2 b, Vector2 c) {
            return Cross(Cross(a, b), c);
        }

        private Vector2 DoubleCross(Vector2 a, Vector2 b) {
            return Cross(Cross(a, b), a);
        }

        private Vector2 Cross(Vector2 a, Vector2 b) {
            return new Vector2(0, a.x * b.y - a.y * b.x);
        }
    }
}
