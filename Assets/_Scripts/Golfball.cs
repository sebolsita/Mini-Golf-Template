using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class Golfball : MonoBehaviour
{
    public float forceAmount;
    Vector3 startPosition;
    public float resetDistance;

    private Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        //cache start position
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, startPosition) > resetDistance)
        {
            ResetPosition();
        }
    }

    [Button]
    private void ResetPosition()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        transform.position = startPosition;
    }

    [Button]
    private void AddForce()
    {
        _rigidbody.AddRelativeForce(Vector3.forward * forceAmount);
    }

    public void Hit(Vector3 hitVector, float hitVelocity)
    {
        //Debug.Log("Hit with force: " + hitVelocity);

        _rigidbody.AddForce(hitVector * hitVelocity);
    }

  
}