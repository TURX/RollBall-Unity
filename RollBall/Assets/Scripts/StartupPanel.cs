using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartupPanel : MonoBehaviour
{

    public Transform player;

    private GUIStyle label = new GUIStyle();

    void Start()
    {
        label.fontSize = 60;
    }

    void OnGUI()
    {

        GUI.Label(new Rect(Screen.width / 2 - 180, Screen.height / 2 - 100, 100, 60), "RollBall Game", label);
        if (GUI.Button(new Rect(Screen.width / 2 - 25, Screen.height / 2 + 50, 50, 30), "Quit"))
        {
            Application.Quit();
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 15, 100, 60), "Start Game"))
        {
            player.gameObject.SetActive(true);
            transform.gameObject.SetActive(false);
        }
    }
}
