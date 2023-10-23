using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingSimulator : MonoBehaviour
{
    public float swingSpeed;
    public bool swing;

    // Start is called before the first frame update
    void Start()
    {
        if(swing == false)
        {
            //reset rotation
            transform.rotation = Quaternion.identity;

            //move back 1 metre
            transform.Translate(Vector3.forward * 1f, Space.World);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //slowly rotate around x axis
        if (swing)
            transform.Rotate(Vector3.right * Time.deltaTime * swingSpeed, Space.World);
        else
            transform.Translate(-Vector3.forward * Time.deltaTime * swingSpeed, Space.World);
    }
}
