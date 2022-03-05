using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonComponent : MonoBehaviour
{
    public GameObject level;

    public void LoadLevel()
    {

        StartCoroutine(DelayInstance());
    }

    IEnumerator DelayInstance()
    {
        yield return new WaitForSeconds(.5f);
        Instantiate(level);
        UIManager.current.GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
