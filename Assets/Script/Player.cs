using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public Transform GAN;
    public GameObject TamaPrefab;
    public GameObject effectPrefab;
    public Transform shotPoint;
    public GameObject bulletPrefab;
    [SerializeField] private float shellSpeed = 1000.0f;
    [SerializeField] private int pelletsCount = 8;
    [SerializeField] private float spreadIntensity = 10.0f;

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

    [Header("***エフェクトの設定***")]
    public float MuzzlFlashParticle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float zSpeed = 5 * Time.deltaTime;
        transform.Translate(0, 0, zSpeed);

        lookAngles.x += inputMoveVelocity.y * (tiltInvart ? -1 : 1);
        lookAngles.y += inputMoveVelocity.x;
        gyroAngle += inputMoveVelocity.x;


        lookAngles = Vector3.Lerp(lookAngles, Vector3.zero, Time.deltaTime);
        gyroAngle = Mathf.Lerp(gyroAngle, 0, Time.deltaTime * 3);

        lookAngles.x = Mathf.Clamp(lookAngles.x, -15, 15);
        lookAngles.y = Mathf.Clamp(lookAngles.y, -15, 15);
        gyroAngle = Mathf.Clamp(gyroAngle, -15, 15);

        lookAxis.transform.eulerAngles = lookAngles;
        gyroAxis.transform.eulerAngles = new Vector3(0, 0, gyroAngle);
    }


    public void OnMove(InputValue value)
    {
        Debug.Log($"移動 [{value.Get<Vector2>()}]");

        Vector3 move = new Vector3(
            value.Get<Vector2>().x,
            value.Get<Vector2>().y,
            0);

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
        if (!value.isPressed) return;

        // shotPointが設定されていない場合は、自身の位置を発射点にする
        Vector3 spawnPosition = shotPoint != null ? shotPoint.position : transform.position;

        for (int i = 0; i < pelletsCount; i++)
        {
            // 1. ランダムな拡散角度を計算
            float randomSpreadX = Random.Range(-spreadIntensity, spreadIntensity);
            float randomSpreadY = Random.Range(-spreadIntensity, spreadIntensity);

            // 2. 【変更点】プレイヤーの回転（transform.rotation）にランダムな回転を合成
            Quaternion spreadRotation = Quaternion.Euler(randomSpreadX, randomSpreadY, 0);
            Quaternion finalRotation = transform.rotation * spreadRotation;

            // 3. 弾を生成
            GameObject shell = Instantiate(bulletPrefab, spawnPosition, finalRotation);

            // 4. 物理演算で飛ばす
            Rigidbody shellRb = shell.GetComponent<Rigidbody>();
            if (shellRb != null)
            {
                // 生成された弾の正面（forward）に向かって力を加える
                shellRb.AddForce(shell.transform.forward * shellSpeed);
            }
            // 2Dゲーム（Rigidbody2D）の場合は以下のように記述します
            // Rigidbody2D shellRb2D = shell.GetComponent<Rigidbody2D>();
            // if (shellRb2D != null) { shellRb2D.AddForce(shell.transform.up * shellSpeed); }

            Destroy(shell, 2.0f + Random.Range(0f, 1.0f));

            if (effectPrefab != null)
            {
                // 銃口の位置にエフェクトを生成
                GameObject effect = Instantiate(effectPrefab, shotPoint.position, shotPoint.rotation);

                // エフェクトが自動で消えない場合は、2秒後に削除する設定
                Destroy(effect, 2.0f);
            }
        }
        void OnTriggerEnter(Collider collision)
        {
            if (!collision.transform.tag.Equals("Item/Barrire"))
            {
                Material m = barrireRendere.material;

                barrireActivation = true;

                m.SetInt("IsActive", 1);
            }
        }
    }
}
