using UnityEngine;
using UnityEditor;

namespace VertigoCase.Systems.ZoneSystem
{
    [CustomEditor(typeof(RewardSO))]
    public class RewardEditor : Editor
    {
        private RewardSO reward;
        private int zoneLevel = 1;
        private float zoneMultiplier = 0.01f;

        private void OnEnable()
        {
            reward = (RewardSO)target;
        }

        public override void OnInspectorGUI()
        {
            DrawIconPreview();
            GUILayout.Space(15);

            base.OnInspectorGUI();
            GUILayout.Space(15);

            DrawDebugSection();
        }

        private void DrawIconPreview()
        {
            EditorGUILayout.LabelField("Icon Preview", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");

            if (reward.icon != null)
            {
                Texture2D preview = AssetPreview.GetAssetPreview(reward.icon);
                if (preview != null)
                {
                    float ratio = (float)preview.width / preview.height;
                    float height = 100f;
                    float width = height * ratio;

                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    Rect rect = GUILayoutUtility.GetRect(width, height);
                    GUI.DrawTexture(rect, preview, ScaleMode.ScaleToFit);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
            }
            else
            {
                EditorGUILayout.HelpBox("No icon assigned.", MessageType.Warning);
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawDebugSection()
        {
            EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");

            zoneLevel = EditorGUILayout.IntField("Zone Level", zoneLevel);
            EditorGUILayout.LabelField("Calculated Scale", reward.CalculateScale(zoneLevel).ToString("N1"));

            GUILayout.Space(10);

            zoneMultiplier = EditorGUILayout.FloatField("Zone Multiplier", zoneMultiplier);
            EditorGUILayout.LabelField(
                "Calculated Reward",
                reward.CalculateRewardAmount(zoneMultiplier * zoneLevel).ToString("N0")
            );

            EditorGUILayout.EndVertical();
        }
    }
}
