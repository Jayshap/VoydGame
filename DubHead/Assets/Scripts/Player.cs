using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health;

    public AnimationClip hitAnimation;

    private float hitAnimationTime;

    // Start is called before the first frame update
    void Start()
    {
        hitAnimationTime = hitAnimation.length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if hits an attack collider take damage
        if(collision.gameObject.layer == LayerMask.NameToLayer("AttackCollider"))
        {
            takeDamage();
        }
    }


    public void takeDamage()
    {
        // set bool for animator
        GetComponent<Animator>().SetBool("Hit", true);

        //decrease health
        health -= 1;

        // start timer until hit animation finished
        StartCoroutine(resetHit());
    }

    IEnumerator resetHit()
    {
        yield return new WaitForSeconds(hitAnimationTime);

        // set bool for animator back to false
        GetComponent<Animator>().SetBool("Hit", false);
    }
}
