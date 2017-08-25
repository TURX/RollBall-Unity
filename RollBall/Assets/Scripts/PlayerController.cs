using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float speedtouch;
    public Text countText;
    public Text winText;
    public Text noticeText;
    public Button quitButton;

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
            rb.AddForce(movementouch * speedtouch);            
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
        }
    }
}