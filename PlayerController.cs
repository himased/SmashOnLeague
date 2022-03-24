using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable, IPunObservable
{
    public static PlayerController Instance;

    [SerializeField] Image healthbarImage;
    [SerializeField] GameObject ui, getHit, dyingUI;

    [SerializeField] GameObject cameraHolder, endGame;

    [SerializeField] float mouseSensivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;

    public GameObject Shield;

    float verticalLookRotation;
    public float speedMovement;
    public bool grounded;
    Vector3 smoothVelocity;
    Vector3 moveAmount;

    public Rigidbody rb;

    public PhotonView PV;

    const float maxHealth = 100f;
    public float currentHealth = maxHealth;

    PlayerManager playerManager;

    public GameObject ball;
    int myTeam;

    Vector3 position;
    Quaternion rotation;

    //animation
    public Animator anim;
    public float animSpeed = 1f;
    public AnimatorStateInfo currentBaseState;         // state ปัจจุบันใน Animator
    float isIdle = 0;


    // ดึงค่าสถานะของ state ต่างๆ มาใส่เก็บเอวไว้ในตัวแปร
    // เพื่อเอาไปใช้อ้างอิงในโปรแกรม

    static int idleState = Animator.StringToHash("Base Layer.Idle");
    static public int locoState = Animator.StringToHash("Base Layer.Locomotion");
    static public int locoNoBallState = Animator.StringToHash("Base Layer.Locomotion 0");
    static public int jumpState = Animator.StringToHash("Base Layer.Jump");
    static public int idleJumpState = Animator.StringToHash("Base Layer.idleJump");
    static public int jumpBackState = Animator.StringToHash("Base Layer.Jump 0");
    static public int restState = Animator.StringToHash("Base Layer.Rest");
    static public int walkBackState = Animator.StringToHash("Base Layer.WalkBack");
    static public int airTimeState = Animator.StringToHash("Base Layer.AirTime");
    static public int airTimeBackState = Animator.StringToHash("Base Layer.AirTime 0");
    static public int rollBackState = Animator.StringToHash("Base Layer.JamRollMove 0");
    static public int rollState = Animator.StringToHash("Base Layer.JamRollMove");

    PlayerMapAreas playerMapArea;

    string myName, killerName = "";

    public SoundManager SM;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();

        if (PV.IsMine)
        {
            myName = PV.Owner.NickName;
        }

        endGame.SetActive(false);
        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(ui);
        }

        PV.RPC("OpenShield", RpcTarget.All);

        //SM.PlaySound("respawn");

        anim = GetComponent<Animator>();
        GetComponent<PlayerController>().enabled = false;
        getHit.SetActive(false);
        StuntController.Instance.isHit = false;
        StuntController.Instance.isStunt = false;
        anim.SetBool("IsHit", false);

        playerMapArea = GetComponent<PlayerMapAreas>();
    }

    private void Update()
    {
        if (!PV.IsMine)
            return;

        Look();
        Move();
        Jump();
        upSpeed();

        if (Input.anyKey && currentHealth > 0)
        {
            PV.RPC("CloseShield", RpcTarget.All);
        }

        PV.RPC("SentHealth", RpcTarget.All);

        if(currentHealth <= 0)
        {
            sprintSpeed = 0;
            walkSpeed = 0;
            mouseSensivity = 0;
            jumpForce = 0;
        }

        /*if (TimeController.isStart == true)
        {
            Die();
            TimeController.inRound = true;
            TimeController.isStart = false;
        }*/

        if(MapCaptureController.isEnd == true)
        {
            endGame.SetActive(true);
            GetComponent<PlayerController>().enabled = false;
            ball.GetComponent<BallController>().enabled = false;
        }

        if (TimeController.isEnd == true)
        {
            endGame.SetActive(true);
            GetComponent<PlayerController>().enabled = false;
            ball.GetComponent<BallController>().enabled = false;
        }

        if (transform.position.y < -10f)
        {
            StartCoroutine(WaitAndDie());
            Die();
        }

        if(PlayerManager.Instance.isTeamOne == true)
        {
            myTeam = 1;
        }
        if(PlayerManager.Instance.isTeamTwo == true)
        {
            myTeam = 2;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(gameObject.transform.position);
            stream.SendNext(gameObject.transform.rotation);
        }
        else
        {
            position = (Vector3)stream.ReceiveNext();
            rotation = (Quaternion)stream.ReceiveNext();
        }
    }

    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -30f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothVelocity, smoothTime);
       
        speedMovement = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            if (currentBaseState.fullPathHash == idleState)
            {
                // และไม่ได้อยู่ในช่วง transition คืออยู่ใน state เรียบร้อยแล้ว
                if (!anim.IsInTransition(0))
                {
                    // กระโดดดดดดดดดดไปในทิศทาง up (0,1,0) 
                    //rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
                    rb.AddForce(transform.up * jumpForce);
                    anim.SetBool("Jump", true);     // พร้อมเปลี่ยน animationไปอยู่ในทางกระโดด
                    SM.PlaySound("jump");
                }
            }

            if (currentBaseState.fullPathHash == locoState)
            {
                // และไม่ได้อยู่ในช่วง transition คืออยู่ใน state เรียบร้อยแล้ว
                if (!anim.IsInTransition(0))
                {
                    // กระโดดดดดดดดดดไปในทิศทาง up (0,1,0) 
                    //rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
                    rb.AddForce(transform.up * jumpForce);
                    anim.SetBool("Jump", true);     // พร้อมเปลี่ยน animationไปอยู่ในทางกระโดด
                    SM.PlaySound("jump");
                }
            }

            if (currentBaseState.fullPathHash == locoNoBallState)
            {
                // และไม่ได้อยู่ในช่วง transition คืออยู่ใน state เรียบร้อยแล้ว
                if (!anim.IsInTransition(0))
                {
                    // กระโดดดดดดดดดดไปในทิศทาง up (0,1,0) 
                    //rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
                    rb.AddForce(transform.up * jumpForce);
                    anim.SetBool("Jump", true);     // พร้อมเปลี่ยน animationไปอยู่ในทางกระโดด
                    SM.PlaySound("jump");
                }
            }

            if (currentBaseState.fullPathHash == walkBackState)
            {
                // และไม่ได้อยู่ในช่วง transition คืออยู่ใน state เรียบร้อยแล้ว
                if (!anim.IsInTransition(0))
                {
                    // กระโดดดดดดดดดดไปในทิศทาง up (0,1,0) 
                    //rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
                    rb.AddForce(transform.up * jumpForce);
                    anim.SetBool("WalkBackJump", true);     // พร้อมเปลี่ยน animationไปอยู่ในทางกระโดด
                    SM.PlaySound("jump");
                }
            }
            //rb.AddForce(transform.up * jumpForce);
        }
    }

    void upSpeed()
    {
        if(MapCaptureController.Instance.allCaptured == 1)
        {
            walkSpeed = 4.5f;
            sprintSpeed = 9;
        }
        if (MapCaptureController.Instance.allCaptured == 2)
        {
            walkSpeed = 5;
            sprintSpeed = 10;
        }
        /*if (MapCaptureController.Instance.allCaptured == 3)
        {
            walkSpeed = 6;
            sprintSpeed = 12;
        }*/
    }

    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine)
            return;
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);

        float h = Input.GetAxis("Horizontal");              // เก็บค่าในแนวแกนนอน ( -1 <= h <=1 ) 
        float v = Input.GetAxis("Vertical") * speedMovement;                // เก็บค่าในแนวแกนตั้ง (-1 <= v <= 1)  
        anim.SetFloat("Speed", v);                          // Speed <--- v 
        anim.SetFloat("Direction", h);                      // Direction <--- h 
        anim.speed = animSpeed;                             // กำหนด speed ของ Animator
        currentBaseState = anim.GetCurrentAnimatorStateInfo(0); // Base Layer(0)

        // check เงื่อนไขหรือสถานะต่างๆ ตามชอบ หรือตามความจำเป็น 
        // ระหว่างที่อยู่ในสถานะ jumpState คือลอยตัวอยู่กลางอากาศ 
        if (currentBaseState.fullPathHash == jumpState)
        {
            if (!anim.IsInTransition(0))
            {
                //  reset ค่า Jump --> false จะได้กลับไปเป็นทางเดินหรือวิ่งตอนกระโดดเสร็จ 
                anim.SetBool("Jump", false);
                /*if (!grounded)
                {
                    anim.SetBool("InAir", true);
                }*/
            }
            if(currentHealth <= 0)
            {
                anim.SetBool("IsDying", true);
            }
            if (grounded)
            {
                anim.SetBool("InAir", false);
            }
            anim.SetBool("Throwing", false);
        }
        // ระหว่างที่เป็น idleState คือยืนอยู่เฉยๆ 
        else if (currentBaseState.fullPathHash == idleState)
        {
            if (!anim.IsInTransition(0))
            {
                //  reset ค่า Jump --> false จะได้กลับไปเป็นทางเดินหรือวิ่งตอนกระโดดเสร็จ 
                anim.SetBool("Jump", false);
                /*if (!grounded)
                {
                    anim.SetBool("InAir", true);
                }*/
            }
            if (!grounded)
            {
                anim.SetBool("InAir", true);
            }
            if (Input.GetKeyDown(KeyCode.T)) // ถ้ากดปุ่ม space จะเปลี่ยนเป็นท่าบิดขี้เกียจ
            {
                anim.SetBool("Rest", true);
                SM.PlaySound("emote");
            }

            if (currentHealth <= 0)
            {
                anim.SetBool("IsDying", true);
            }

            if (Input.GetKeyUp(KeyCode.Mouse0) && BallController.slotFull == true)
            {
                anim.SetBool("Throwing", true);
            }
        }

        else if (currentBaseState.fullPathHash == locoState)
        {
            if (!grounded)
            {
                anim.SetBool("InAir", true);
            }
            if (Input.GetKeyUp(KeyCode.Mouse0) && BallController.slotFull == true)
            {
                anim.SetBool("Throwing", true);
            }
            if (currentHealth <= 0)
            {
                anim.SetBool("IsDying", true);
            }
            /*if(BallController.slotFull == false)
            {
                anim.SetBool("HandBall", false);
            }*/
        }

        else if (currentBaseState.fullPathHash == locoNoBallState)
        {
            if (!anim.IsInTransition(0))
            {
                //  reset ค่า Jump --> false จะได้กลับไปเป็นทางเดินหรือวิ่งตอนกระโดดเสร็จ 
                anim.SetBool("Jump", false);
                /*if (!grounded)
                {
                    anim.SetBool("InAir", true);
                }*/
            }
            if (!grounded)
            {
                anim.SetBool("InAir", true);
            }
            if (currentHealth <= 0)
            {
                anim.SetBool("IsDying", true);
            }
            /*if (BallController.slotFull == true)
            {
                anim.SetBool("HandBall", true);
            }*/
        }

        else if (currentBaseState.fullPathHash == restState)
        {
            if (!anim.IsInTransition(0))
            {
                // ปิดขี้เกียจเสร็จแล้ว ต้องให้กลับไปอยู่ท่า idle เหมือนเดิม ดังนั้นต้องเปลี่ยน Rest --> false
                anim.SetBool("Rest", false);
            }
            if (Input.GetKeyUp(KeyCode.Mouse0) && BallController.slotFull == true)
            {
                anim.SetBool("Throwing", true);
            }
            if (currentHealth <= 0)
            {
                anim.SetBool("IsDying", true);
            }
        }

        else if(currentBaseState.fullPathHash == walkBackState)
        {
            if (Input.GetKeyUp(KeyCode.Mouse0) && BallController.slotFull == true)
            {
                anim.SetBool("Throwing", true);
            }
            if (currentHealth <= 0)
            {
                anim.SetBool("IsDying", true);
            }
            /*if (!grounded)
            {
                anim.SetBool("InAir", true);
            }*/
            if (!anim.IsInTransition(0))
            {
                //  reset ค่า Jump --> false จะได้กลับไปเป็นทางเดินหรือวิ่งตอนกระโดดเสร็จ 
                anim.SetBool("WalkBackJump", false);
                if (!grounded)
                {
                    anim.SetBool("InAir", true);
                }
            }
            if (grounded)
            {
                anim.SetBool("InAir", false);
            }
        }

        else if(currentBaseState.fullPathHash == airTimeState)
        {
            if (Input.GetKeyUp(KeyCode.Mouse0) && BallController.slotFull == true)
            {
                anim.SetBool("Throwing", true);
            }
            if (currentHealth <= 0)
            {
                anim.SetBool("IsDying", true);
            }
        }

        if (BallController.slotFull == true)
        {
            anim.SetBool("HandBall", true);
            isIdle += 0.1f;
            if(isIdle >= 1)
            {
                isIdle = 1;
            }
        }
        if (BallController.slotFull == false)
        {
            anim.SetBool("HandBall", false);
            isIdle -= 0.1f;
            if (isIdle <= 0)
            {
                isIdle = 0;
            }
        }
        //ground
        if (grounded)
        {
            anim.SetBool("InAir", false);
        }

        anim.SetFloat("IsIdle", isIdle);
    }

    void IsHitFalse()
    {
        anim.SetBool("IsHit", false);
    }

    void ThrowSuccess()
    {
        anim.SetBool("Throwing", false);
        GetComponent<PlayerController>().enabled = true;
    }

    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.Others, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (!PV.IsMine)
            return;

        currentHealth -= damage;

        getHit.SetActive(true);
        StuntController.Instance.isHit = true;
        anim.SetBool("IsHit", true);
        StartCoroutine(GetHit());

        healthbarImage.fillAmount = currentHealth / maxHealth;

        if(currentHealth <= 0)
        {
            //rb.constraints = RigidbodyConstraints.None;
            //rb.AddForce(Random.Range(30, 50), Random.Range(5, 15), Random.Range(30, 50), ForceMode.Impulse);
            //GetComponent<PlayerController>().enabled = false;
            //ball.GetComponent<BallController>().enabled = false;
            PV.RPC("OpenShield", RpcTarget.All);
            Destroy(playerMapArea);
            dyingUI.SetActive(true);
            SM.PlaySound("die");


            StartCoroutine(WaitAndDie());
            PV.RPC("RPC_PlayerKilled", RpcTarget.AllBuffered, myTeam);
            //Die();
        }
    }

    private IEnumerator WaitAndDie()
    {
        yield return new WaitForSeconds(5f);
        Die();
    }

    public void Die()
    {
        playerManager.Die();
    }

    [PunRPC]
    void RPC_PlayerKilled(int team)
    {
        if(team == 2)
        {
            GameSetup.teamOneScore++;
        }
        if(team == 1)
        {
            GameSetup.teamTwoScore++;
        }
        //PV.RPC("RPC_SentScore", RpcTarget.OthersBuffered, GameSetup.teamOneScore, GameSetup.teamTwoScore);
    }

    /*[PunRPC]
    void RPC_SentScore(int _teamOneScore, int _teamTwoScore)
    {
        GameSetup.teamOneScore = _teamOneScore;
        GameSetup.teamTwoScore = _teamTwoScore;
    }*/

    void AfterSpawn()
    {
        GetComponent<PlayerController>().enabled = true;
    }

    [PunRPC]
    void OpenShield()
    {
        Shield.SetActive(true);
    }

    [PunRPC]
    void CloseShield()
    {
        Shield.SetActive(false);
    }

    IEnumerator GetHit()
    {
        //getHit.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        getHit.SetActive(false);
        StuntController.Instance.isHit = false;
    }

    //JFaii
    void endKick()
    {
        anim.SetBool("useSkill1", false);
    }

    [PunRPC]
    void SentHealth()
    {
        currentHealth = currentHealth / 1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ball" && collision.gameObject.GetComponent<PhotonView>().IsMine == false)
        {
            killerName = collision.gameObject.GetComponent<PhotonView>().Owner.NickName;
            if(myName != killerName)
            {
                //SM.PlaySound("hit");
            }
            SM.PlaySound("hit");
        }
    }
}
