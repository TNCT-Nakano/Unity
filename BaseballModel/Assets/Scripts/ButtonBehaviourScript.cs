using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehaviourScript : MonoBehaviour {

	public void startBaseball()
    {
        SceneManager.LoadScene("BaseballModel");
    }

    public void startSetting()
    {
        //SceneManager.LoadScene("Setting");
    }

    public void endGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
