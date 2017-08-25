using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;
using System;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float speedtouch;
    public Text countText;
    public Text winText;
    public Text noticeText;
    public Button quitButton;
    public Button nextButton;
    public Transform PickUpPrefab;
    public int countofbricks;
    public Text levelText;
    public int maxlevels;

    private Rigidbody rb;
    private int count;
    private int level = 0;

    bool win = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        quitButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
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
        quitButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
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
        if (win == false) rb.AddForce(movement * speed);
        #endregion
        #region Touchscreen
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            Vector3 movementouch = new Vector3(-touchDeltaPosition.x, 0.0f, -touchDeltaPosition.y);
            if (win == false) rb.AddForce(movementouch * speedtouch);
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
            quitButton.gameObject.SetActive(true);
            countText.gameObject.SetActive(false);
            levelText.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(true);
            if (level >= maxlevels) AllWin();
        }
    }

    void AllWin()
    {
        winText.gameObject.SetActive(true);
        winText.text = ("You passed all the levels!");
        noticeText.text = "Congratulations!";
        win = true;
        quitButton.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(false);
    }
}