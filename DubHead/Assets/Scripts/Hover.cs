using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    public float hoverDistance;
    public float hoverSpeed;

    private bool goingUp = true;
    private float startingPos;
    private Vector2 yBounds;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position.y;
        yBounds.x = startingPos - hoverDistance;
        yBounds.y = startingPos + hoverDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (goingUp)
        {
            if (transform.position.y < yBounds.y)
            {
                Vector3 modifier = transform.position;
                modifier.y += hoverSpeed;
                transform.position = modifier;
            }

            else
                goingUp = false;
        }

        else
        {
            if (transform.position.y > yBounds.x)
            {
                Vector3 modifier = transform.position;
                modifier.y -= hoverSpeed;
                transform.position = modifier;
            }

            else
                goingUp = true;
        }


    }
}
