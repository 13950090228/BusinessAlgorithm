using UnityEngine;
using UnityEditor;
using BusinessAlgorithm.DrawGraph;
using BusinessAlgorithm.Test;
namespace BusinessAlgorithm.BAEditor {

    [CustomEditor(typeof(TestRange), true)]
    public class TestRangeEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            TestRange script = (TestRange)target;

            // 指定按钮的宽度、高度、间隔
            float buttonWidth = 280f;
            float buttonHeight = 40f;
            float buttonSpacing = 10f;

            GUILayout.Space(30); // 添加间隔

            if (GUILayout.Button("清除", GUILayout.Width(100), GUILayout.Height(40))) {
                script.Clear();
            }

            // 让元素处于同一行得用 Begin 和 End 包起来
            GUILayout.BeginVertical();

            GUILayout.Space(buttonSpacing); // 添加间隔

            if (GUILayout.Button("计算起始点与目标点的距离,包含目标点的体积", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight))) {
                script.GetStartCenterToTargetDisWithBodySize();
            }

            GUILayout.Space(buttonSpacing); // 添加间隔

            if (GUILayout.Button("计算起始点与目标点的距离,包含目标点的体积", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight))) {
                script.GetStartToTargetDisWithBodySize();
            }

            GUILayout.Space(buttonSpacing); // 添加间隔

            GUILayout.EndVertical();

        }
    }

}
