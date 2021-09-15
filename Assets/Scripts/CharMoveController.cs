using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMoveController : MonoBehaviour
{
    [Header("Movement")]
    public float moveAccel;
    public float maxSpeed;

    [Header("Jump")]
    public float jumpAccel;

    [Header("Ground Raycast")]
    public float groundRaycastDistance;
    public LayerMask groundLayerMask;

    [Header("Camera")]
    public CameraMoveController gameCamera;

    [Header("Scoring")]
    public ScoreController score;
    public float scoreRatio;

    [Header("Game Over")]
    public GameObject gameOverScreen;
    public float fallPositionY;
    

    private Rigidbody2D rig;
    private Animator anim;
    private CharSoundController sound;
    private AudioSource gameBGM;
    
    
    private float lastPositionX;

    
    public bool isJumping;
    private bool isOnGround;

    
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sound = GetComponent<CharSoundController>();
        gameBGM = GetComponent<AudioSource>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        //read input
       if(Input.GetMouseButtonDown(0))
       {
           if(isOnGround)
           {
               isJumping = true;
               sound.PlayJump();
           }
       }

       //change animation
       anim.SetBool("isOnGround", isOnGround);

       //calculate score
       int distancePassed = Mathf.FloorToInt(transform.position.x - lastPositionX);
       int scoreIncrement = Mathf.FloorToInt(distancePassed / scoreRatio);

       if(scoreIncrement > 0)
       {
           score.IncreaseCurrentScore(scoreIncrement);
           lastPositionX += distancePassed;
       }

       //game over
       if(transform.position.y < fallPositionY)
       {
           GameOver();

           //play game over sound
           sound.GameOverSound();
           
       }
    }

    private void FixedUpdate() 
    {
        //raycast ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance, groundLayerMask);
        if(hit)
        {
            if(!isOnGround && rig.velocity.y <= 0)
            {
                isOnGround = true;
            }
        }
        else
        {
            isOnGround = false;
        }

        //calculate velocity vector
        Vector2 velocityVector = rig.velocity;

        if(isJumping)
        {
            velocityVector.y += jumpAccel;
            isJumping = false;
        }

        velocityVector.x = Mathf.Clamp(velocityVector.x + moveAccel * Time.deltaTime, 0.0f, maxSpeed);

        rig.velocity = velocityVector;
    }

    private void GameOver()
    {
        //set hi score
        score.FinishScoring();

        //stop camera movement
        gameCamera.enabled = false;

        //show game over
        gameOverScreen.SetActive(true);

        //disable this
        this.enabled = false;

        //stop game bgm
        gameBGM.Stop();
    }

    private void OnDrawGizmos() 
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundRaycastDistance), Color.white);    
    }

}//class
