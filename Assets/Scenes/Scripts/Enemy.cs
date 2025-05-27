using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed = 3.0f;
    public ParticleSystem explosionParticle;
    private Rigidbody enemyRb;
    private GameObject player;
    private GameObject manager;

    private float life = 100;

    private float minimumImpactLimit = 3.0f;
    private float damageIntensifier = 3.0f;

    private bool isAlive = true;

    void Awake()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("EggPlayer");
        manager = GameObject.Find("Manager");
    }
    // Update is called once per frame
    void Update()
    {
        if (!player || !manager.GetComponent<GameManager>().isGameActive)
            return;
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;//ojo error se debe solucionar cuando muera
        enemyRb.AddForce(lookDirection * speed);
        if (transform.position.y < -10 || !isAlive)
        {
            player.GetComponent<PlayerController>().addPointsForKill();

            Destroy(gameObject);
            //Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            ParticleSystem explosionInstance = Instantiate(
                explosionParticle,
                transform.position,
                explosionParticle.transform.rotation
            );
            // 2) Escalamos todo el objeto (x2 en este ejemplo)
            explosionInstance.transform.localScale *= 50f;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UnityEngine.Vector3 contactNormal = collision.GetContact(0).normal;
            float playerImpact = UnityEngine.Vector3.Dot(player.GetComponent<PlayerController>().CalculateVelocity(), contactNormal);
            float enemyImpact = UnityEngine.Vector3.Dot(enemyRb.linearVelocity, -contactNormal);

            float playerImpactAbs = Mathf.Max(0, playerImpact);
            float enemyImpactAbs = Mathf.Max(0, enemyImpact);

            float calculatedImpact = playerImpactAbs - enemyImpactAbs;

            if (calculatedImpact > minimumImpactLimit)
            {
                life = life - calculatedImpact * damageIntensifier;
                if (life <= 0)
                {
                    life = 0;
                    isAlive = false;
                }
                Debug.Log($"Enemy life updated, current: {life}, isAlive={isAlive}");
            }

        }
    }
}
