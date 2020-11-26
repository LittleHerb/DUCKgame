using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DuckFlightScript : MonoBehaviour
{
    public float jumpForce = 1;
    public Vector2 xConstraints;
    public Vector2 yConstraints;
    public float maxGravity; // negative number
    float score = 0;
    public bool dead = false;
    public GameObject textScore;
    public GameObject deathMenu;
    public GameObject deathScore;
    public GameObject HUDRef;
    bool activated = false;
    public bool Activated
    {
        get
        {
            return activated;
        }
    }

    public virtual void Start()
    {
        Time.timeScale = 0;
        Debug.Log("Off");
    }

    public virtual void Update()
    {
        if (!dead)
        {
            Vector3 moveVector = Vector3.zero;
            //Up-down movement
            if (Input.GetKey(KeyCode.Space) || Input.GetTouch(0).phase == TouchPhase.Began)
            {
                GetComponent<Rigidbody>().velocity = new Vector3(0, jumpForce, 0);
                if (!activated)
                {
                    Time.timeScale = 1;
                    activated = true;
                    Debug.Log("On");
                }
            }

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, xConstraints.x, xConstraints.y), Mathf.Clamp(transform.position.y, yConstraints.x, yConstraints.y), 0);
            GetComponent<Rigidbody>().velocity = new Vector3(0, Mathf.Clamp(GetComponent<Rigidbody>().velocity.y, maxGravity, jumpForce), 0);

            //Score
            score += 1 * Time.deltaTime;
            if (textScore.GetComponent<Text>())
            {
                textScore.GetComponent<Text>().text = "Score: " + Mathf.RoundToInt(score);
            }
        }
    }

    //Run when player dies
    public void TargetHit()
    {
        dead = true;
        Time.timeScale = 0;
        deathMenu.SetActive(true);
        HUDRef.SetActive(false);
        if (deathScore.GetComponent<Text>())
        {
            deathScore.GetComponent<Text>().text = "Score: " + Mathf.RoundToInt(score);
        }
    }

    public void IncreaseScore(int increase)
    {
        score += increase;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        //Detects collision with enemies
        if (other.CompareTag("Obstacle"))
        {
            TargetHit();
        }
    }
}
