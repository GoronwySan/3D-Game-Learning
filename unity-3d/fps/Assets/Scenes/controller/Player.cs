using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float maxHealth = 100f;  // ���Ѫ��
    public float currentHealth;     // ��ǰѪ��
    public float moveSpeed = 5f;    // �ƶ��ٶ�
    public float attackPower = 10f; // ������

    private bool isHealthLocked = false;  // �Ƿ�����Ѫ��
    private bool isSpeedBoosted = false; // �Ƿ����

    private CharacterController characterController;
    private bool isDead = false;  // ����Ƿ�����
    // UI Ԫ��
    public GameObject deathPanel;  // �������

    void Start()
    {
        // ��ʼ��Ѫ��
        currentHealth = maxHealth;

        // ��ȡ CharacterController ���
        characterController = GetComponent<CharacterController>();

        // ȷ���������һ��ʼ�����ص�
        if (deathPanel != null)
        {
            deathPanel.SetActive(false);
        }
    }

    // Ѫ������
    public void HealFullHealth()
    {
        if (!isHealthLocked)
        {
            currentHealth = maxHealth;
            Debug.Log("Health is fully restored.");
        }
    }

    public void HealHealth(float addHealth)
    {
        currentHealth += addHealth;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    // ����Ѫ�������ָܻ������
    public void LockHealth(bool isLocked)
    {
        isHealthLocked = isLocked;
        Debug.Log("Health locked: " + isLocked);
    }

    // ���٣������ƶ��ٶ�
    public void BoostSpeed(float multiplier)
    {
        if (!isSpeedBoosted)
        {
            moveSpeed *= multiplier;
            isSpeedBoosted = true;
            Debug.Log("Speed boosted.");
        }
    }

    // ���٣������ƶ��ٶ�
    public void SlowDown(float multiplier)
    {
        moveSpeed /= multiplier;
        isSpeedBoosted = false;
        Debug.Log("Speed slowed down.");
    }

    // ����������
    public void IncreaseAttackPower(float amount)
    {
        attackPower += amount;
        Debug.Log("Attack power increased.");
    }

    // ���͹�����
    public void DecreaseAttackPower(float amount)
    {
        attackPower -= amount;
        Debug.Log("Attack power decreased.");
    }

    // ���˴���
    public void TakeDamage(float damage)
    {
        if (!isHealthLocked)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
            }
            Debug.Log("Took damage: " + damage + " | Current Health: " + currentHealth);
        }
    }

    // ��������
    private void Die()
    {
        if (!isDead)
        {
            Cursor.lockState = CursorLockMode.None;  // �������
            Cursor.visible = true;  // ��ʾ���
            SceneManager.LoadScene("SampleScene");
            isDead = true;
            Debug.Log("Player has died.");
            // ��ֹ��ҿ���
            characterController.enabled = false;

            // ��ֹ��������������������ƶ�������ͨ������NavMeshAgent�ȣ�
            StopAllMovingObjects();

            // ��ʾ�������
            if (deathPanel != null)
            {
                deathPanel.SetActive(true);
            }
        }
    }

    // ֹͣ���п��ƶ����壨�������壩
    private void StopAllMovingObjects()
    {
        // ֹͣ���� NavMeshAgent ���
        NavMeshAgent[] agents = FindObjectsOfType<NavMeshAgent>();
        foreach (var agent in agents)
        {
            if (agent.isOnNavMesh)  // ��� NavMeshAgent �Ƿ��ѷ����� NavMesh ��
            {
                agent.isStopped = true;  // ֹͣ NavMeshAgent
            }
            else
            {
                Debug.LogWarning("NavMeshAgent is not on the NavMesh!");
            }
        }

        // �������� CharacterController
        CharacterController[] controllers = FindObjectsOfType<CharacterController>();
        foreach (var controller in controllers)
        {
            controller.enabled = false;  // ���� CharacterController
        }

        // �������� Rigidbody��ʹ�䲻��������ϵͳ����
        Rigidbody[] rigidbodies = FindObjectsOfType<Rigidbody>();
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = true;  // ����Ϊ Kinematic����������ģ��
        }
    }



    // ��ȡ��ǰѪ���Ĺ����ӿ�
    public float GetCurrentHealth()
    {
        return currentHealth;
    }


    // ���£���������ƶ���ʹ�� Starter Assets - FirstPerson ϵͳ��
    void Update()
    {
        if (isDead)
        {
            return;  // �������Ѿ����������������ƶ�
        }
        if (characterController != null)
        {
            // ��һ����ƶ����ƣ��ɸ�����������޸ģ�
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    public void RestartGame()
    {
        // ���¼��ص�ǰ����������Ч����
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ������������水ť
    public void ReturnToMainMenu()
    {
        // �������˵��������������˵��ĳ�����Ϊ"MainMenu"��
        SceneManager.LoadScene("SampleScene");
    }
}
