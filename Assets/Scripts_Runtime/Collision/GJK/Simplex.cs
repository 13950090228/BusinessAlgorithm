using UnityEngine;
using System;

namespace BusinessAlgorithm.Collision {
    /// <summary>
    /// 二阶单纯型
    /// </summary>
    public class Simplex {
        public Vector2 direction { get; private set; }
        public Vector2 A { get; private set; }
        public Vector2 B { get; private set; }
        public Vector2 C { get; private set; }
        int a;
        int b;
        int c;

        public void SetDirection(Vector2 dir) {
            this.direction = dir;
        }

        public void SetPointA(Vector2 point) {
            this.A = point;
            a = 1;
        }

        public void SetPointB(Vector2 point) {
            this.B = point;
            b = 1;
        }

        public void SetPointC(Vector2 point) {
            this.C = point;
            c = 1;
        }

        public void AddPoint(Vector2 supportPoint) {
            if (a == 0) {
                SetPointA(supportPoint);
            } else if (b == 0) {
                SetPointB(supportPoint);
            } else {
                SetPointC(supportPoint);
            }
        }

        public void RemovePointA() {
            a = 0; ;
        }

        public void RemovePointB() {
            b = 0; ;
        }

        public void RemovePointC() {
            c = 0; ;
        }

        public int GetLength() {
            return a + b + c;
        }
    }
}
