using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{  
     //public TerrainManager terrainManager;
    private enum Direction
    {
        Up, Right, Left
    }
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private PlayerInput playerInput;
    private BoxCollider2D coll;

    [Header("得分")]
    public int stepPoint;

    private int pointResult; 
    [Header("跳跃")]
    public float jumpDistance;
    private float moveDistance;
    private Vector2 destination;
    private Vector2 touchPostion;
    private Direction dir;
    private bool buttonHeld;
    private bool isJump;
    private bool canJump;
    private bool isDead;

    [Header("方向指示")]
    public SpriteRenderer singRenderer;
    public Sprite upSign;
    public Sprite leftSign;
    public Sprite rightSign;


    private RaycastHit2D[] result = new RaycastHit2D[2];


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        playerInput = GetComponent <PlayerInput>();
        coll = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {   
        if (isDead)
        {
            DisableInput();
            return;
        }
        if (canJump)
        {
            TriggerJump();
        }
    }

    private void FixedUpdate()
    {
        if (isJump)
            rb.position = Vector2.Lerp(transform.position, destination, 0.134f);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Water") && !isJump)
        {
            Vector2 origin = transform.position + Vector3.up * 0.1f;
            int hitCount = Physics2D.RaycastNonAlloc(origin, Vector2.down, result, 0.1f);
            bool inWater = true;
            foreach (var hit in result)
            {
                if (hit.collider == null) continue;


                if (hit.collider.CompareTag("Wood"))
                {
                    transform.parent = hit.collider.transform;
                    Debug.Log("在木板上");
                    inWater = false;
                }
            }
            if (inWater && !isJump)
            {
                Debug.Log("In Water Game Over!");
                isDead = true;
            }
        }

        if (other.CompareTag("Border") || other.CompareTag("Car"))
        {
            Debug.Log("GAME OVER!");
            isDead = true;
        }
        if (!isJump && other.CompareTag("Obstacle"))
        {
            Debug.Log("GAME OVER!");
            isDead = true;
        }
        if (isDead)
        {   //通知游戏结束
            EventHandler.CallGameOverEvent();
            coll.enabled = false;

        }
    }

   
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && !isJump)
        {
            moveDistance = jumpDistance;
            //Debug.Log("你好，跳跃: " + " " + moveDistance);
            destination = new Vector2(transform.position.x, transform.position.y + moveDistance);
            canJump = true;
            AudioManager .instance .SetJumpClip(0);
        }

        if(dir == Direction.Up && context.performed && !isJump)
        {
            pointResult += stepPoint;
        }
    }

    public void LongJump(InputAction.CallbackContext context)
    {
        if (context.performed && !isJump)
        {
            moveDistance = jumpDistance * 2;
            buttonHeld = true;
            AudioManager .instance .SetJumpClip(1);
            singRenderer .gameObject.SetActive(true);

        }
        if (context.canceled && buttonHeld && !isJump)
        {
            Debug.Log("long jump" + context);
            if(dir == Direction.Up)
              pointResult += stepPoint * 2;

            buttonHeld = false;
            destination = new Vector2(transform.position.x, transform.position.y + moveDistance);
            canJump = true;
            singRenderer .gameObject.SetActive(false);
        }
    }

    public void GetTouchPositon(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            touchPostion = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());

            var offset = ((Vector3)touchPostion - transform.position).normalized;

            if (Mathf.Abs(offset.x) <= 0.7f)
            {
                dir = Direction.Up;
                singRenderer .sprite = upSign ;
            }
            else if (offset.x < 0)
            {
                dir = Direction.Left;
               // if(transform.localScale.x == -1)
                   // singRenderer .sprite = rightSign ;
                //else
                    singRenderer .sprite = leftSign ;
            }
            else if (offset.x > 0)
            {
                dir = Direction.Right;
                if(transform.localScale.x == -1)
                    singRenderer .sprite = leftSign ;
                else
                    singRenderer .sprite = rightSign ;
            }
        }
    }

    /// <summary>
    /// 触发跳跃动画
    /// </summary>
    private void TriggerJump()
    {
        canJump = false;
        switch (dir)
        {
            case Direction.Up:
                anim.SetBool("isSide", false);
                destination = new Vector2(transform.position.x, transform.position.y + moveDistance);
                transform.localScale = Vector3.one;
                break;
            case Direction.Right:
                anim.SetBool("isSide", true);
                destination = new Vector2(transform.position.x + moveDistance, transform.position.y);
                transform.localScale = new Vector3(-1, 1, 1);
                break;
            case Direction.Left:
                anim.SetBool("isSide", true);
                destination = new Vector2(transform.position.x - moveDistance, transform.position.y);
                transform.localScale = Vector3.one;
                break;
        }
        anim.SetTrigger("Jump");
    }

    #region Animation Event
    public void JumpAnimationEvent()
    {   AudioManager .instance .PlayJumpFX();
        isJump = true;
        Debug.Log(dir);
        sr.sortingLayerName = "Front";
        transform.parent = null;
    }

    public void FinishJumpAnimationEvent()
    {
        isJump = false;
        sr.sortingLayerName = "Middle";

        if(dir==Direction.Up && !isDead)
        {
            //terrainManager.CheckPosition();

            EventHandler.CallGetPointEvent(pointResult);

            Debug .Log ("总得分:" + pointResult );
        }
    }
    #endregion

    private void DisableInput()
    {
      playerInput .enabled = false;
    }
}