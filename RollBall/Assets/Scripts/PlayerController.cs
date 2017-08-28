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
    public int countofbricks;
    public int maxlevels;
    public Text countText;
    public Text winText;
    public Text noticeText;
    public Text levelText;
    public Transform PickUpPrefab;

    private Rigidbody rb;
    private int count;
    private int level = 0;

    bool win = false;
    bool allwin = false;

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
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winText.gameObject.SetActive(false);
        levelText.text = "Level: 0";
        for (int t = 0; t < countofbricks; t++) Instantiate(PickUpPrefab, new Vector3(RandomNum() % 9, 0.5f, RandomNum() % 9), Quaternion.identity);
    }


    int RandomNum()
    {
        byte[] randomBytes = new byte[4];
        RNGCryptoServiceProvider rngCrypto = new RNGCryptoServiceProvider();
        rngCrypto.GetBytes(randomBytes);
        int rngNum = BitConverter.ToInt32(randomBytes, 0);//此为随机数
        return rngNum;
    }



    void NextLevel()
    {
        winText.gameObject.SetActive(false);
        noticeText.gameObject.SetActive(false);
        countText.gameObject.SetActive(true);
        levelText.gameObject.SetActive(true);
        win = false;
        count = 0;
        level++;
        countofbricks += (level * countofbricks);
        for (int t = 0; t < countofbricks; t++) Instantiate(PickUpPrefab, new Vector3(RandomNum() % 9, 0.5f, RandomNum() % 9), Quaternion.identity);
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
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Collected: " + count.ToString() + " of " + countofbricks.ToString();
        if (count >= countofbricks)
        {
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

    void AllWin()
    {
        winText.gameObject.SetActive(true);
        winText.text = ("You passed all the levels!");
        noticeText.text = "Congratulations!";
        win = true;
        allwin = true;
    }
}