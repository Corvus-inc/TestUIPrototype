using System;
using UnityEngine;
using DG.Tweening;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(RectTransform))]
public class UIElementTweener : MonoBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("Start scale when showing")]
    [SerializeField] private Vector3 startScale = Vector3.one * 1f;
    [Tooltip("Overshoot scale")]
    [SerializeField] private Vector3 overshootScale = Vector3.one * 1.25f;
    [Tooltip("End scale after showing")]
    [SerializeField] private Vector3 endScale = Vector3.one;
    [Tooltip("Duration of show animation in seconds")]
    [SerializeField] private float showDuration = 0.3f;
    [Tooltip("Duration of hide animation in seconds")]
    [SerializeField] private float hideDuration = 0.2f;
    [Tooltip("Ease type for show animation")]
    [Range(0f, 1f)]
    [SerializeField] protected float overshootRatio = 0.6f;
    [Tooltip("Portion of showDuration for settling to final scale (0 to 1)")]
    [Range(0f, 1f)]
    [SerializeField] protected float settleRatio = 0.4f;
    [Tooltip("Ease type for show animation")]
    [SerializeField] private Ease showEase = Ease.OutBack;
    [Tooltip("Ease type for hide animation")]
    [SerializeField] private Ease hideEase = Ease.InBack;

    public event Action OnShowPeak;
    public event Action OnShowComplete;
    public event Action OnHideComplete;
    
    protected RectTransform RectTransformTweener;
    
    private Sequence _showSequence;
    
    private Tweener _overshootTweener;
    private Tweener _settleTweener;
    private Tweener _hideTweener;
    

    public bool IsVisible { get; private set; }

    protected virtual void Awake()
    {
        RectTransformTweener = GetComponent<RectTransform>();
        RectTransformTweener.localScale = startScale;
    }

    public void Show()
    {
        KillTweens();
        RectTransformTweener.localScale = startScale;
        IsVisible = true;

        _overshootTweener = RectTransformTweener
            .DOScale(overshootScale, showDuration * overshootRatio)
            .SetEase(showEase)
            .OnComplete(() => OnShowPeak?.Invoke());

        _settleTweener = RectTransformTweener
            .DOScale(endScale, showDuration * settleRatio)
            .SetEase(showEase)
            .OnComplete(() =>
                OnShowComplete?.Invoke());

        DOTween.Sequence()
            .Append(_overshootTweener)
            .Append(_settleTweener);
    }

    public void Hide()
    {
        KillTweens();
        IsVisible = false;

        _hideTweener = RectTransformTweener
            .DOScale(Vector3.zero, hideDuration)
            .SetEase(hideEase)
            .OnComplete(() => OnHideComplete?.Invoke());
    }

    public void Toggle()
    {
        if (IsVisible)
        {
            Hide();
        }
        else Show();
    }
    
    private void KillTweens()
    {
        _overshootTweener?.Kill();
        _settleTweener?.Kill();
        _hideTweener?.Kill();
    }
    
    protected virtual void OnDestroy()
    {
        KillTweens();

        OnShowPeak     = null;
        OnShowComplete = null;
        OnHideComplete = null;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(UIElementTweener))]
public class UIElementTweenerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        UIElementTweener tweener = (UIElementTweener)target;
        EditorGUILayout.Space();
        // Show toggle button only in play mode for runtime debugging
        if (Application.isPlaying)
        {
            if (GUILayout.Button(tweener.IsVisible ? "Hide Element" : "Show Element"))
            {
                tweener.Toggle();
            }
        }
    }
}
#endif