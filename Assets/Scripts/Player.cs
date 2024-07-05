using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController character;
    private Vector3 direction;

    public float gravity = 9.81f * 2f;

    public float jumpForce = 8f;
    public float duckHeightScale = 0.5f;

    private Vector3 ogScale;
    private Vector3 duckScale;
    private float originalCenter;
    private float duckCenter;
    private float ogHeight;
    private bool isDucking = false;


    private void Awake()
    {
        character = GetComponent<CharacterController>();

        ogScale = transform.localScale;
        duckScale = new Vector3(ogScale.x, ogScale.y * duckHeightScale, ogScale.z);
        ogHeight = character.height;
        originalCenter = character.radius;
        duckCenter = originalCenter * duckHeightScale;

    }
    private void OnEnable()
    {
        direction = Vector3.zero;
    }
    private void Update()
    {
        direction += Vector3.down * gravity * Time.deltaTime;

        if (character.isGrounded)
        {
            direction = Vector3.down;

            if (Input.GetButton("Jump") || (Input.GetKey(KeyCode.UpArrow)))
            {
                direction = Vector3.up * jumpForce;
            }
        }

        //Duck function
        if (Input.GetKeyDown(KeyCode.DownArrow)) 
        {
            Duck();
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            StandUp();
        }


         character.Move(direction * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            //trigger game over
            GameManager.Instance.GameOver();
        }
    }

    private void Duck()
    {
        isDucking = true;
        transform.localScale = duckScale;
        character.height = ogHeight * duckHeightScale;
        character.radius = duckCenter;
        
    }
    private void StandUp()
    {
        isDucking = false;
        transform.localScale = ogScale;
        character.height = ogHeight;
        character.radius = originalCenter;
    }
}
