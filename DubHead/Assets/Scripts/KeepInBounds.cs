using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepInBounds : MonoBehaviour
{
    private Bounds screenBounds;
    private Vector2 verticalBoundsWorldSpace;
    private float objectWidth;

    // Use this for initialization
    void Start()
    {
        Camera boundingCamera = SceneImportants.Instance.cam.GetComponent<Camera>();

        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = boundingCamera.orthographicSize * 2;
        screenBounds = new Bounds( boundingCamera.transform.position, new Vector3(cameraHeight * screenAspect, cameraHeight, 0));

        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2

        verticalBoundsWorldSpace.x = screenBounds.center.x - screenBounds.extents.x + objectWidth;
        verticalBoundsWorldSpace.y = screenBounds.center.x + screenBounds.extents.x - objectWidth;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, verticalBoundsWorldSpace.x, verticalBoundsWorldSpace.y);
        transform.position = viewPos;
    }
}