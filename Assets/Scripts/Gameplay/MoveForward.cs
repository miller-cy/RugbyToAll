using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed;  // Speed at which the object moves
    public int dir;  // Direction in which the object moves
    private Vector2 startPos;  // Starting position of the object

    private void Start()
    {
        startPos = transform.position;
        transform.localScale = new Vector3(dir, 1, 1);  // Initialize startPos with the object's initial position
    }

    private void Update()
    {
        if (Mathf.Abs(transform.position.x - startPos.x) > 25)
        {
            Destroy(this.gameObject);  // Destroy the game object if it moves more than 15 units away from the start position on the x-axis
        }
        Move();  // Call the Move method to move the object
    }

    private void Move()
    {
        transform.position += transform.right * dir * speed * Time.deltaTime;
    }
}
