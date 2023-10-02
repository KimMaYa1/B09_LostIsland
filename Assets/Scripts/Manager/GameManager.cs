using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    private GameObject playerObj;

    public GameObject PlayerObj
    {
        get { return playerObj; }
    }
    public static GameManager Instance {
        get {
                return _instance;
            } 
    }
    void Awake()
    {
        _instance = this;
        playerObj = GameObject.FindWithTag("Player");
    }

}
