using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class goToSceneGame : MonoBehaviour
{

    public void GoToGameScene()
    {
        SceneManager.LoadScene("Game");
    }
}
