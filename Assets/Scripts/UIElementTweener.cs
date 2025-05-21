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
    [SerializeField] private Ease showEase = Ease.OutBack;
    [Tooltip("Ease type for hide animation")]
    [SerializeField] private Ease hideEase = Ease.InBack;

    protected RectTransform rectTransform;
    private Sequence showSequence;
    private Tweener hideTweener;

    public bool IsVisible { get; private set; } = true;

    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = startScale;
    }

    public void Show()
    {
        showSequence?.Kill();
        hideTweener?.Kill();

        rectTransform.localScale = startScale;

        showSequence = DOTween.Sequence();
        showSequence.Append(rectTransform
            .DOScale(overshootScale, showDuration * 0.6f)
            .SetEase(showEase));
        showSequence.Append(rectTransform
            .DOScale(endScale, showDuration * 0.4f)
            .SetEase(showEase));
    }

    public void Hide()
    {
        showSequence?.Kill();
        hideTweener?.Kill();

        hideTweener = rectTransform
            .DOScale(Vector3.zero, hideDuration)
            .SetEase(hideEase);
    }

    public void Toggle()
    {
        if (IsVisible)
        {
            Hide();
        }
        else Show();
        
        IsVisible = !IsVisible;
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