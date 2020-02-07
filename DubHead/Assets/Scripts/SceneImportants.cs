using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneImportants : MonoBehaviour
{
    public GameObject player;
    public GameObject cam;


    private static SceneImportants _instance;

    public static SceneImportants Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
}