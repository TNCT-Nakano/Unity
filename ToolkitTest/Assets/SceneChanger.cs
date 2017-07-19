using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.SceneManagement;
using System;

public class SceneChanger : MonoBehaviour, IInputClickHandler
{
    public string sceneName;
    public void OnInputClicked(InputClickedEventData eventData)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
