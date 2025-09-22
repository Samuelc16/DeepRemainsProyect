using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;

    [Header("Disparo")]
    public GameObject torpedoPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;

    [Header("Límites")]
    [Range(0f, 0.2f)]
    public float screenMargin = 0.05f;

    private float nextFire = 0f;
    private Rigidbody2D rb;
    private Vector2 input;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Capturar input
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        // Disparo (barra espaciadora)
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFire && torpedoPrefab != null && firePoint != null)
        {
            Instantiate(torpedoPrefab, firePoint.position, firePoint.rotation);
            nextFire = Time.time + fireRate;
        }
    }

    void FixedUpdate()
    {
        // Movimiento
        Vector2 movement = input.normalized * speed;
        rb.linearVelocity = movement;

        // Mantener dentro de la cámara
        ClampToScreen();
    }

    void ClampToScreen()
    {
        Vector3 vp = Camera.main.WorldToViewportPoint(transform.position);
        vp.x = Mathf.Clamp(vp.x, screenMargin, 1f - screenMargin);
        vp.y = Mathf.Clamp(vp.y, screenMargin, 1f - screenMargin);
        transform.position = Camera.main.ViewportToWorldPoint(vp);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GetComponent<PlayerHealth>()?.TakeDamage(1);
        }
    }
}
