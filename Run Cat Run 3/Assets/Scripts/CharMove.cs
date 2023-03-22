using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMove : MonoBehaviour
{
    private float _currentTime;
    [SerializeField] private float _durationTime;
    Rigidbody rb;

    //public GameObject mist;

    Vector2 startTouchPosition;
    Vector2 endTouchPosition;
    Vector2 currentPosition;
    bool stopTouch = false;
    bool swipeLeft = false;
    bool swipeRight = false;
    bool swipeJump = false;
    bool tap = false;
    bool stopPrefab = false;

    public float swipeRange = 200.0f;
    public float tapRange = 20.0f;
    public float speed;
    public float jumpMultiplier;

    int i = 2;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _currentTime = _durationTime;
        InvokeRepeating("CallPrefab", 1.0f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-speed * Time.deltaTime, 0, 0);
        Swipe();

        if(i == 1)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, -1.5f), 3 * Time.deltaTime);
        }
        else if(i == 2)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, 0), 3 * Time.deltaTime);
        }
        else if( i == 3)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, 0), 3 * Time.deltaTime);
        }
        StartCoroutine(updateTime());

        if (swipeJump)
        {
            rb.velocity = Vector3.zero;
            rb.velocity = Vector3.up * jumpMultiplier;


            if (transform.position.y == 1.70f)
            {
                rb.velocity = Vector3.zero;
            }
        }



    }

    IEnumerator updateTime()
    {
        while (_currentTime >= 0)
        {
            yield return new WaitForSeconds(0.5f);
            _currentTime -= Time.deltaTime;
            if (_currentTime == 0)
            {
                stopPrefab = true;
            }
        }
    }

    void Swipe()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPosition = Input.GetTouch(0).position;
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            currentPosition = Input.GetTouch(0).position;
            Vector2 Distance = currentPosition - startTouchPosition;
            if (!stopTouch)
            {
                if (Distance.x < -swipeRange)
                {
                    swipeLeft = true;
                    stopTouch = true;


                }
                else if (Distance.x > swipeRange)
                {
                    swipeRight = true;
                    stopTouch = true;


                }
                else if (Distance.y > swipeRange)
                {
                    swipeJump = true;
                    stopTouch = true;

                }
            }
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            stopTouch = false;

            endTouchPosition = Input.GetTouch(0).position;

            Vector2 Distance = endTouchPosition - startTouchPosition;
            if (Mathf.Abs(Distance.x) < tapRange && Mathf.Abs(Distance.y) < tapRange)
            {
                tap = true;

            }
        }
        if (swipeRight == true && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            stopPrefab = false;
            i++;
            if (i == 4)
            {
                i = 3;
            }
            swipeRight = false;
        }
        else if (swipeLeft == true && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            stopPrefab = false;
            i--;
            if (i == 0)
            {
                i = 1;
            }
            swipeLeft = false;
        }
        else if (swipeJump == true && Input.GetTouch(0).phase == TouchPhase.Ended)
        {

            swipeJump = false;
        }

    }
    /*
    public void CallPrefab()
    {
        GameObject prefab = Instantiate(mist);
        prefab.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        if (stopPrefab == true)
        {
            CancelInvoke("CallPrefab");
        }
    }
    */
}
