using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfClub : MonoBehaviour
{

    // Golf ball script component
    private Golfball _golfBall;

    // Minimum velocity required to hit the ball
    public float minVelocity = 10f;

    //velocity multiplier
    public float velocityMultiplier = 10f;
    private Vector3 _previousPosition;

    [SerializeField]
    private float _golfClubVelocityMagnitude;
    public Vector3 GolfClubVelocity { get; private set; }
    private BoxCollider _boxCollider;

    //start is called before the first frame update
    private void Start()
    {   
        _boxCollider = GetComponent<BoxCollider>();
        PutterActive(true);
    }

    //fixed update is called every physics update
    private void Update()
    {
        GolfClubVelocity = (transform.position - _previousPosition) / Time.deltaTime;
        _previousPosition = transform.position;

        _golfClubVelocityMagnitude = GolfClubVelocity.magnitude;
               
    }

    // Apply a force to the golf ball based on the velocity of the club
    private void OnCollisionEnter(Collision collision)
    {
        _golfBall = collision.gameObject.GetComponent<Golfball>();

        if(_golfBall != null)
        {
            // Get the normal vector of the collision
            Vector3 hitDirection = collision.contacts[0].normal;
            hitDirection = -hitDirection;
            hitDirection.y = 0f;

            Debug.DrawRay(collision.contacts[0].point, hitDirection * 1, Color.red, 10f);

            // Calculate the power of the hit based on the velocity of the club
            // float hitPower = Mathf.Clamp(hitVelocity - minVelocity, 0f, 100f);

            // multiply the velocity by a multiplier
            _golfClubVelocityMagnitude *= velocityMultiplier;                

            // Apply a force to the golf ball in the direction of the normal vector
            _golfBall.Hit(hitDirection, _golfClubVelocityMagnitude);
        }

        _golfBall = null;

    }

    //disable box collider when the ball is hit
    private void OnCollisionExit(Collision collision)
    {
        //if (collision.gameObject == golfBall.gameObject)
        //{
        //  PutterActive(false);
        //}
    }

    //toggle putter on and off
    public void PutterActive(bool toggle)
    {
        //toggle the putter
        _boxCollider.enabled = toggle;
    }
}
