using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KandiBullet : MonoBehaviour
{
    public float liveTime;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, liveTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name != "Player")
        {
            Destroy(gameObject);
        }
    }
}

