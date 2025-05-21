using UnityEngine;

public class MainLoader : MonoBehaviour
{
   [SerializeField] private Transform uiRoot;
   [SerializeField] private GameObject mainMenuPrefab;

   private void Awake()
   {
       if (uiRoot == null)
       {
           Debug.LogError("UI Root is not assigned in the inspector.");
           return;
       }

       if (mainMenuPrefab == null)
       {
           Debug.LogError("Main Menu Prefab is not assigned in the inspector.");
           return;
       }

       Instantiate(mainMenuPrefab, uiRoot);
   }
}
