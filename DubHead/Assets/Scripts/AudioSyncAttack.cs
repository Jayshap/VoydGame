using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioSyncAttack : MonoBehaviour
{
    public string description;
    public AudioSource audioSource;

    public bool addMode;
    public bool debugMode;
    public float audioStartTime;
    public float adjustmentTime = 0.0f;

    public UnityEvent attackFunction;
    public List<float> times;

    public List<Vector2> ignoreTimes;


    private int nextTime = 0;
    private int nextIgnoreTime = 0;
    private float checkTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (addMode || debugMode)
        {
            audioSource.time = audioStartTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(debugMode)
        {
            Debug.Log(audioSource.time);
        }

        // iff add mode add the time on b keybind
        if (addMode)
        {
            Debug.Log(audioSource.time);

            if (Input.GetKeyDown(KeyCode.B))
                times.Add(audioSource.time);
        }
        // if play mode and have more attack times
        else if (!addMode && (nextTime < times.Count))
        {
            // calculate attack time with adjustmenttime
            checkTime = times[nextTime] + adjustmentTime;

            // if time to attack
            if((Mathf.Abs(checkTime - audioSource.time) < 0.1))
            {
                // check for ignore times
                if (nextIgnoreTime < ignoreTimes.Count)
                {
                    // if we are before the next ignore time than can attack
                    if (times[nextTime] < ignoreTimes[nextIgnoreTime].x)
                    {
                        attackFunction.Invoke();
                    }
                }
                // if no ignore times left can attack no matter what
                else
                {
                    // Debug.Log(audioSource.time);
                    attackFunction.Invoke();
                }

                // move onto next attack
                nextTime++;
            }
        }

        // if theres still ignore times left
        if (nextIgnoreTime < ignoreTimes.Count)
        {
            // if past end of ignore time go to next ignore time
            if (audioSource.time >= ignoreTimes[nextIgnoreTime].y)
            {
                nextIgnoreTime++;
            }
        }


    }
}
