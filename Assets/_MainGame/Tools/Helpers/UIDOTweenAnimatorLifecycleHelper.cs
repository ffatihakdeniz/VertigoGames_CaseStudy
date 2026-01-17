using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;

namespace FatihAkdeniz.Helpers.DotweenHelpers
{
    /// <summary>
    /// UI tarafinda birden fazla DOTweenAnimation varsa:
    /// - Oyun basinda default state'i tek sefer capture eder
    /// - OnEnable'da hepsini oynatir
    /// - OnDisable'da hepsini durdurur ve default state'e tek seferde (instant) alir
    /// </summary>
    public class UIDOTweenAnimatorLifecycleHelper : MonoBehaviour
    {
        [Header("If empty, will auto-find DOTweenAnimation on this GameObject (and children if enabled).")]
        [SerializeField] private List<DOTweenAnimation> animations = new();

        [SerializeField] private bool includeChildren = true;

        [Header("Restore Behavior")]
        [Tooltip("Disable olunca resetlerken Rewind + Restore default state + Pause uygular.")]
        [SerializeField] private bool rewindOnDisable = true;

        [SerializeField] private bool restoreDefaultsOnDisable = true;

        bool _defaultsCaptured;
        readonly List<(DOTweenAnimation anim, object defaultValue)> _defaults = new();

        void Awake()
        {
            CaptureDefaultsOnce();
        }

        void OnEnable()
        {
            EnsureAnimationsList();
            PlayAll();
        }

        void OnDisable()
        {
            StopAndRestore();
        }

        // ---- Public API (optional) ----

        public void CaptureDefaultsOnce()
        {
            if (_defaultsCaptured) return;

            EnsureAnimationsList();

            _defaults.Clear();
            foreach (var anim in animations)
            {
                if (anim == null) continue;
                _defaults.Add((anim, CaptureDefaultValue(anim)));
            }

            _defaultsCaptured = true;
        }

        // ---- Internals ----

        void EnsureAnimationsList()
        {
            if (animations != null && animations.Count > 0) return;

            animations = new List<DOTweenAnimation>(
                includeChildren
                    ? GetComponentsInChildren<DOTweenAnimation>(true)
                    : GetComponents<DOTweenAnimation>()
            );
        }

        void PlayAll()
        {
            foreach (var anim in animations)
            {
                if (anim == null) continue;

                // Eğer tween daha önce autogenerate olmadıysa burada üretip oynatmak daha stabil
                if (anim.isActive && anim.autoGenerate && anim.tween == null)
                    anim.CreateTween(false, false);

                anim.DORestart(); // DOTweenAnimation kendi target-id grubuyla oynatir
            }
        }

        void StopAndRestore()
        {
            if (animations == null) return;

            foreach (var anim in animations)
            {
                if (anim == null) continue;

                if (rewindOnDisable) anim.DORewind();
                else anim.DOPause();
            }

            if (!restoreDefaultsOnDisable) return;

            if (!_defaultsCaptured) CaptureDefaultsOnce();

            // Default degerleri tek seferde geri bas
            for (int i = 0; i < _defaults.Count; i++)
            {
                var (anim, def) = _defaults[i];
                if (anim == null) continue;
                RestoreDefaultValue(anim, def);
            }
        }

        // ---- Default Value Capture/Restore ----
        // DOTweenAnimation'in "from value"si runtime'da direkt expose edilmedigi icin
        // pratikte target'in mevcut degerlerini yakalayip geri basiyoruz.
        // UI’da en yaygin olanlari kapsadik: RectTransform(anchoredPos/sizeDelta), Transform(localPos/localRot/localScale), Graphic/Image/Text/TMP color&alpha, FillAmount.

        object CaptureDefaultValue(DOTweenAnimation anim)
        {
            if (anim == null || anim.target == null) return null;

            switch (anim.animationType)
            {
                case DOTweenAnimation.AnimationType.Move:
                case DOTweenAnimation.AnimationType.LocalMove:
                case DOTweenAnimation.AnimationType.PunchPosition:
                case DOTweenAnimation.AnimationType.ShakePosition:
                    {
                        if (anim.target is RectTransform rt) return rt.anchoredPosition3D;
                        if (anim.target is Transform t) return (anim.animationType == DOTweenAnimation.AnimationType.Move) ? t.position : t.localPosition;
                        return null;
                    }

                case DOTweenAnimation.AnimationType.Rotate:
                case DOTweenAnimation.AnimationType.LocalRotate:
                case DOTweenAnimation.AnimationType.PunchRotation:
                case DOTweenAnimation.AnimationType.ShakeRotation:
                    {
                        if (anim.target is Transform t) return (anim.animationType == DOTweenAnimation.AnimationType.Rotate) ? t.eulerAngles : t.localEulerAngles;
                        return null;
                    }

                case DOTweenAnimation.AnimationType.Scale:
                case DOTweenAnimation.AnimationType.PunchScale:
                case DOTweenAnimation.AnimationType.ShakeScale:
                    {
                        if (anim.target is Transform t) return t.localScale;
                        return null;
                    }

                case DOTweenAnimation.AnimationType.UIWidthHeight:
                    {
                        if (anim.target is RectTransform rt) return rt.sizeDelta;
                        return null;
                    }

                case DOTweenAnimation.AnimationType.FillAmount:
                    {
                        if (anim.target is UnityEngine.UI.Image img) return img.fillAmount;
                        return null;
                    }

                case DOTweenAnimation.AnimationType.Color:
                case DOTweenAnimation.AnimationType.Fade:
                    {
                        // UI: Graphic (Image/Text/TMP) vs SpriteRenderer vs Renderer vs CanvasGroup
                        if (anim.target is CanvasGroup cg) return cg.alpha;

                        if (anim.target is UnityEngine.UI.Graphic g)
                            return g.color;

                        if (anim.target is SpriteRenderer sr)
                            return sr.color;

                        if (anim.target is Renderer r && r.material != null)
                            return r.material.color;

                        if (anim.target is TextMeshProUGUI tmpUgui)
                            return tmpUgui.color;

                        if (anim.target is TextMeshPro tmp)
                            return tmp.color;

                        return null;
                    }

                case DOTweenAnimation.AnimationType.Text:
                    {
                        if (anim.target is UnityEngine.UI.Text ut) return ut.text;
                        if (anim.target is TextMeshProUGUI tmpUgui) return tmpUgui.text;
                        if (anim.target is TextMeshPro tmp) return tmp.text;
                        return null;
                    }
            }

            return null;
        }

        void RestoreDefaultValue(DOTweenAnimation anim, object def)
        {
            if (anim == null || anim.target == null || def == null) return;

            switch (anim.animationType)
            {
                case DOTweenAnimation.AnimationType.Move:
                case DOTweenAnimation.AnimationType.LocalMove:
                case DOTweenAnimation.AnimationType.PunchPosition:
                case DOTweenAnimation.AnimationType.ShakePosition:
                    {
                        if (anim.target is RectTransform rt && def is Vector3 v3rt) rt.anchoredPosition3D = v3rt;
                        else if (anim.target is Transform t && def is Vector3 v3)
                        {
                            if (anim.animationType == DOTweenAnimation.AnimationType.Move) t.position = v3;
                            else t.localPosition = v3;
                        }
                        break;
                    }

                case DOTweenAnimation.AnimationType.Rotate:
                case DOTweenAnimation.AnimationType.LocalRotate:
                case DOTweenAnimation.AnimationType.PunchRotation:
                case DOTweenAnimation.AnimationType.ShakeRotation:
                    {
                        if (anim.target is Transform t && def is Vector3 eul)
                        {
                            if (anim.animationType == DOTweenAnimation.AnimationType.Rotate) t.eulerAngles = eul;
                            else t.localEulerAngles = eul;
                        }
                        break;
                    }

                case DOTweenAnimation.AnimationType.Scale:
                case DOTweenAnimation.AnimationType.PunchScale:
                case DOTweenAnimation.AnimationType.ShakeScale:
                    {
                        if (anim.target is Transform t && def is Vector3 sc) t.localScale = sc;
                        break;
                    }

                case DOTweenAnimation.AnimationType.UIWidthHeight:
                    {
                        if (anim.target is RectTransform rt && def is Vector2 sz) rt.sizeDelta = sz;
                        break;
                    }

                case DOTweenAnimation.AnimationType.FillAmount:
                    {
                        if (anim.target is UnityEngine.UI.Image img && def is float f) img.fillAmount = f;
                        break;
                    }

                case DOTweenAnimation.AnimationType.Color:
                case DOTweenAnimation.AnimationType.Fade:
                    {
                        if (anim.target is CanvasGroup cg && def is float a) cg.alpha = a;
                        else if (anim.target is UnityEngine.UI.Graphic g && def is Color c) g.color = c;
                        else if (anim.target is SpriteRenderer sr && def is Color cs) sr.color = cs;
                        else if (anim.target is Renderer r && r.material != null && def is Color cm) r.material.color = cm;
                        else if (anim.target is TextMeshProUGUI tmpUgui && def is Color ctu) tmpUgui.color = ctu;
                        else if (anim.target is TextMeshPro tmp && def is Color ct) tmp.color = ct;
                        break;
                    }

                case DOTweenAnimation.AnimationType.Text:
                    {
                        if (def is string s)
                        {
                            if (anim.target is UnityEngine.UI.Text ut) ut.text = s;
                            else if (anim.target is TextMeshProUGUI tmpUgui) tmpUgui.text = s;
                            else if (anim.target is TextMeshPro tmp) tmp.text = s;
                        }
                        break;
                    }
            }
        }
    }
}
