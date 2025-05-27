using UnityEngine;

public class Floater : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float waterHeight = 0f; // Altura del agua
    public float floatStrength = 1f; // Fuerza de flotación

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y < waterHeight)
        {
            float difference = waterHeight - transform.position.y;
            rb.AddForce(Vector3.up * difference * floatStrength, ForceMode.Force);
        }
    }
}
