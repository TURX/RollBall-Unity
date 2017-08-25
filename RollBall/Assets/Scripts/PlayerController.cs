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
    public Rigidbody pickup1;
    public Rigidbody pickup2;
    public Rigidbody pickup3;
    public Rigidbody pickup4;
    public Rigidbody pickup5;
    public Rigidbody pickup6;
    public Rigidbody pickup7;
    public Rigidbody pickup8;

    private Rigidbody rb;
    private int count;

    bool win = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winText.text = "";
        quitButton.gameObject.SetActive(false);
        pickup1.position = new Vector3(RandomNum() % 9, 0.5f, RandomNum() % 9);
        pickup2.position = new Vector3(RandomNum() % 9, 0.5f, RandomNum() % 9);
        pickup3.position = new Vector3(RandomNum() % 9, 0.5f, RandomNum() % 9);
        pickup4.position = new Vector3(RandomNum() % 9, 0.5f, RandomNum() % 9);
        pickup5.position = new Vector3(RandomNum() % 9, 0.5f, RandomNum() % 9);
        pickup6.position = new Vector3(RandomNum() % 9, 0.5f, RandomNum() % 9);
        pickup7.position = new Vector3(RandomNum() % 9, 0.5f, RandomNum() % 9);
        pickup8.position = new Vector3(RandomNum() % 9, 0.5f, RandomNum() % 9);
    }

    int RandomNum()
    {
        byte[] randomBytes = new byte[4];
        RNGCryptoServiceProvider rngCrypto =
        new RNGCryptoServiceProvider();
        rngCrypto.GetBytes(randomBytes);
        int rngNum = BitConverter.ToInt32(randomBytes, 0);//此为随机数
        return rngNum;
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
            count = count + 1;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Collected: " + count.ToString();
        if (count >= 8)
        {
            winText.text = "You Win!";
            noticeText.text = "A great job.";
            win = true;
            quitButton.gameObject.SetActive(true);
            countText.gameObject.SetActive(false);
        }
    }
}