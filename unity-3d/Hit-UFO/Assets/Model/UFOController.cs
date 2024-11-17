using UnityEngine;

public class UFOController : MonoBehaviour
{
    public GameObject explosionPrefab; // 爆炸特效预制体
    [SerializeField] private Transform thinRoundPad;
    [SerializeField] private float rotationSpeed = 100f;
    private Rigidbody rb;

    private Transform mainCamera;
    public int Num { get; set; } = -1;

    private Vector3 targetPosition;
    private bool isInterpolating = false;
    private float lerpDuration = 3f;
    private float elapsedTime = 0f;
    public float baseForce;

    private float yAxisForceInterval = 0.5f; // 每隔0.5秒施加一次y轴力
    private float yAxisForceTimer = 0f;     // 计时器

    public UFOController[] ufos; // 飞碟数组

    void Start()
    {
        InitializeComponents();
        InitializeMotionParameters();
    }

    void Update()
    {
        RotateThinRoundPad();
        CheckAllUFOPositions(); // 修改后的逻辑
    }

    private void FixedUpdate()
    {
        if (isInterpolating)
        {
            InterpolateToTarget();
        }
        else
        {
            ApplyRandomForces();
        }

        ApplyYAxisForce();
    }

    public void StartInterpolation(Vector3 target, float duration)
    {
        targetPosition = target;
        lerpDuration = duration;
        elapsedTime = 0f;
        isInterpolating = true;

        Debug.Log($"UFO starts interpolating to {targetPosition} over {lerpDuration} seconds.");
    }

    public void StopMotion()
    {
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            rb.isKinematic = true;
        }
        Debug.Log($"UFO {name} has stopped moving.");
    }

    public void RestartAllUFOs()
    {
        foreach (var ufo in ufos)
        {
            if (ufo != null)
            {
                ufo.RestartMotion();
            }
        }
    }

    public void RestartMotion()
    {
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.velocity = Vector3.zero;

            transform.position = new Vector3(
                Random.Range(-10f, 10f),
                Random.Range(5f, 6f),
                Random.Range(5f, 6f)
            );

            Debug.Log($"UFO {name} has restarted motion.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            TriggerExplosion();
        }
    }

    void TriggerExplosion()
    {
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 0.5f);
        }
        else
        {
            Debug.LogWarning("Explosion prefab not set or not found.");
        }
    }

    // Helper Functions
    private void InitializeComponents()
    {
        mainCamera = Camera.main?.transform;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera is not found. Please ensure the main camera is tagged as 'MainCamera'.");
        }

        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = true;

        if (gameObject.GetComponent<Collider>() == null)
        {
            SphereCollider collider = gameObject.AddComponent<SphereCollider>();
            collider.isTrigger = false;
        }

        if (explosionPrefab == null)
        {
            explosionPrefab = Resources.Load<GameObject>("Prefabs/ExplosionEffect");
        }

        ufos = FindObjectsOfType<UFOController>();
    }

    private void InitializeMotionParameters()
    {
        baseForce = Random.Range(5f, 6f);
    }

    private void RotateThinRoundPad()
    {
        if (thinRoundPad != null)
        {
            thinRoundPad.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }

    private void CheckAllUFOPositions()
    {
        foreach (var ufo in ufos)
        {
            if (ufo != null && ufo.transform.position.z >= 0)
            {
                return; // 如果有飞碟的 Z 轴大于等于 0，则继续游戏
            }
        }

        // 所有飞碟的 Z 坐标都小于 0，游戏结束
        SSDirector.getInstance().SetGameEnd(true);
        Debug.Log("All UFOs have Z < 0. Game Over!");
    }

    private void InterpolateToTarget()
    {
        elapsedTime += Time.fixedDeltaTime;

        // 计算插值比例
        float t = Mathf.Clamp01(elapsedTime / lerpDuration);
        Vector3 interpolatedPosition = Vector3.Lerp(transform.position, targetPosition, t);

        rb.MovePosition(interpolatedPosition);

        if (t >= 1f)
        {
            isInterpolating = false;
            Debug.Log("UFO has reached the target position via interpolation.");
        }
    }

    private void ApplyRandomForces()
    {
        if (rb != null)
        {
            Vector3 randomForce = new Vector3(
                Random.Range(-1f, 1f),
                0f, // y轴力通过定时逻辑控制
                Random.Range(-1f, 1f)
            ).normalized * baseForce;

            rb.AddForce(randomForce);
        }
    }

    private void ApplyYAxisForce()
    {
        yAxisForceTimer += Time.fixedDeltaTime;

        if (yAxisForceTimer >= yAxisForceInterval)
        {
            // 每隔0.5秒施加一次向上的力
            Vector3 upwardForce = new Vector3(0f, Random.Range(5f, 6f), -1f);
            rb.AddForce(upwardForce, ForceMode.Impulse);

            Debug.Log($"Applied upward force: {upwardForce.y}");
            yAxisForceTimer = 0f; // 重置计时器
        }
    }
}
