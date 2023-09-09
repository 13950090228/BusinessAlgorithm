using BusinessAlgorithm.BaseAction;
using BusinessAlgorithm.DrawGraph;
using UnityEngine;
using System.Collections.Generic;

namespace BusinessAlgorithm.Test {
    public class TestRange2D : MonoBehaviour {
        public DrawRectangle drawRectangle;
        public DrawCircle drawCircle;
        public DrawSector drawSector;

        public Actor2D caster;
        public Actor2D target;

        public DrawGraphArgs drawGraphArgs;

        public void Clear() {
            // 遍历父节点的所有子节点
            List<GameObject> goList = new List<GameObject>();
            foreach (Transform item in this.transform) {
                goList.Add(item.gameObject);
            }

            foreach (GameObject item in goList) {
                DestroyImmediate(item);
            }
        }

        public void CustomAction() {
            Clear();
            var result = Range2DAction.GetPosByDirAndDis(caster.pos, drawGraphArgs.angle, drawGraphArgs.radius);
            target.pos = result;
            DrawGraph();
            Debug.Log($"[lyq]计算结果:{result}");
        }

        // 计算起始点与目标点的距离,包含目标点的体积
        public void GetStartCenterToTargetDisWithBodySize() {
            Clear();
            var result = Range2DAction.GetStartCenterToTargetDisWithBodySize(caster.pos, target.pos, target.radius);

            DrawGraph();

            Debug.Log($"[lyq]计算结果:{result}");
        }

        // 计算起始点与目标点的距离,包含双方的体积
        public void GetStartToTargetDisWithBodySize() {
            Clear();
            var result = Range2DAction.GetStartToTargetDisWithBodySize(caster.pos, caster.radius, target.pos, target.radius);

            DrawGraph();

            Debug.Log($"[lyq]计算结果:{result}");
        }

        // 判断起始点和目标点的距离是否在指定范围内（包含目标体积）
        public void CheckTargetInRange() {
            Clear();
            var result = Range2DAction.CheckTargetInRange(caster.pos, target.pos, target.radius, drawGraphArgs.radius);

            DrawGraph();

            Debug.Log($"[lyq]计算结果:{result}");
        }

        // 判断目标点是否处于指定角度的扇形范围内（包含目标点体积）
        public void CheckInSectorRangeOfDirectionWithBodySize() {
            Clear();
            var result = Range2DAction.CheckInSectorRangeOfDirectionWithBodySize(caster.pos, target.pos, target.radius, drawGraphArgs.radius, drawGraphArgs.angle, drawGraphArgs.direction);

            DrawGraph();

            Debug.Log($"[lyq]计算结果:{result}");
        }

        // 判断目标点是否处于指定角度的扇形范围内
        public void CheckInSectorRangeOfDirection() {
            Clear();
            var result = Range2DAction.CheckInSectorRangeOfDirection(caster.pos, target.pos, drawGraphArgs.radius, drawGraphArgs.angle, drawGraphArgs.direction);

            DrawGraph();

            Debug.Log($"[lyq]计算结果:{result}");
        }

        // 判断目标点是否在矩形范围内（包含目标体积）
        public void CheckTargetInRectangleWithBodySize() {
            Clear();
            var result = Range2DAction.CheckTargetInRectangleWithBodySize(caster.pos, target.pos, target.radius, drawGraphArgs.direction, drawGraphArgs.length, drawGraphArgs.width, drawGraphArgs.centerRangType);

            DrawGraph();

            Debug.Log($"[lyq]计算结果:{result}");
        }

        // 判断目标点是否在矩形范围内
        public void CheckTargetInRectangle() {
            Clear();
            var result = Range2DAction.CheckTargetInRectangle(caster.pos, target.pos, drawGraphArgs.direction, drawGraphArgs.length, drawGraphArgs.width, drawGraphArgs.centerRangType);

            DrawGraph();

            Debug.Log($"[lyq]计算结果:{result}");
        }



        void DrawGraph() {
            DrawGraphToArgs(drawGraphArgs);
            DrawGraphToActor(caster);
            DrawGraphToActor(target);
        }

        void DrawGraphToArgs(DrawGraphArgs args) {
            DrawGraphBase casterDrawGraph = CreateDrawGraph(args);
        }

        void DrawGraphToActor(Actor2D actor) {
            DrawGraphArgs args = new DrawGraphArgs() {
                pos = actor.pos,
                radius = actor.radius,
            };

            DrawGraphBase casterDrawGraph = CreateDrawGraph(args);
        }

        DrawGraphBase CreateDrawGraph(DrawGraphArgs args) {
            DrawGraphBase drawGraph;

            switch (args.drawGraphType) {
                case DrawGraphType.Circle:
                    drawGraph = GameObject.Instantiate(drawCircle, this.transform);
                    break;
                case DrawGraphType.Rectangle:
                    drawGraph = GameObject.Instantiate(drawRectangle, this.transform);
                    break;
                case DrawGraphType.Sector:
                    drawGraph = GameObject.Instantiate(drawSector, this.transform);
                    break;
                default:
                    drawGraph = GameObject.Instantiate(drawCircle, this.transform);
                    break;
            }

            drawGraph.InitArgs(args);
            drawGraph.Draw2D();
            return drawGraph;
        }
    }
}