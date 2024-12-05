using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowController : MonoBehaviour
{
    private AudioSource audioSource;  // ��ƵԴ���
    private AudioClip arrowSound;  // ���ڴ洢������Ч
    public GameObject player;  // ��ҽ�ɫ���ã�ȷ���� Inspector �и�ֵ

    private Animator crossbowAnimator;
    private bool isFiring = false;
    private Camera mainCamera;

    // ���ڴ�ż��Ķ����
    public GameObject arrowPrefab;
    private Queue<GameObject> arrowPool = new Queue<GameObject>();
    private List<GameObject> activeArrows = new List<GameObject>(); // ����ʹ�õļ�
    public int poolSize = 15;
    public float arrowSpeed = 500f;
    public float recycleTime = 10f;

    public float customGravity = -9f;  // ����Ե������ֵ���ı��������

    private Dictionary<GameObject, int> shootingPositions = new Dictionary<GameObject, int>(); // ��¼ÿ��ShootingPosition���������

    void Start()
    {
        // ȷ����Ҷ����ڳ�����
        if (player == null)
        {
            player = GameObject.Find("PlayerCapsule");  // ������ҵ� GameObject ��Ϊ PlayerCapsule
        }

        // ��ȡ AudioSource ���
        audioSource = GetComponent<AudioSource>();

        // ���û�� AudioSource ����������һ��
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        crossbowAnimator = GetComponent<Animator>();
        mainCamera = Camera.main;

        // ���ؼ���Prefab
        if (arrowPrefab == null)
        {
            arrowPrefab = Resources.Load<GameObject>("crossbow/Arrow");
        }


        // ��ʼ���������
        for (int i = 0; i < poolSize; i++)
        {
            GameObject arrow = Instantiate(arrowPrefab);
            arrow.SetActive(false); // ��ʼʱ���м�������ʾ
            arrowPool.Enqueue(arrow);
            Arrow arr = arrow.AddComponent<Arrow>();
        }

        // �� Resources �ļ��м��ؼ�����Ч
        LoadArrowSound();

        // ��ʼ��ÿ�����ΪShootingPosition��������������
        GameObject[] shootingPositionsArray = GameObject.FindGameObjectsWithTag("ShootingPosition");
        foreach (GameObject position in shootingPositionsArray)
        {
            shootingPositions[position] = 0;  // ÿ��λ�ó�ʼ�������Ϊ0
        }
    }

    // ���ؼ�����Ч
    private void LoadArrowSound()
    {
        // ���ش洢�� Resources �ļ����е���Ч
        arrowSound = Resources.Load<AudioClip>("7924");

        // �����Ч�Ƿ���سɹ�
        if (arrowSound == null)
        {
            Debug.LogError("Arrow sound not found in Resources/Sounds/ArrowSound.");
        }
        else
        {
            Debug.Log("Arrow sound loaded successfully.");
        }
    }    

    void Update()
    {
        // Crossbow����main camera����ת
        Vector3 targetDirection = mainCamera.transform.forward;
        transform.rotation = Quaternion.LookRotation(targetDirection);

        // ���������� Fire ����
        if (Input.GetMouseButtonDown(0)) // 0��ʾ������
        {
            // �ȼ���Ƿ�վ�ڱ��ΪShootingPosition��������
            if (IsPlayerAtShootingPosition())
            {
                // �л� firing ״̬�����津�� Fire ����
                isFiring = !isFiring;  // �л�״̬

                // ���� Fire ���������� isFiring ״̬����������
                crossbowAnimator.SetBool("Fire", isFiring);
                if (!isFiring) FireArrow();
            }
            else
            {
                Debug.Log("You must stand on a ShootingPosition to shoot.");
            }
        }

    }

    // �ж�����Ƿ�վ�ڱ��ΪShootingPosition��������
    // �ж�����Ƿ�վ�ڱ��ΪShootingPosition��������
    private bool IsPlayerAtShootingPosition()
    {
        foreach (var entry in shootingPositions)
        {
            GameObject shootingPosition = entry.Key;

            // ��ȡ��ҵ�λ��
            Vector3 playerPosition = player.transform.position;

            // �������Ƿ��� ShootingPosition ����ײ�巶Χ�ڣ����������δ���� 10 ��
            if (shootingPosition.GetComponent<Collider>().bounds.Contains(playerPosition) && shootingPositions[shootingPosition] < 100)
            {
                return true;
            }
        }
        return false;
    }


    // ��ȡ�Ƿ�������Ĺ��к���
    public bool CanShootArrow()
    {
        foreach (var entry in shootingPositions)
        {
            GameObject shootingPosition = entry.Key;

            // ��ȡ��ҵ�λ��
            Vector3 playerPosition = player.transform.position;

            // �������Ƿ��� ShootingPosition ����ײ�巶Χ�ڣ����������δ���� 10 ��
            if (shootingPosition.GetComponent<Collider>().bounds.Contains(playerPosition) && shootingPositions[shootingPosition] < 100)
            {
                Debug.Log($"Player can shoot from {shootingPosition.name}. Remaining shots: {10 - shootingPositions[shootingPosition]}");
                return true;
            }
        }

        Debug.Log("Player cannot shoot: Not in any valid ShootingPosition.");
        return false;
    }

    // ��ȡʣ���������
    public int GetRemainingArrows()
    {
        foreach (var entry in shootingPositions)
        {
            GameObject shootingPosition = entry.Key;

            // ��ȡ��ҵ�λ��
            Vector3 playerPosition = player.transform.position;

            // �������Ƿ��� ShootingPosition ����ײ�巶Χ��
            if (shootingPosition.GetComponent<Collider>().bounds.Contains(playerPosition))
            {
                int remainingArrows = 10 - shootingPositions[shootingPosition];
                Debug.Log($"Remaining arrows at {shootingPosition.name}: {remainingArrows}");
                return remainingArrows;
            }
        }

        Debug.Log("No valid ShootingPosition: Returning 0 arrows.");
        return 0;  // ����������������򷵻� 0
    }



    // �����
    private void FireArrow()
    {
        GameObject arrow;

        if (arrowPool.Count > 0)
        {
            // �ӳ��л�ȡһ����
            arrow = arrowPool.Dequeue();
        }
        else
        {
            // ���û�п��õļ�����������Ļ��
            arrow = activeArrows[0];
            activeArrows.RemoveAt(0); // �ӻ���б��Ƴ�
            arrow.SetActive(false);  // ���ü�
        }

        // �����������λ�ú���ת
        arrow.SetActive(true);
        arrow.transform.position = transform.position;
        arrow.transform.rotation = transform.rotation;

        // ��ӵ�����б�
        activeArrows.Add(arrow);

        // ��ȡ���ĸ��������ʩ����
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // ȷ�����岻�� Kinematic
            rb.isKinematic = false;
            rb.mass = 0.1f;  // ����������Ϊ��Сֵ������������Ӱ��
            rb.drag = 0.05f;  // ���Ϳ�������
            rb.angularDrag = 0.05f;  // ���ͽǶ�����
            rb.velocity = Vector3.zero; // �����ٶ�
            rb.useGravity = false;
            rb.AddForce(Vector3.up * customGravity, ForceMode.Acceleration);
            rb.AddForce(transform.forward * arrowSpeed, ForceMode.VelocityChange);
        }

        // ���ż��������Ч
        if (arrowSound != null)
        {
            audioSource.PlayOneShot(arrowSound);  // ����һ����Ч
        }

        // ���µ�ǰ���λ�õ��������
        foreach (var entry in shootingPositions)
        {
            GameObject shootingPosition = entry.Key;
            Vector3 playerPosition = player.transform.position;
            // �������Ƿ��ڵ�ǰ���λ��
            if (shootingPosition.GetComponent<Collider>().bounds.Contains(playerPosition))
            {
                shootingPositions[shootingPosition]++;  // �����������
                //Debug.Log($"Updated shooting position {shootingPosition.name} shot count: {shootingPositions[shootingPosition]}");
                break;  // ֻ����һ��λ��
            }
        }

        // �������ռ���Э��
        StartCoroutine(RecycleArrow(arrow));
    }

    // ���ռ���10���
    private IEnumerator RecycleArrow(GameObject arrow)
    {
        // �ȴ�10�����ռ�
        yield return new WaitForSeconds(recycleTime);

        // ֹͣ�����˶�������
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        if (rb != null && rb.isKinematic == false)
        {
            rb.velocity = Vector3.zero; // ֹͣ������ƶ�
        }

        activeArrows.Remove(arrow); // �ӻ���б��Ƴ�
        arrow.SetActive(false); // ���ؼ�
        arrowPool.Enqueue(arrow); // �������ص������
    }
}
