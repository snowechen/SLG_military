using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    Text text;
    public void setGameOver()
    {
        text.text = "Game Over";
    }

    public void setWinner()
    {
        text.text = "You Win!";
    }
    public void SceneChange(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
