using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopGame : MonoBehaviour
{

    public GameObject canvas;


    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnStopped);
    }

    public void OnStopped()
    {
        Time.timeScale = 0;
        canvas.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnGame()
    {
        Time.timeScale = 1;
        canvas.SetActive(false);
        gameObject.SetActive(true);
    }
}
