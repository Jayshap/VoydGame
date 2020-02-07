using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodMoon : MonoBehaviour
{
    public GameObject startMarker;
    public GameObject endMarker;

    public float totalLerpTime;
    public float spinSpeed;

    private float currentLerpTime;
    private bool lerpFinished;
    // Start is called before the first frame update
    void Start()
    {
        currentLerpTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!lerpFinished)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > totalLerpTime)
            {
                lerpFinished = true;
                currentLerpTime = totalLerpTime;
            }

            float percent = currentLerpTime / totalLerpTime;
            transform.position = Vector3.Lerp(startMarker.transform.position, endMarker.transform.position, percent);
        }

        transform.RotateAround(transform.position, new Vector3(0, 0, 1), spinSpeed);

    }
}
