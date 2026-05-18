using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public Transform GAN;
    public GameObject TamaPrefab;

    public float TamaSpeed = 20f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float zSpeed = 5 * Time.deltaTime;
        transform.Translate(0,0,zSpeed);
    }

    public void OnMove(InputValue value)
    {
        Debug.Log($"à⁄ìÆ [{value.Get<Vector2>()}]");

        Vector3 move = new Vector3(
            value.Get<Vector2>().x,
            value.Get<Vector2>().y,
            0 );

        if (transform.position.x + value.Get<Vector2>().x < -8
            || transform.position.x + value.Get<Vector2>().x > 8)
            return;

        if (transform.position.x + value.Get<Vector2>().y < -4
            || transform.position.x + value.Get<Vector2>().y > 6)
            return;

        move.x = Mathf.Round(move.x);
        move.y = Mathf.Round(move.y);

        transform.Translate(move);
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            GameObject Tama = Instantiate(TamaPrefab, GAN.position, GAN.rotation);
            Debug.Log($"çUåÇÉAÉNÉVÉáÉì [{value.Get<float>()}]");
            Rigidbody rb = Tama.GetComponent<Rigidbody>();
            rb.AddForce(GAN.forward * TamaSpeed, ForceMode.Impulse);
            Destroy(Tama, 5f);

        }
    }
}
