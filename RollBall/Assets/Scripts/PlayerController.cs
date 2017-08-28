using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;
using System;

public class PlayerController : MonoBehaviour
{

    public float key_speed;
    public float touch_speed;
    public int orgCountofbricks;
    public int maxlevels;
    public Text countText;
    public Text winText;
    public Text noticeText;
    public Text levelText;
    public Text timeText;
    public Transform PickUpPrefab;
    public GameObject Walls;

    private Rigidbody rb;
    private int count;
    private int level = 0;
    private int countofbricks;

    bool win = false;
    bool allwin = false;
    bool fail = false;

    long time;
    long org_time;
    long now_time;
    private System.Timers.Timer timer = new System.Timers.Timer(1000);

    void OnGUI()
    {
        if (win)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 15, 100, 30), "Quit Game"))
            {
                Application.Quit();
            }
            if (!allwin && GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 30, 100, 30), "Next Level"))
            {
                NextLevel();
            }
        }
        if (fail)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 30, 100, 30), "Retry this level"))
            {
                ReTry();
            }
        }
    }

    void Start()
    {
        countofbricks = orgCountofbricks;
        timeText.color = Color.green;
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winText.gameObject.SetActive(false);
        levelText.text = "Level: 0";
        for (int t = 0; t < countofbricks; t++) Instantiate(PickUpPrefab, new Vector3(RandomNum() % 9, 0.5f, RandomNum() % 9), Quaternion.identity);
        time = 30;
        now_time = time;
        timer.Elapsed += new System.Timers.ElapsedEventHandler(CountDown);
        timer.Start();
        org_time = countofbricks * 2;
    }

    void Update()
    {
        #region FellIntoVoid
        if (rb.position.y < -20)
        {
            timer.Stop();
            Die("You fell into void.");
        }
        #endregion
        #region TimeOut
        timeText.text = "Countdown: " + time.ToString() + "s.";
        if (time <= 0)
        {
            timer.Stop();
            timeText.color = Color.red;
            timeText.fontStyle = FontStyle.Bold;
            Die("Time out.");
        }
        #endregion
    }

    int RandomNum()
    {
        byte[] randomBytes = new byte[4];
        RNGCryptoServiceProvider rngCrypto = new RNGCryptoServiceProvider();
        rngCrypto.GetBytes(randomBytes);
        int rngNum = BitConverter.ToInt32(randomBytes, 0);
        return rngNum;
    }

    void NextLevel()
    {
        Walls.SetActive(false);
        winText.gameObject.SetActive(false);
        noticeText.gameObject.SetActive(false);
        countText.gameObject.SetActive(true);
        levelText.gameObject.SetActive(true);
        win = false;
        count = 0;
        level++;
        countofbricks += (level * countofbricks);
        for (int t = 0; t < countofbricks; t++) Instantiate(PickUpPrefab, new Vector3(RandomNum() % 9, 0.5f, RandomNum() % 9), Quaternion.identity);
        org_time *= level + 1;
        time = org_time;
        now_time = time;
        timer.Start();
    }

    void ReTry()
    {
        fail = false;
        win = false;
        allwin = false;
        timeText.color = Color.green;
        winText.color = Color.yellow;
        rb.MovePosition(new Vector3(0, 0, 0));
        rb = GetComponent<Rigidbody>();
        levelText.gameObject.SetActive(true);
        noticeText.gameObject.SetActive(false);
        winText.gameObject.SetActive(false);
        time = now_time;
        timer.Start();
        org_time = countofbricks * 2;
    }

    void FixedUpdate()
    {
        #region Keyboard
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        if (win == false) rb.AddForce(movement * key_speed);
        #endregion
        #region Touchscreen
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            Vector3 movementouch = new Vector3(-touchDeltaPosition.x, 0.0f, -touchDeltaPosition.y);
            if (win == false) rb.AddForce(movementouch * touch_speed);
        }
        #endregion
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up") && !fail)
        {
            Destroy(other.gameObject);
            count++;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Collected: " + count.ToString() + " of " + countofbricks.ToString();
        if (count >= countofbricks && !fail)
        {
            timer.Stop();
            Walls.SetActive(true);
            winText.gameObject.SetActive(true);
            winText.text = "You won level " + level.ToString() + "!";
            noticeText.text = "A great job.";
            levelText.text = "Level: " + (level + 1).ToString();
            win = true;
            countText.gameObject.SetActive(false);
            levelText.gameObject.SetActive(false);
            if (level >= maxlevels) AllWin();
        }
    }

    void CountDown(object source, System.Timers.ElapsedEventArgs e)
    {
        time--;
    }

    void AllWin()
    {
        winText.gameObject.SetActive(true);
        winText.text = "You passed all the levels!";
        noticeText.text = "Congratulations!";
        win = true;
        allwin = true;
    }

    void Die(string reason)
    {
        fail = true;
        winText.color = Color.red;
        noticeText.color = Color.red;
        levelText.gameObject.SetActive(false);
        winText.gameObject.SetActive(true);
        noticeText.gameObject.SetActive(true);
        winText.text = "You died at level " + level.ToString() + ".";
        noticeText.text = reason;
        win = true;
        allwin = true;
    }
}