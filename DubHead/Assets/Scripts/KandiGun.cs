using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KandiGun : MonoBehaviour
{
    public int bulletDamage;
    public float bulletSpeed;
    public float timeBetweenShots;

    private bool canShoot;

    public Joystick joystick;
    private float threshhold = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        canShoot = true;
    }

    // Late update so that input happens first then shoot
    void Update()
    {
        //if (Input.GetKey(KeyCode.X) && canShoot)
        //{
        //    StartCoroutine(Shoot(timeBetweenShots));
        //}

        if ((joystick.Horizontal > threshhold || joystick.Vertical > threshhold) && canShoot)
        {
            StartCoroutine(Shoot(timeBetweenShots));
        }
    }

    IEnumerator Shoot(float tbs)
    {
        canShoot = false;

        Vector2 direction = new Vector2(joystick.Horizontal, joystick.Vertical);
        direction.Normalize();

        //// get which way player is facing to shoot that direction
        //switch (SceneImportants.Instance.player.GetComponent<PlayerController>().facing)
        //{
        //    case PlayerController.Direction.L:
        //        direction = new Vector2(-1, 0);
        //        direction.Normalize();
        //        break;

        //    case PlayerController.Direction.LU:
        //        direction = new Vector2(-1, 1);
        //        direction.Normalize();
        //        break;

        //    case PlayerController.Direction.U:
        //        direction = new Vector2(0, 1);
        //        direction.Normalize();
        //        break;

        //    case PlayerController.Direction.RU:
        //        direction = new Vector2(1, 1);
        //        direction.Normalize();
        //        break;

        //    case PlayerController.Direction.R:
        //        direction = new Vector2(1, 0);
        //        direction.Normalize();
        //        break;

        //    default:
        //        break;
        //}

        GameObject projectile = Instantiate(Resources.Load("Prefabs/KandiBullet"), gameObject.transform.position, Quaternion.identity) as GameObject;
        projectile.GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value, 1.0f);
        projectile.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

        yield return new WaitForSeconds(tbs);

        canShoot = true;
    }

}