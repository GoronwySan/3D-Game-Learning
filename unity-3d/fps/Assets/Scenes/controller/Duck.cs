using UnityEngine;
using UnityEngine.AI;  // ����NavMesh��������ռ�
using System.Collections;
using System.Collections.Generic;

// ���ࣺѼ��
public class Duck : Enemy
{
    private NavMeshAgent navMeshAgent;  // ����NavMeshAgent
    private float moveCooldown = 10f;  // �ƶ���ȴʱ�䣨�������������Χ��

    void Start()
    {
        // ��ȡNavMeshAgent���
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent ���δ�ҵ���");
        }

        // �����ƶ�Э��
        StartCoroutine(Move());
        StartCoroutine(Rotate());
    }

    // ÿ0.3����ǰƽ���ƶ�����
    public override IEnumerator Move()
    {
        while (true)
        {
            // ���ѡ��һ��Ŀ���
            Vector3 randomTargetPosition = GetRandomPointOnNavMesh();

            // ����Ŀ��λ��
            navMeshAgent.SetDestination(randomTargetPosition);

            // �ȴ�ֱ��Ŀ��λ�õ����ʱ
            yield return new WaitForSeconds(moveCooldown);  // �ƶ���ȴʱ��
        }
    }

    // ��ȡ����㣬ȷ����λ��NavMesh��
    private Vector3 GetRandomPointOnNavMesh()
    {
        Vector3 randomPoint = new Vector3(
            Random.Range(-125f, 100f),  // ���x��λ��
            0,  // y��߶ȿ��Թ̶�Ϊ0�����߸��ݳ�������
            Random.Range(-100f, 100f)   // ���z��λ��
        );

        // �ҵ���NavMesh�ϵ���Чλ��
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 10f, NavMesh.AllAreas))
        {
            return hit.position;  // ������Ч��λ��
        }
        else
        {
            return randomPoint;  // ����޷��ҵ���Чλ�ã�����ԭ��
        }
    }

    // ÿ��������ӻ������תy�ᣨ������duck_f��duck_m��
    public override IEnumerator Rotate()
    {
        while (true)
        {
            // ����x��z��Ϊ0��ֻ�ı�y�����ת
            //Vector3 currentRotation = transform.eulerAngles;
            //transform.eulerAngles = new Vector3(0, currentRotation.y + Random.Range(-5f, 5f), 0);

            // ÿ 0.3 �����һ��
            yield return new WaitForSeconds(0.3f);
        }
    }

    // ÿ֡���³���
    void Update()
    {
        // ��� NavMeshAgent �����ƶ�
        if (navMeshAgent.velocity.sqrMagnitude > 0.1f)
        {
            // ��ȡ�ƶ�����ֻ���� x, z �᷽��
            Vector3 direction = navMeshAgent.velocity.normalized;

            // ����Ŀ����ת�Ƕȣ�����ת180��
            Quaternion targetRotation = Quaternion.LookRotation(-direction);  // ʹ�÷�����

            // ƽ����ת
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

}
