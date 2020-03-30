using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] float moveSpeed = 15f;
    [SerializeField] float padding = 0.5f;
    [SerializeField] int health = 200;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 20f;
    [SerializeField] float projectileFiringPeriod = 1f;

    [Header("Death")]
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float explosionTimer = 1f;
    [SerializeField] AudioClip explosionSFX;
    [SerializeField] [Range(0, 1f)] float explosionSFXVolume = 1f;

    [Header("Hit")]
    [SerializeField] AudioClip hitSFX;
    [SerializeField] [Range(0, 1f)] float hitSFXVolume = 1f;

    Coroutine firingCoroutine;

    float xMin, yMin;
    float xMax, yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
           firingCoroutine = StartCoroutine(FireContinuosly());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    private IEnumerator FireContinuosly()
    {
        while (true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        var nextXPosition = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var nextYPosition = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(nextXPosition, nextYPosition);
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        AudioSource.PlayClipAtPoint(hitSFX, Camera.main.transform.position, hitSFXVolume);
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        AudioSource.PlayClipAtPoint(explosionSFX, Camera.main.transform.position, explosionSFXVolume);
        Instantiate(explosionPrefab, transform.position, new Quaternion(180, 0, 0, 0));
        Destroy(gameObject);
        FindObjectOfType<Level>().LoadGameOver();
    }

    public int GetHealth()
    {
        return health;
    }
}
