using UnityEngine;
using UnityEditor;
using BusinessAlgorithm.DrawGraph;
using BusinessAlgorithm.Test;

namespace BusinessAlgorithm.BAEditor {

    [CustomEditor(typeof(TestRangeSearch2D), true)]
    public class TestRangeSearch2DEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            TestRangeSearch2D script = (TestRangeSearch2D)target;

            // 指定按钮的宽度、高度、间隔
            float buttonWidth = 300f;
            float buttonHeight = 40f;
            float buttonSpacing = 10f;

            GUILayout.Space(30);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("清除", GUILayout.Width(100), GUILayout.Height(40))) {
                script.Clear();
            }

            GUILayout.Space(30);

            if (GUILayout.Button("自定义方法", GUILayout.Width(100), GUILayout.Height(40))) {
                script.CustomAction();
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();

            GUILayout.Space(buttonSpacing);

            GUILayout.Space(buttonSpacing);

            if (GUILayout.Button("计算起始点与目标点的距离,包含目标点的体积", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight))) {
                script.GetStartCenterToTargetDisWithBodySize();
            }

            GUILayout.Space(buttonSpacing);

            if (GUILayout.Button("计算起始点与目标点的距离,包含双方的体积", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight))) {
                script.GetStartToTargetDisWithBodySize();
            }

            GUILayout.Space(buttonSpacing);

            if (GUILayout.Button("判断起始点和目标点的距离是否在指定范围内（包含目标体积）", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight))) {
                script.CheckTargetInRange();
            }

            GUILayout.Space(buttonSpacing);

            if (GUILayout.Button("判断目标点是否处于指定角度的扇形范围内（包含目标点体积）", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight))) {
                script.CheckInSectorRangeOfDirectionWithBodySize();
            }

            GUILayout.Space(buttonSpacing);

            if (GUILayout.Button("判断目标点是否处于指定角度的扇形范围内", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight))) {
                script.CheckInSectorRangeOfDirection();
            }

            GUILayout.Space(buttonSpacing);

            if (GUILayout.Button("判断目标点是否在矩形范围内（包含目标体积）", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight))) {
                script.CheckTargetInRectangleWithBodySize();
            }

            GUILayout.Space(buttonSpacing);

            if (GUILayout.Button("判断目标点是否在矩形范围内", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight))) {
                script.CheckTargetInRectangle();
            }

            GUILayout.EndVertical();

        }
    }
}
