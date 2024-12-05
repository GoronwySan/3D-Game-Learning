using UnityEngine;
using UnityEngine.AI;  // 引入NavMesh相关命名空间
using System.Collections;
using System.Collections.Generic;

// 子类：鸭子
public class Duck : Enemy
{
    private NavMeshAgent navMeshAgent;  // 引用NavMeshAgent
    private float moveCooldown = 10f;  // 移动冷却时间（可以设置随机范围）

    void Start()
    {
        // 获取NavMeshAgent组件
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent 组件未找到！");
        }

        // 启动移动协程
        StartCoroutine(Move());
        StartCoroutine(Rotate());
    }

    // 每0.3秒向前平滑移动动物
    public override IEnumerator Move()
    {
        while (true)
        {
            // 随机选择一个目标点
            Vector3 randomTargetPosition = GetRandomPointOnNavMesh();

            // 设置目标位置
            navMeshAgent.SetDestination(randomTargetPosition);

            // 等待直到目标位置到达或超时
            yield return new WaitForSeconds(moveCooldown);  // 移动冷却时间
        }
    }

    // 获取随机点，确保其位于NavMesh上
    private Vector3 GetRandomPointOnNavMesh()
    {
        Vector3 randomPoint = new Vector3(
            Random.Range(-125f, 100f),  // 随机x轴位置
            0,  // y轴高度可以固定为0，或者根据场景调整
            Random.Range(-100f, 100f)   // 随机z轴位置
        );

        // 找到在NavMesh上的有效位置
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 10f, NavMesh.AllAreas))
        {
            return hit.position;  // 返回有效的位置
        }
        else
        {
            return randomPoint;  // 如果无法找到有效位置，返回原点
        }
    }

    // 每秒随机增加或减少旋转y轴（适用于duck_f和duck_m）
    public override IEnumerator Rotate()
    {
        while (true)
        {
            // 保持x和z轴为0，只改变y轴的旋转
            //Vector3 currentRotation = transform.eulerAngles;
            //transform.eulerAngles = new Vector3(0, currentRotation.y + Random.Range(-5f, 5f), 0);

            // 每 0.3 秒调整一次
            yield return new WaitForSeconds(0.3f);
        }
    }

    // 每帧更新朝向
    void Update()
    {
        // 如果 NavMeshAgent 正在移动
        if (navMeshAgent.velocity.sqrMagnitude > 0.1f)
        {
            // 获取移动方向（只考虑 x, z 轴方向）
            Vector3 direction = navMeshAgent.velocity.normalized;

            // 计算目标旋转角度，并旋转180度
            Quaternion targetRotation = Quaternion.LookRotation(-direction);  // 使用反方向

            // 平滑旋转
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

}
