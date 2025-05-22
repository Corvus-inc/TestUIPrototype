using System;
using UnityEngine;
using UnityEngine.UI;

namespace CharacterSelect
{
    public class CharacterSelectView : UIElementTweener
    {
        [SerializeField] private Button backButton;
    
        public event Action OnBackClick = delegate { };
        
        private void OnEnable()
        {
            Show();
            backButton.onClick.AddListener(OnBackButtonClick);
        }
    
        private void OnDisable()
        {
            Hide();
            backButton.onClick.RemoveListener(OnBackButtonClick);
        }

        private void OnBackButtonClick()
        {
            OnBackClick?.Invoke();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            backButton.onClick.RemoveAllListeners();
            Destroy(gameObject);
        }
    }
}
