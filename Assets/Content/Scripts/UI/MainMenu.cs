using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject MenuUI;

    
    
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.scale(MenuUI, Vector3.one * 1.1f, 1f)
            .setEaseInOutSine()
            .setLoopPingPong()
            .setIgnoreTimeScale(true);

    }

    // Update is called once per frame
    public void GameStart()
    {
        SceneManager.LoadScene("InGame");
    }
}
