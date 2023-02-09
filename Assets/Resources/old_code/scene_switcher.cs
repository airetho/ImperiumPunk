using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class scene_switcher : MonoBehaviour
{
    public void sceneForward()
    {
        Debug.Log("Call Forward");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void sceneBack()
    {
        Debug.Log("Call Backward");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
