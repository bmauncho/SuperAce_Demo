using System.Collections;
using UnityEngine;

public class ScatterMotions : MonoBehaviour
{
    public bool isRotate;
    public bool isBounce;
    public float rotationSpeed = 360f; // Rotation speed in degrees per second
    private Coroutine rotateCoroutine;

    [ContextMenu("Rotate")]
    public void Rotate ()
    {
        GetComponent<Animator>().enabled = false;
        if (rotateCoroutine != null) // Stop the existing rotate coroutine if it's running
        {
            StopCoroutine(rotateCoroutine);
        }
        isRotate = true;
        isBounce = false;
        rotateCoroutine = StartCoroutine(RotateObject()); // Start rotation coroutine

    }
    [ContextMenu("Bounce")]
    public void Bounce ()
    {
        isRotate = false;
        isBounce = true;
        GetComponent<Animator>().enabled = true;
        transform.localRotation = Quaternion.Euler(0 , 0 , 0);
    }


    private IEnumerator RotateObject ()
    {
        while (isRotate)  // Infinite loop for rotation
        {
            // Rotate around the Y-axis
            transform.Rotate(Vector3.up , rotationSpeed * Time.deltaTime);  // Rotate on the Y-axis
            yield return null;
        }
    }


    public void StopMovementAndRotation ()
    {
        isBounce = false; // Ensure bounce stops
        GetComponent<Animator>().enabled = false;
        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
            isRotate = false; // Ensure rotation stops
        }
    }
}
