using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float yAxis;
    public float zAxis;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        GameObject toFollow = SceneImportants.Instance.player;
        SceneImportants.Instance.cam.transform.position = new Vector3(toFollow.transform.position.x, toFollow.transform.position.y + yAxis, zAxis);
    }
}
