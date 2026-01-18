using UnityEditor;
using UnityEngine;

namespace VertigoCase.Systems.ZoneSystem
{
    [CustomEditor(typeof(RewardDataSO))]
    public class RewardSOEditor : Editor
    {
        private RewardDataSO _reward;

        // Serialized props
        private SerializedProperty _rewardType;
        private SerializedProperty _rewardName;
        private SerializedProperty _rewardID;
        private SerializedProperty _zoneType;
        private SerializedProperty _icon;

        private SerializedProperty _baseAmount;
        private SerializedProperty _scaleMultiplier;

        // Editor prefs keys
        private const string PrefRefPPU = "VertigoCase.RewardSOEditor.ReferencePPU";
        private const string PrefFrameW = "VertigoCase.RewardSOEditor.FrameW";
        private const string PrefFrameH = "VertigoCase.RewardSOEditor.FrameH";

        // Designer settings
        private float _referencePixelsPerUnit = 100f;               // Canvas: Reference Pixels Per Unit
        private Vector2 _frameSize = new Vector2(100f, 100f);       // Red frame default: 100x100

        private void OnEnable()
        {
            _reward = (RewardDataSO)target;

            _rewardType = serializedObject.FindProperty("rewardType");
            _rewardName = serializedObject.FindProperty("rewardName");
            _rewardID = serializedObject.FindProperty("rewardID");
            _zoneType = serializedObject.FindProperty("zoneType");
            _icon = serializedObject.FindProperty("icon");

            _baseAmount = serializedObject.FindProperty("baseAmount");
            _scaleMultiplier = serializedObject.FindProperty("scaleMultiplier");

            // Load editor prefs
            _referencePixelsPerUnit = Mathf.Max(1f, EditorPrefs.GetFloat(PrefRefPPU, 100f));
            float w = Mathf.Max(1f, EditorPrefs.GetFloat(PrefFrameW, 100f));
            float h = Mathf.Max(1f, EditorPrefs.GetFloat(PrefFrameH, 100f));
            _frameSize = new Vector2(w, h);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawHeader();
            GUILayout.Space(8);

            DrawIdentitySection();
            GUILayout.Space(8);

            DrawDesignerSettingsSection();
            GUILayout.Space(8);

            DrawVisualSection();
            GUILayout.Space(8);

            DrawValueSection();
            GUILayout.Space(8);

            DrawScaleSection();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawHeader()
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("RewardSO", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Wheel Game Reward Configuration", EditorStyles.miniLabel);
            }
        }

        private void DrawIdentitySection()
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Identity", EditorStyles.boldLabel);

                EditorGUILayout.PropertyField(_rewardType);
                EditorGUILayout.PropertyField(_rewardName);
                EditorGUILayout.PropertyField(_rewardID);
                EditorGUILayout.PropertyField(_zoneType);
            }
        }

        private void DrawDesignerSettingsSection()
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Designer Settings", EditorStyles.boldLabel);

                EditorGUI.BeginChangeCheck();

                _referencePixelsPerUnit = EditorGUILayout.FloatField("Reference Pixels Per Unit", _referencePixelsPerUnit);
                _referencePixelsPerUnit = Mathf.Max(1f, _referencePixelsPerUnit);

                _frameSize = EditorGUILayout.Vector2Field("Icon Frame Size", _frameSize);
                _frameSize.x = Mathf.Max(1f, _frameSize.x);
                _frameSize.y = Mathf.Max(1f, _frameSize.y);

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Reset Frame (100x100)"))
                        _frameSize = new Vector2(100f, 100f);

                    if (GUILayout.Button("Reset Overflow (1.0)"))
                        _scaleMultiplier.floatValue = 1f;
                }

                if (EditorGUI.EndChangeCheck())
                {
                    EditorPrefs.SetFloat(PrefRefPPU, _referencePixelsPerUnit);
                    EditorPrefs.SetFloat(PrefFrameW, _frameSize.x);
                    EditorPrefs.SetFloat(PrefFrameH, _frameSize.y);
                }
            }
        }

        private void DrawVisualSection()
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Visual", EditorStyles.boldLabel);

                EditorGUILayout.PropertyField(_icon);

                DrawFramedIconPreview();
            }
        }

        private void DrawFramedIconPreview()
        {
            var sprite = _reward.icon;
            if (sprite == null)
            {
                EditorGUILayout.HelpBox("No icon assigned.", MessageType.Info);
                return;
            }

            Texture2D tex = AssetPreview.GetAssetPreview(sprite) ?? (AssetPreview.GetMiniThumbnail(sprite) as Texture2D);
            if (tex == null)
            {
                EditorGUILayout.HelpBox("Icon preview is not ready yet (Unity is generating it).", MessageType.None);
                return;
            }

            // 1) Sprite native UI size (Image.SetNativeSize)
            Vector2 nativeUISize = GetNativeUISize(sprite, _referencePixelsPerUnit);
            if (nativeUISize.x <= 0f || nativeUISize.y <= 0f)
                return;

            // 2) "Auto-fit to frame" scale (contain) -> icon normalde HER ZAMAN frame'e sigar
            float fitScale = Mathf.Min(_frameSize.x / nativeUISize.x, _frameSize.y / nativeUISize.y);
            fitScale = Mathf.Max(0f, fitScale);

            // 3) Overflow tuning (scaleMultiplier) -> 1.0 iken tam sigar, >1 tasar, <1 daha kucuk kalir
            float overflow = Mathf.Max(0f, _scaleMultiplier.floatValue);
            Vector2 finalUISize = nativeUISize * fitScale * overflow;

            GUILayout.Space(6);

            const float pad = 14f;
            float previewW = Mathf.Max(240f, _frameSize.x + pad * 2f);
            float previewH = Mathf.Max(140f, _frameSize.y + pad * 2f);

            Rect previewRect = GUILayoutUtility.GetRect(previewW, previewH, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(previewRect, new Color(0f, 0f, 0f, 0.08f));

            // Frame rect centered
            Rect frameRect = new Rect(
                previewRect.center.x - _frameSize.x * 0.5f,
                previewRect.center.y - _frameSize.y * 0.5f,
                _frameSize.x,
                _frameSize.y
            );

            // Red frame
            DrawRectOutline(frameRect, new Color(1f, 0f, 0f, 0.9f), 2f);

            // Icon rect centered to frame (hizalama)
            Rect iconRect = new Rect(
                frameRect.center.x - finalUISize.x * 0.5f,
                frameRect.center.y - finalUISize.y * 0.5f,
                finalUISize.x,
                finalUISize.y
            );

            GUI.DrawTexture(iconRect, tex, ScaleMode.ScaleToFit, true);

            GUILayout.Space(6);
            using (new EditorGUILayout.VerticalScope("box"))
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUILayout.Vector2Field("Native UI Size", nativeUISize);
                    EditorGUILayout.FloatField("Auto Fit Scale (Contain)", fitScale);
                    EditorGUILayout.FloatField("Overflow Multiplier", overflow);
                    EditorGUILayout.Vector2Field("Final UI Size", finalUISize);
                }

                EditorGUILayout.HelpBox(
                    "Icon is automatically fitted to the red frame. Scale Multiplier now controls intentional overflow (1.0 = fits, >1.0 = overflow).",
                    MessageType.None
                );
            }
        }

        private void DrawValueSection()
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Value", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_baseAmount, new GUIContent("Base Amount"));
            }
        }

        private void DrawScaleSection()
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Scale Settings", EditorStyles.boldLabel);

                // Designer: overflow ayari
                EditorGUILayout.PropertyField(_scaleMultiplier, new GUIContent("Scale Multiplier (Overflow)"));

                // UX: hizli slider
                float v = _scaleMultiplier.floatValue;
                float nv = EditorGUILayout.Slider("Overflow Slider", v, 0.5f, 1.5f);
                if (!Mathf.Approximately(nv, v))
                    _scaleMultiplier.floatValue = nv;
            }
        }

        /// <summary>
        /// UnityEngine.UI.Image.SetNativeSize() ile aynı hesap:
        /// nativeSize = sprite.rect.size * (referencePPU / spritePPU)
        /// </summary>
        private static Vector2 GetNativeUISize(Sprite sprite, float referencePixelsPerUnit)
        {
            if (sprite == null) return Vector2.zero;

            float spritePPU = sprite.pixelsPerUnit;
            if (spritePPU <= 0f) spritePPU = 100f;

            return sprite.rect.size * (referencePixelsPerUnit / spritePPU);
        }

        private static void DrawRectOutline(Rect rect, Color color, float thickness)
        {
            // Top
            EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
            // Bottom
            EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
            // Left
            EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
            // Right
            EditorGUI.DrawRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        }
    }
}
