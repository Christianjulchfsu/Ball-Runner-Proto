using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float movementSpeed = 5.0f;

    private Rigidbody mRigidbody;
    private Vector2 mMovement;

    private void Start() {
        mRigidbody = GetComponent<Rigidbody>();
        //parent.transform.position = transform.position;
    }

    private void FixedUpdate() {
        mMovement.x = Input.GetAxisRaw("Mouse X");
        mMovement.y = Input.GetAxisRaw("Mouse Y");

        Vector3 move = new Vector3(mMovement.x, 0.0f, mMovement.y) + Camera.main.transform.forward;
        move.Normalize();

        //Accelerate
        if (Input.GetMouseButton(0)) {
            mRigidbody.AddForce(Camera.main.transform.forward * movementSpeed);
        }

        //Decelerate
        if (Input.GetMouseButton(1)) {
            mRigidbody.velocity = Vector3.Lerp(mRigidbody.velocity,  Vector3.zero, 0.05f);
        }
    }
}
