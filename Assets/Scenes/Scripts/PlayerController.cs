using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering;
using System;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public GameObject powerupIndicator; // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject manager;
    public float speed = 5.0f;
    public bool haspowerUp = false;
    public ParticleSystem explosionParticle;
    private float powerupStrength = 15.0f;
    private float life = 100;

    private int points = 0;

    private float minimumImpactLimit = 3.0f;
    private float damageIntensifier = 0.5f;

    private bool isAlive = true;


    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        manager.GetComponent<GameManager>().UpdateScore(points);
        manager.GetComponent<GameManager>().UpdateLife(life);
    }
    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * forwardInput * speed);
        powerupIndicator.transform.position = transform.position + new UnityEngine.Vector3(0, -0.5f, 0);

        if (transform.position.y < -10 || !isAlive)
        {
            Destroy(powerupIndicator);
            Destroy(gameObject);
            //  Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            // 1) Instanciamos y guardamos la referencia
            ParticleSystem explosionInstance = Instantiate(
                explosionParticle,
                transform.position,
                explosionParticle.transform.rotation
            );
            // 2) Escalamos todo el objeto (x2 en este ejemplo)
            explosionInstance.transform.localScale *= 50f;
            manager.GetComponent<GameManager>().GameOver();


        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            haspowerUp = true;
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
        }
    }
    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        haspowerUp = false;
        powerupIndicator.gameObject.SetActive(false);

    }

    public UnityEngine.Vector3 CalculateVelocity()
    {
        float powerUpMultiplier = haspowerUp ? 2 : 1;
        return playerRb.linearVelocity * powerUpMultiplier;
    }

    public void addPointsForKill()
    {
        points += haspowerUp ? 75 : 100;
        manager.GetComponent<GameManager>().UpdateScore(points);
        Debug.Log($"Points updated: {points}");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();

            UnityEngine.Vector3 contactNormal = collision.GetContact(0).normal;

            float playerImpact = UnityEngine.Vector3.Dot(CalculateVelocity(), -contactNormal);
            float enemyImpact = UnityEngine.Vector3.Dot(enemyRb.linearVelocity, contactNormal);

            float playerImpactAbs = Mathf.Max(0, playerImpact);
            float enemyImpactAbs = Mathf.Max(0, enemyImpact);

            float calculatedImpact = playerImpactAbs - enemyImpactAbs;

            if (calculatedImpact > minimumImpactLimit)
            {
                life = (int)Math.Ceiling(life - calculatedImpact * damageIntensifier);
                if (life <= 0)
                {
                    life = 0;
                    isAlive = false;
                }
                Debug.Log($"Player life updated, current: {life}, isAlive={isAlive}");
                manager.GetComponent<GameManager>().UpdateLife(life);

            }

            if (haspowerUp)
            {
                UnityEngine.Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
                enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
                Debug.Log("Collided with" + collision.gameObject.name + " with powerup set to" + haspowerUp);
            }

        }
    }
}