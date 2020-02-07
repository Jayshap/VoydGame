using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSvddenDeath : MonoBehaviour
{
    public int health;
    public GameObject leftHand;
    public GameObject rightHand;

    [Header("BloodFall")]
    public int bloodFallIndicatorNumBlinks;
    public float bloodFallIndicatorBlinkTime;
    public float bloodFallHorizontalBoundsAdjustment;
    public GameObject bloodFallIndicatorPrefab;

    public GameObject bloodFallPrefab;

    public enum FallSide
    {
        Left = 1,
        Right = 2,
        Random = 3
    }

    [Header("EyeShot")]
    public float eyeShotSpeed;
    public GameObject eyeShotPrefab;

    private Vector2 bloodFallHorizontalBounds;
    private float screenMaxVertical;
    private Bounds screenBounds;

    [Header("Beginning Lerp")]
    public GameObject startMarker;
    public GameObject endMarker;
    public float totalLerpTime;
    public float startLerpTime;
    public AudioSource audioSource;

    private float currentLerpTime;
    private bool lerpFinished = false;
    ////////////////////////////////////
    // Start is called before the first frame update
    void Start()
    {
        transform.position = startMarker.transform.position;
        GetComponent<Hover>().enabled = false;
        Camera boundingCamera = SceneImportants.Instance.cam.GetComponent<Camera>();

        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = boundingCamera.orthographicSize * 2;
        screenBounds = new Bounds(boundingCamera.transform.position, new Vector3(cameraHeight * screenAspect, cameraHeight, 0));

        // calculate the left and right bounds that blood fall can be randomly generated 
        // CODE FOR ANYWHERE ON SCREEN
        //bloodFallHorizontalBounds.x = screenBounds.center.x - screenBounds.extents.x;
        //bloodFallHorizontalBounds.y = screenBounds.center.x + screenBounds.extents.x;

        // maximum y value of screen
        screenMaxVertical = screenBounds.center.y + screenBounds.extents.y;

        currentLerpTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!lerpFinished && audioSource.time >= startLerpTime)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > totalLerpTime)
            {
                currentLerpTime = totalLerpTime;
                lerpFinished = true;
                GetComponent<Hover>().enabled = true;
            }

            float percent = currentLerpTime / totalLerpTime;
            transform.position = Vector3.Lerp(startMarker.transform.position, endMarker.transform.position, percent);
        }

        // calculate the left and right bounds that blood fall can be randomly generated 
        // CODE FOR NEAR PLAYER
        bloodFallHorizontalBounds.x = SceneImportants.Instance.player.transform.position.x - bloodFallHorizontalBoundsAdjustment;
        bloodFallHorizontalBounds.y = SceneImportants.Instance.player.transform.position.x + bloodFallHorizontalBoundsAdjustment;

        if (Input.GetKeyDown(KeyCode.L))
        {
            Attack_EyeShot();
        }
    }

    public void Attack_EyeShot()
    {
        GameObject shootingHand;
        // if player on left side, shoot with left hand
        if(SceneImportants.Instance.player.transform.position.x < transform.position.x)
        {
            shootingHand = leftHand;
        }
        // right hand
        else
        {
            shootingHand = rightHand;
        }

        Vector3 direction = SceneImportants.Instance.player.transform.position - shootingHand.transform.position;
        direction.Normalize();

        Vector3 eyeShotSpawnPos = shootingHand.transform.position;
        eyeShotSpawnPos.z = eyeShotPrefab.transform.position.z;

        GameObject projectile = Instantiate(Resources.Load("Prefabs/EyeShot"), eyeShotSpawnPos, Quaternion.identity) as GameObject;
        projectile.GetComponent<Rigidbody2D>().velocity = direction * eyeShotSpeed;
    }

    public void Attack_BloodFall(int side)
    {
        StartCoroutine(BloodFall(side));
    }

    IEnumerator BloodFall(int side)
    {
        GameObject bloodFallObject;
        float xPos;

        // keep generating random positions until one will land in a good spot (even elevation)
        do
        {
            if((FallSide)side == FallSide.Right)
            {
                // generate x position for bloodfall on right side of player
                xPos = Random.Range(SceneImportants.Instance.player.transform.position.x, bloodFallHorizontalBounds.y);
            }
            else if((FallSide)side == FallSide.Left)
            {
                // generate x position for bloodfall on left side of player
                xPos = Random.Range(bloodFallHorizontalBounds.x, SceneImportants.Instance.player.transform.position.x);
            }
            else
            {
                // generate x position for bloodfall on either side of player
                xPos = Random.Range(bloodFallHorizontalBounds.x, bloodFallHorizontalBounds.y);
            }

            // spawn blood fall attack part
            Vector3 bloodFallSpawnPos = new Vector3(xPos, screenMaxVertical, bloodFallPrefab.transform.position.z);
            bloodFallObject = Instantiate(bloodFallPrefab, bloodFallSpawnPos, Quaternion.identity);

        } while (!CheckBloodFallLanding(ref bloodFallObject));


        // height of the indicator
        float indicatorHeight = bloodFallIndicatorPrefab.transform.GetComponent<SpriteRenderer>().bounds.extents.y;

        // spawn indicator at top of screen at random xPos position
        Vector3 indicatorSpawnPos = new Vector3(xPos, screenMaxVertical - indicatorHeight, 0);
        GameObject indicator = Instantiate(bloodFallIndicatorPrefab, indicatorSpawnPos, Quaternion.identity);
        indicator.GetComponent<Indicator>().blinkTime = bloodFallIndicatorBlinkTime;
        indicator.GetComponent<Indicator>().numBlinks = bloodFallIndicatorNumBlinks;



        // wait for indicator to stop blinking
        yield return new WaitForSeconds(bloodFallIndicatorBlinkTime * 2 * bloodFallIndicatorNumBlinks);

        // tell blood object ready to fall
        bloodFallObject.GetComponent<BloodFall>().StartFall();
    }


    bool CheckBloodFallLanding(ref GameObject bloodFallObject)
    {
        // width of the bloodFallObject
        float bloodFallObjectWidth = bloodFallObject.transform.GetComponent<SpriteRenderer>().bounds.extents.x;

        Vector3 leftSide = bloodFallObject.transform.position;
        leftSide.x -= bloodFallObjectWidth;

        Vector3 rightSide = bloodFallObject.transform.position;
        rightSide.x += bloodFallObjectWidth;

        float rayDistance = 500.0f;

        // check left side hit elevation
        RaycastHit2D leftSideHit = Physics2D.Raycast(leftSide, -Vector2.up, rayDistance, 1 << LayerMask.NameToLayer("Ground"));
        //Debug.DrawLine(leftSide, leftSideHit.point);

        // check left side hit elevation
        RaycastHit2D rightSideHit = Physics2D.Raycast(rightSide, -Vector2.up, rayDistance, 1 << LayerMask.NameToLayer("Ground"));
        //Debug.DrawLine(rightSide, rightSideHit.point);

        // if nothing hit or distance is too big then elevation different return false
        if (leftSideHit.collider == null || rightSideHit.collider == null || Mathf.Abs(leftSideHit.point.y - rightSideHit.point.y) > 0.1f)
        {
            Destroy(bloodFallObject);
            return false;
        }

        return true;
    }

}
