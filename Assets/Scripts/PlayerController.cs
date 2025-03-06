using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : NetworkBehaviour
{
    public UIManager uIManager;
    public GameManager gameManager;
    public float jumpForce;
    public float moveSpeed;
    private Rigidbody2D rB2D;
    private Vector2 moveDirection;
    private bool isGrounded;
    public TextMeshProUGUI playerNumText;
    public NetworkVariable<int> playerNum = new(1,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    private SpriteRenderer sprite;
    private Animator playerAnim;
    private bool isFacingLeft;
    public bool hasLost;
    public NetworkVariable<int> score = new(0,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    // Start is called before the first frame update

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        rB2D = gameObject.GetComponent<Rigidbody2D>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        playerAnim = gameObject.GetComponent<Animator>();
        uIManager = FindObjectOfType<UIManager>();
        gameManager = FindObjectOfType<GameManager>();
        hasLost = false;
        rB2D.bodyType = RigidbodyType2D.Dynamic;
        gameManager.numberOfPlayers += 1;
        if(IsServer)playerNum.OnValueChanged += NumChanged;
        if(IsHost)
        {
            playerNum.Value = 1;
            transform.position = gameManager.p1Point.position;
            playerNumText.text = string.Format("P{0}", playerNum.Value);
        }
        if(IsOwner && !IsHost)
        {
            playerNum.Value = 2;
            transform.position = gameManager.p2Point.position;
            playerNumText.text = string.Format("P{0}", playerNum.Value);
        }
    }

    void Start()
    {
        playerAnim.SetBool("isIdle",false);
    }

    void FixedUpdate()
    {
        rB2D.velocity = new Vector2(moveDirection.x * moveSpeed * Time.deltaTime, rB2D.velocity.y);
        playerAnim.SetFloat("yVelocity", rB2D.velocity.y);
        playerAnim.SetBool("isGrounded",isGrounded);
        if(moveDirection.x < 0)
        {
            isFacingLeft = true;
        }
        else if(moveDirection.x > 0)
        {
            isFacingLeft = false;
        }
        if(isFacingLeft)
        {
            transform.localScale = new Vector3(-1,1,1);
        }
        else
        {
            transform.localScale = new Vector3(1,1,1);
        }
    }

    void OnMove(InputValue movementValue)
    {
        if(IsOwner)
        {
            Vector2 moveVector2 = movementValue.Get<Vector2>();
            moveDirection.x = moveVector2.x;
            if(moveDirection.x < 0)
            {
                isFacingLeft = true;
                playerAnim.SetBool("isIdle",false);
            }
            else if(moveDirection.x > 0)
            {
                isFacingLeft = false;
                playerAnim.SetBool("isIdle",false);
            }
            else
            {
                playerAnim.SetBool("isIdle",true);
            }
        }
    }

    void OnJump()
    {
        if(IsOwner)
        {
            if(isGrounded)
            {
                rB2D.AddForce(transform.up * jumpForce);
                playerAnim.SetTrigger("jump");
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player")
        {
            isGrounded = true;
        }
    }

    void  OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player")
        {
            isGrounded =false;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player")
        {
            isGrounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player")
        {
            isGrounded =false;
        }
    }

    private void NumChanged(int prevVal, int newVal)
    {
        playerNumText.text = string.Format("P{0}", playerNum.Value);
    }
}

