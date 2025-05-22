using System;
using UnityEngine;

namespace CharacterSelect
{
    public interface ICharacterSelectView
    {
        event Action<int>  OnCharacterButtonClicked;
        event Action       OnBackClicked;
        
        public void Build(int count);
        void SetSelectedBigIcon(Sprite icon);
        void SetSmallIcon(int index, Sprite icon);
        void SetProgress(int index, float normalizedValue);
        void AnimateSwitch(int from, int to);
        void Show();
        void Hide();
    }
}