using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public Transform GAN;
    public GameObject TamaPrefab;

    public float TamaSpeed = 20f;

    [Header("***移動値の設定")]
    private Vector3 inputMoveVelocity;

    [Header("***回転軸の設定")]
    public bool tiltInvart = false;
    public GameObject lookAxis;
    public GameObject gyroAxis;
    private Vector3 lookAngles;
    private float gyroAngle;

    [Header("***バリア設定***")]
    public GameObject barrire;
    public MeshRenderer barrireRendere;
    public bool barrireActivation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float zSpeed = 5 * Time.deltaTime;
        transform.Translate(0,0,zSpeed);

        lookAngles.x += inputMoveVelocity.y * (tiltInvart ? -1 : 1);
        lookAngles.y += inputMoveVelocity.x;
        gyroAngle += inputMoveVelocity.x;
        

        lookAngles = Vector3.Lerp(lookAngles,Vector3.zero, Time.deltaTime);
        gyroAngle = Mathf.Lerp(gyroAngle,0, Time.deltaTime * 3);

        lookAngles.x = Mathf.Clamp(lookAngles.x, -15, 15);
        lookAngles.y = Mathf.Clamp(lookAngles.y, -15, 15);
        gyroAngle = Mathf.Clamp(gyroAngle,-15, 15);

        lookAxis. transform.eulerAngles = lookAngles;
        gyroAxis.transform.eulerAngles = new Vector3(0,0, gyroAngle);
    }


    public void OnMove(InputValue value)
    {
        Debug.Log($"移動 [{value.Get<Vector2>()}]");

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

        inputMoveVelocity = move;
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            GameObject Tama = Instantiate(TamaPrefab, GAN.position, GAN.rotation);
            Debug.Log($"攻撃アクション [{value.Get<float>()}]");
            Rigidbody rb = Tama.GetComponent<Rigidbody>();
            rb.AddForce(GAN.forward * TamaSpeed, ForceMode.Impulse);
            Destroy(Tama, 5f);

        }

        void OnTriggerEnter(Collider collision)
        {
            if(!collision.transform.tag.Equals("Item/Barrire"))
            {
                Material m = barrireRendere.material;

                barrireActivation = true;

                m.SetInt("IsActive", 1);
            }
        }
    }
}
