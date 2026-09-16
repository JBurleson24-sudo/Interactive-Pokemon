using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    public float speedFactor = 0.1f;

    public Transform anchorToMoveTo;

    public AudioSource slide;



    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, anchorToMoveTo.position, speedFactor);
    }

    public void setAnchor(Transform anchor)
    {
        slide.Play();

        anchorToMoveTo = anchor;
    }
}
