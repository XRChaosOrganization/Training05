using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuGizmo : MonoBehaviour
{
    public Button pauseButton;
    public Image pauseSprite;
    public Button settingsButton;

    public Sprite pauseIcon;
    public Sprite playIcon;

    CanvasGroup previousMenu;

    bool pauseState;
    bool settingsState;





    public void OnClickSettings()
    {
        settingsState = !settingsState;
        UIManager.current.playTransitionActive = false;
        if (settingsState)
        {
            previousMenu = UIManager.current.currentMenu;
            UIManager.current.SwitchToMenu(5);
        }
        else
        {
            int menuID = -1;
            for (int i = 1; i < UIManager.current.canvasGroups.Length-1; i++)
                if (UIManager.current.canvasGroups[i] == previousMenu)
                    menuID = i;
            if (menuID < 0)
                UIManager.current.HideMenu();
            else UIManager.current.SwitchToMenu(menuID);
        }

        UIManager.current.playTransitionActive = true;

    }

    public void OnClickPause()
    {
        pauseState = !pauseState;
        Time.timeScale = pauseState ? 0f : 1f;
        UIManager.current.playTransitionActive = false;
        pauseSprite.sprite = pauseState ? playIcon : pauseIcon;

        if (pauseState)
            UIManager.current.SwitchToMenu(4);
        else UIManager.current.HideMenu();

        UIManager.current.playTransitionActive = true;



    }

    
}
