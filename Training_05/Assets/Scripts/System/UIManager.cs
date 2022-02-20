using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager current;

    public CanvasGroup[] canvasGroups;
    public CanvasGroup currentMenu;
    MenuGizmo menuGizmo;
    

    public bool playTransitionActive = true;
    public float menuTransitionTime;
    public float sceneTransitionTime;



    private void Awake()
    {
        current = this;
        DontDestroyOnLoad(this);

        canvasGroups = GetComponentsInChildren<CanvasGroup>();
        currentMenu = canvasGroups[1];
        menuGizmo = canvasGroups[0].GetComponent<MenuGizmo>();
    }

    #region Public Methods

    public void SwitchToMenu(int _i)
    {
        StartCoroutine(MenuTransition(_i));
    }

    public void HideMenu()
    {
        currentMenu.alpha = 0;
        currentMenu.interactable = false;
        currentMenu.blocksRaycasts = false;
        currentMenu = null;
        menuGizmo.settingsButton.gameObject.SetActive(false);
        menuGizmo.pauseButton.gameObject.SetActive(true);
    }

    public void LoadScene(int _i)
    {
        StartCoroutine(SceneTransition(_i));
    }

    public void QuitPrompt(bool _b)
    {
        currentMenu.interactable = !_b;
        currentMenu.blocksRaycasts = !_b;
        canvasGroups[canvasGroups.Length - 1].alpha = _b ? 1f : 0f;
        canvasGroups[canvasGroups.Length - 1].interactable = _b;
        canvasGroups[canvasGroups.Length - 1].blocksRaycasts = _b;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
        
#endif
    }

    #endregion

    #region Coroutines

    IEnumerator MenuTransition( int _i)
    {
        if (_i < canvasGroups.Length)
        {
            if(currentMenu != null)
            {
                currentMenu.blocksRaycasts = false;
                currentMenu.interactable = false;
                currentMenu.alpha = 0;
            }
            

            if (playTransitionActive)
            {
                // call transition

                yield return new WaitForSecondsRealtime(menuTransitionTime);

            }
            else yield return null;

            currentMenu = canvasGroups[_i];
            currentMenu.blocksRaycasts = true;
            currentMenu.interactable = true;
            currentMenu.alpha = 1;
            if (_i != 4)
                menuGizmo.pauseButton.gameObject.SetActive(false);
            else menuGizmo.pauseButton.gameObject.SetActive(true);
            menuGizmo.settingsButton.gameObject.SetActive(true);
        }
        else Debug.LogError(_i + " is not a valid CanvasGroup ID");
        
    }

    IEnumerator SceneTransition(int _i)
    {
        if (_i < SceneManager.sceneCount)
        {
            // call transition

            yield return new WaitForSecondsRealtime(sceneTransitionTime);
            HideMenu();
            SceneManager.LoadScene(_i);
        }
        else Debug.LogError(_i + " is not a valid Scene ID");

    }

#endregion
}
