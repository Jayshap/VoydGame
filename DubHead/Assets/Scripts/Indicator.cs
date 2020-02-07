using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    public int numBlinks;
    public float blinkTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Blink());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Blink()
    {
        for(int i = 0; i < numBlinks; ++i)
        {   
            yield return new WaitForSeconds(blinkTime);

            GetComponent<SpriteRenderer>().enabled = false;

            yield return new WaitForSeconds(blinkTime);

            GetComponent<SpriteRenderer>().enabled = true;
        }

        Destroy(gameObject);
    }
}
