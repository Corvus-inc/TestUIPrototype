using System;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : UIElementTweener
{
    [SerializeField] private List<GameObject> buttonPrefabs;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private UIElementTweener labelTweener;
    
    private List<UIElementTweener> _buttonTweens = new ();
    
    private Action _playButtonsHandler;
    private readonly List<Action> _peakHandlers = new();
    private void Start()
    {
        InstantiateAndPrepareButtons();
        Show();
        labelTweener.Show();
    }
    
    private void InstantiateAndPrepareButtons()
    {
        foreach (var buttonPrefab in buttonPrefabs)
        {
            GameObject button = Instantiate(buttonPrefab, buttonContainer);
            var tween = button.GetComponent<UIElementTweener>();
            
            if (tween == null)
            {
                Debug.LogWarning($"Prefab '{buttonPrefab.name}' does not have a UIElementTweener component.");
                continue;
            }
            
            _buttonTweens.Add(tween);
        }

        _playButtonsHandler = PlayButtons;
        OnShowComplete += _playButtonsHandler;
    }

    private void PlayButtons()
    {
        if (_buttonTweens.Count == 0) return;

        _buttonTweens[0].Show();

        for (int i = 1; i < _buttonTweens.Count; i++)
        {
            int index = i;
            Action peakHandler = () => _buttonTweens[index].Show();
            _buttonTweens[index - 1].OnShowPeak += peakHandler;
            _peakHandlers.Add(peakHandler);
        }
    }
    
    private void OnDestroy()
    {
        if (_playButtonsHandler != null)
            OnShowComplete -= _playButtonsHandler;

        for (int i = 1; i < _buttonTweens.Count && i - 1 < _peakHandlers.Count; i++)
        {
            _buttonTweens[i - 1].OnShowPeak -= _peakHandlers[i - 1];
        }
        _peakHandlers.Clear();
    }
}
