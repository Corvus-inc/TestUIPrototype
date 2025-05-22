using System;
using MainMenu;

namespace CharacterSelect
{
    public sealed class CharacterSelectPresenter : IDisposable
    {
        private readonly ICharacterSelectView _view;
        private readonly ISelectorModel _model;

        public CharacterSelectPresenter(ICharacterSelectView view, ISelectorModel model)
        {
            _view  = view;
            _model = model;

            // init UI
            _view.Build(_model.Characters.Count);
            for (int i = 0; i < _model.Characters.Count; i++)
            {
                var cm = _model.Characters[i];
                _view.SetSmallIcon(i, cm.Data.icon);
                _view.SetProgress(i, cm.Xp / (float)cm.GetXpToNextLevel());
                cm.OnXpChanged += (_, _) => RefreshProgress(i);
            }
            RefreshSelection(0, instant:true);

            // subscribe UI events
            _view.OnCharacterButtonClicked += OnUiSelect;
            _view.OnBackClicked            += HandleBack;
            _model.OnSelectedChanged       += modelIndex => RefreshSelection(modelIndex, instant:false);
        }

        void OnUiSelect(int idx) => _model.Select(idx);

        void RefreshSelection(int newIndex)
        {
            RefreshSelection(newIndex, false);
        }
        
        void RefreshSelection(int newIndex, bool instant)
        {
            var sprite = _model.Characters[newIndex].Data.icon;
            var cm = _model.Characters[newIndex];
            var currentValue = cm.Xp / (float)cm.GetXpToNextLevel();
            if (instant) _view.SetSelectedBigIcon(sprite, currentValue);
            else _view.AnimateSwitch(_model.SelectedIndex, newIndex, currentValue);
            _view.SetSelectedBigIcon(sprite, currentValue);
        }

        void RefreshProgress(int idx)
        {
            var cm = _model.Characters[idx];
            _view.SetProgress(idx, cm.Xp / (float)cm.GetXpToNextLevel());
        }

        void HandleBack()
        {
            _view.Hide();            
        }

        public void Dispose()
        {
            _view.OnCharacterButtonClicked -= OnUiSelect;
            _view.OnBackClicked            -= HandleBack;
            _model.OnSelectedChanged       -= RefreshSelection;
        }
    }

}