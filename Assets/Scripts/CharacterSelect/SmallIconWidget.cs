using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CharacterSelect
{
    public sealed class SmallIconWidget : MonoBehaviour
    {
        [SerializeField]
        [Header("References")]
        Image icon;

        [SerializeField] Slider progressBar;
        [SerializeField] Button button;

        public Image Icon => icon;
        public Slider ProgressBar => progressBar;
        public Button Button => button;

        void Awake()
        {
            button.onClick.AddListener(PlayClickAnimation);
        }

        void PlayClickAnimation()
        {
            icon.transform.DOKill();
            icon.transform.localScale = Vector3.one;
            icon.transform
                .DOPunchScale(Vector3.one * 0.15f, 0.2f, 8, 0.8f);
        }
    }
}