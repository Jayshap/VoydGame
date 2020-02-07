using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeShot : MonoBehaviour
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
        // destroy if colliding with something other than boss or other attacks
        if (col.gameObject.layer != LayerMask.NameToLayer("Boss") && col.gameObject.layer != LayerMask.NameToLayer("Attacks") && col.gameObject.layer != LayerMask.NameToLayer("AttackCollider"))
        {
            Destroy(gameObject);
        }
    }
}
