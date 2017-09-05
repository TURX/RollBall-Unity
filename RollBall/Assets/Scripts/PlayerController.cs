using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float key_speed;
    public float touch_speed;
    public float vbutton_speed;
    public int orgCountofbricks;
    public int maxlevels;
    public int CountofBlocks;
    public int orgCountofDanPick;
    public int bloodAll;
    public Text countText;
    public Text winText;
    public Text noticeText;
    public Text levelText;
    public Text timeText;
    //public Text dieCountText;
    public Text bloodText;
    public Transform PickUpPrefab;
    public Transform BlockPrefab;
    public Transform DangerousPick;
    public GameObject Walls;
    public Camera PIP;

    private Rigidbody rb;
    private int count;
    private int level = 0;
    private int countofbricks;
    private int CountofDanPick;
    //private int dietime = 0;
    private int blood;

    private bool win = false;
    private bool allcomplete = false;
    private bool fail = false;
    private bool allfail = false;
    private bool levelock = false;
    private bool[,] area = new bool[20, 20];

    private long time;
    private long org_time;
    private long now_time;
    private System.Timers.Timer timer = new System.Timers.Timer(1000);

    void OnGUI()
    {
        if (win)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 15, 100, 30), "Quit Game"))
            {
                Application.Quit();
            }
            if (!allcomplete && GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 30, 100, 30), "Next Level"))
            {
                NextLevel();
            }
        }
        if (fail)
        {
            if (!allfail && GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 30, 100, 30), "Retry this level"))
            {
                ReTry();
            }
        }
        #region On-Screen Controller
        if (win == false && (SystemInfo.deviceType == DeviceType.Handheld || SystemInfo.operatingSystem.Contains("Android") || SystemInfo.operatingSystem.Contains("iOS"))) // 
        {
            if (GUI.Button(new Rect(Screen.width - 60, Screen.height - 60, 30, 30), "○") && rb.position.y == 0.5f)
            {
                rb.AddForce(new Vector3(0f, 250f, 0f));
            }
            /*
            if (GUI.RepeatButton(new Rect(Screen.width - 60, Screen.height - 90, 30, 30), "↑"))
            {
                rb.AddForce(new Vector3(0f, 0f, 1f) * vbutton_speed);
            }
            if (GUI.RepeatButton(new Rect(Screen.width - 60, Screen.height - 30, 30, 30), "↓"))
            {
                rb.AddForce(new Vector3(0f, 0f, -1f) * vbutton_speed);
            }
            if (GUI.RepeatButton(new Rect(Screen.width - 90, Screen.height - 60, 30, 30), "←"))
            {
                rb.AddForce(new Vector3(-1f, 0f, 0f) * vbutton_speed);
            }
            if (GUI.RepeatButton(new Rect(Screen.width - 30, Screen.height - 60, 30, 30), "→"))
            {
                rb.AddForce(new Vector3(1f, 0f, 0f) * vbutton_speed);
            }
            if (GUI.RepeatButton(new Rect(Screen.width - 30, Screen.height - 90, 30, 30), "↗"))
            {
                rb.AddForce(new Vector3(1f, 0f, 1f) * vbutton_speed);
            }
            if (GUI.RepeatButton(new Rect(Screen.width - 90, Screen.height - 90, 30, 30), "↖"))
            {
                rb.AddForce(new Vector3(-1f, 0f, 1f) * vbutton_speed);
            }
            if (GUI.RepeatButton(new Rect(Screen.width - 30, Screen.height - 30, 30, 30), "↘"))
            {
                rb.AddForce(new Vector3(1f, 0f, -1f) * vbutton_speed);
            }
            if (GUI.RepeatButton(new Rect(Screen.width - 90, Screen.height - 30, 30, 30), "↙"))
            {
                rb.AddForce(new Vector3(-1f, 0f, -1f) * vbutton_speed);
            }
            */
        }
        #endregion
    }

    void Start()
    {
        winText.gameObject.SetActive(true);
        countText.gameObject.SetActive(true);
        levelText.gameObject.SetActive(true);
        bloodText.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true);
        noticeText.gameObject.SetActive(true);
        PIP.gameObject.SetActive(true);

        countofbricks = orgCountofbricks;
        CountofDanPick = orgCountofDanPick;
        blood = bloodAll;
        timeText.color = Color.green;
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winText.gameObject.SetActive(false);
        levelText.text = "Level: 0";
        bloodText.text = "HP: " + blood + "/" + bloodAll;

        Array.Clear(area, 0, area.Length);

        for (int t = 0; t < CountofBlocks; t++)
        {
            regenerate_blocks:
            int rand1 = 0;
            for (; rand1 == 0;) rand1 = RandomNum() % 4;
            int rand2 = 0;
            for (; rand2 == 0;) rand2 = RandomNum() % 4;
            if (rand1 != 1 && rand2 != 1) goto regenerate_blocks;
            BlockPrefab.transform.localScale = new Vector3(rand1 - 0.5f, 10f, rand2 - 0.5f);
            int Block_x = RandomNum() % 10;
            int Block_z = RandomNum() % 10;
            for (int t2 = -rand1; t2 < rand1; t2++)
            {
                //Debug.Log(Block_x + 10 + t2 + " " + Block_z + 10);
                area[Block_x + 10 + t2, Block_z + 10] = true;
            }
            for (int t2 = -rand2; t2 < rand2; t2++)
            {
                //Debug.Log(Block_x + 10 + " " + (Block_z + 10 + t2).ToString() + " " + t2);
                area[Block_x + 10, Block_z + 10 + t2] = true;
            }
            // 还是会卡墙里，但似乎不会卡进红色方块
            Instantiate(BlockPrefab, new Vector3(Block_x, 5f, Block_z), Quaternion.identity);
        }
        for (int t = 0; t < CountofDanPick; t++)
        {
            int DanPick_x = RandomNum() % 10;
            int DanPick_z = RandomNum() % 10;
            danpick_regenerate:
            if (area[DanPick_x + 10, DanPick_z + 10] == false)
            {
                Instantiate(DangerousPick, new Vector3(DanPick_x, 0.5f, DanPick_z), Quaternion.identity);
                area[DanPick_x + 10, DanPick_z + 10] = true;
            }
            else
            {
                DanPick_x = RandomNum() % 10;
                DanPick_z = RandomNum() % 10;
                goto danpick_regenerate;
            }
        }
        for (int t = 0; t < countofbricks; t++)
        {
            int Brick_x = RandomNum() % 10;
            int Brick_z = RandomNum() % 10;
            brick_regenerate:
            if (area[Brick_x + 10, Brick_z + 10] == false)
            {
                Instantiate(PickUpPrefab, new Vector3(Brick_x, 0.5f, Brick_z), Quaternion.identity);
            }
            else
            {
                Brick_x = RandomNum() % 10;
                Brick_z = RandomNum() % 10;
                goto brick_regenerate;
            }
        }

        for (int t1 = 0; t1 < 20; t1++)
        {
            for (int t2 = 0; t2 < 20; t2++)
            {
                Debug.Log(area[t1, t2] + " from " + t1 + "," + t2);
            }
            Debug.Log(t1);
        }
        Debug.Break();

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
        timeText.text = "Countdown: " + time.ToString() + "s";
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
        levelock = false;
        Walls.SetActive(false);
        winText.gameObject.SetActive(false);
        noticeText.gameObject.SetActive(false);
        countText.gameObject.SetActive(true);
        levelText.gameObject.SetActive(true);
        PIP.gameObject.SetActive(true);
        win = false;
        count = 0;
        level++;
        countofbricks += (level * countofbricks);
        for (int t = 0; t < countofbricks; t++) Instantiate(PickUpPrefab, new Vector3(RandomNum() % 9, 0.5f, RandomNum() % 9), Quaternion.identity);
        org_time = Convert.ToInt32(countofbricks * (maxlevels - level + 1));
        time = org_time;
        now_time = time;
        timer.Start();
    }

    void ReTry()
    {
        levelock = false;
        PIP.gameObject.SetActive(true);
        //dietime++;
        //dieCountText.gameObject.SetActive(true);
        //if (dietime == 1) dieCountText.text = "You died for 1 time"; else dieCountText.text = "You died for " + dietime.ToString() + " times";
        fail = false;
        win = false;
        allcomplete = false;
        timeText.color = Color.green;
        winText.color = Color.yellow;
        rb.MovePosition(new Vector3(0, 0, 0));
        rb = GetComponent<Rigidbody>();
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
        if (win == false)
        {
            if (Input.GetKeyDown(KeyCode.Space) && rb.position.y == 0.5f) rb.AddForce(new Vector3(0f, 250f, 0f));
            rb.AddForce(movement * key_speed);
        }
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
        if (other.gameObject.CompareTag("Dangerous Pick") && !fail && !win)
        {
            Destroy(other.gameObject);
            timer.Stop();
            Die("You died for pick an dangerous pickup.");
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
            PIP.gameObject.SetActive(false);
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
        allcomplete = true;
    }

    void Die(string reason)
    {

        fail = true;
        PIP.gameObject.SetActive(false);
        winText.color = Color.red;
        noticeText.color = Color.red;
        winText.gameObject.SetActive(true);
        noticeText.gameObject.SetActive(true);
        winText.text = "You died at level " + level.ToString() + ".";
        win = true;
        allcomplete = true;
        if (levelock == false)
        {
            blood--;
            if (blood > 0)
            {
                noticeText.text = reason;
                bloodText.text = "HP: " + blood + "/" + bloodAll;
            }
            else
            {
                allfail = true;
                bloodText.color = Color.red;
                bloodText.text = "HP: " + blood + "/" + bloodAll;
                noticeText.text = reason + "\nAnd you have no blood.";
            }
            levelock = true;
        }
    }
}