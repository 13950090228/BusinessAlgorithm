using UnityEngine;
using UnityEditor;
using BusinessAlgorithm.DrawGraph;

namespace BusinessAlgorithm.BAEditor {
    [CustomEditor(typeof(DrawGraphBase), true)]
    public class DrawGraphEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            DrawGraphBase script = (DrawGraphBase)target;

            // 指定按钮的宽度、高度、间隔
            float buttonWidth = 100f;
            float buttonHeight = 40f;
            float buttonSpacing = 10f; 

            // 让元素处于同一行得用 Begin 和 End 包起来
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("绘制", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight))) {
                script.Draw();
            }

            GUILayout.Space(buttonSpacing); // 添加间隔

            if (GUILayout.Button("清除", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight))) {
                script.Reset();
            }

            GUILayout.EndHorizontal();

        }
    }
}

