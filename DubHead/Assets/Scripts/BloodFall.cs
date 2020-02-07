using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodFall : MonoBehaviour
{
    public float fallingSpeed;
    public float collapseSpeed;
    public GameObject impactParticlePrefab;

    private GameObject impactParticle;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().gravityScale = fallingSpeed;
    }


    public void StartFall()
    {
        // unfreeze y so starts falling
        GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        GetComponent<Rigidbody2D>().WakeUp();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // when hit the ground freeze the movement and start collapsing
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Vector3 modifier = transform.position;
            modifier.z -= 1;

            impactParticle = Instantiate(impactParticlePrefab, modifier, Quaternion.identity);

            GetComponent<Rigidbody2D>().constraints |= RigidbodyConstraints2D.FreezePositionY;
            StartCoroutine(Collapse());
        }
    }

    // collapses to give waterfall impression
    IEnumerator Collapse()
    {
        // scale down, pivot is at bottom so will scale from top down
        while (transform.localScale.y > 0.2)
        {
            if(transform.localScale.y < 0.5)
                Destroy(impactParticle);

            Vector3 scaler = transform.localScale;
            scaler.y -= collapseSpeed;
            transform.localScale = scaler;

            yield return null;
        }

        // destroy after fully collapsed
        Destroy(gameObject);
    }
}
