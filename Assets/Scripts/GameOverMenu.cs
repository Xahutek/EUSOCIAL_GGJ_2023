using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using DG.Tweening;

public class GameOverMenu : MonoBehaviour
{
    public void Leave()
    {
        Application.Quit();
    }
    public void Again()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
