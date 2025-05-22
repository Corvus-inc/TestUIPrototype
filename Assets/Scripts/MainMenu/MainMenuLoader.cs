using CharacterSelect;
using UnityEngine;

namespace MainMenu
{
    public class MainMenuLoader : MonoBehaviour
    {
        [SerializeField] private Transform uiRoot;
        [SerializeField] private GameObject mainMenuPrefab;
        [SerializeField] private GameObject characterSelectPrefab;

        private MainMenu _mainMenu;
        private CharacterSelectView _characterSelectView;
        private MainMenuNavigator _navigator;

        private void Awake()
        {
            _mainMenu = Instantiate(mainMenuPrefab, uiRoot).GetComponent<MainMenu>();
            _characterSelectView = Instantiate(characterSelectPrefab, uiRoot).GetComponent<CharacterSelectView>();
            _characterSelectView.gameObject.SetActive(false);

            _navigator = new MainMenuNavigator(_mainMenu, _characterSelectView);
        }

        private void OnDestroy()
        {
            _navigator?.Dispose();

            if (_mainMenu != null) Destroy(_mainMenu.gameObject);
            if (_characterSelectView != null) Destroy(_characterSelectView.gameObject);
        }
    }
}