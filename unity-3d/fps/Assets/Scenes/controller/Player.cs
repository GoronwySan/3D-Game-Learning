using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float maxHealth = 100f;  // 最大血量
    public float currentHealth;     // 当前血量
    public float moveSpeed = 5f;    // 移动速度
    public float attackPower = 10f; // 攻击力

    private bool isHealthLocked = false;  // 是否锁定血量
    private bool isSpeedBoosted = false; // 是否加速

    private CharacterController characterController;
    private bool isDead = false;  // 玩家是否死亡
    // UI 元素
    public GameObject deathPanel;  // 死亡面板

    void Start()
    {
        // 初始化血量
        currentHealth = maxHealth;

        // 获取 CharacterController 组件
        characterController = GetComponent<CharacterController>();

        // 确保死亡面板一开始是隐藏的
        if (deathPanel != null)
        {
            deathPanel.SetActive(false);
        }
    }

    // 血量回满
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

    // 锁定血量，不能恢复或减少
    public void LockHealth(bool isLocked)
    {
        isHealthLocked = isLocked;
        Debug.Log("Health locked: " + isLocked);
    }

    // 加速，提升移动速度
    public void BoostSpeed(float multiplier)
    {
        if (!isSpeedBoosted)
        {
            moveSpeed *= multiplier;
            isSpeedBoosted = true;
            Debug.Log("Speed boosted.");
        }
    }

    // 减速，降低移动速度
    public void SlowDown(float multiplier)
    {
        moveSpeed /= multiplier;
        isSpeedBoosted = false;
        Debug.Log("Speed slowed down.");
    }

    // 提升攻击力
    public void IncreaseAttackPower(float amount)
    {
        attackPower += amount;
        Debug.Log("Attack power increased.");
    }

    // 降低攻击力
    public void DecreaseAttackPower(float amount)
    {
        attackPower -= amount;
        Debug.Log("Attack power decreased.");
    }

    // 受伤处理
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

    // 死亡处理
    private void Die()
    {
        if (!isDead)
        {
            Cursor.lockState = CursorLockMode.None;  // 解锁鼠标
            Cursor.visible = true;  // 显示鼠标
            SceneManager.LoadScene("SampleScene");
            isDead = true;
            Debug.Log("Player has died.");
            // 禁止玩家控制
            characterController.enabled = false;

            // 禁止场景中其他所有物体的移动（例如通过禁用NavMeshAgent等）
            StopAllMovingObjects();

            // 显示死亡面板
            if (deathPanel != null)
            {
                deathPanel.SetActive(true);
            }
        }
    }

    // 停止所有可移动物体（包括刚体）
    private void StopAllMovingObjects()
    {
        // 停止所有 NavMeshAgent 组件
        NavMeshAgent[] agents = FindObjectsOfType<NavMeshAgent>();
        foreach (var agent in agents)
        {
            if (agent.isOnNavMesh)  // 检查 NavMeshAgent 是否已放置在 NavMesh 上
            {
                agent.isStopped = true;  // 停止 NavMeshAgent
            }
            else
            {
                Debug.LogWarning("NavMeshAgent is not on the NavMesh!");
            }
        }

        // 禁用所有 CharacterController
        CharacterController[] controllers = FindObjectsOfType<CharacterController>();
        foreach (var controller in controllers)
        {
            controller.enabled = false;  // 禁用 CharacterController
        }

        // 禁用所有 Rigidbody，使其不再受物理系统控制
        Rigidbody[] rigidbodies = FindObjectsOfType<Rigidbody>();
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = true;  // 设置为 Kinematic，禁用物理模拟
        }
    }



    // 获取当前血量的公共接口
    public float GetCurrentHealth()
    {
        return currentHealth;
    }


    // 更新，控制玩家移动（使用 Starter Assets - FirstPerson 系统）
    void Update()
    {
        if (isDead)
        {
            return;  // 如果玩家已经死亡，则不再允许移动
        }
        if (characterController != null)
        {
            // 玩家基本移动控制（可根据需求进行修改）
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    public void RestartGame()
    {
        // 重新加载当前场景（重生效果）
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 点击返回主界面按钮
    public void ReturnToMainMenu()
    {
        // 加载主菜单场景（假设主菜单的场景名为"MainMenu"）
        SceneManager.LoadScene("SampleScene");
    }
}
