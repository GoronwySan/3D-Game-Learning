using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Enemy : MonoBehaviour
{
    public int health = 100;

    // �����˺��ķ���
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"{gameObject.name} ���ˣ�ʣ������: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} ������");

        // �ڵ�������λ�����ɱ�ըЧ��
        GameObject explosionEffect = Resources.Load<GameObject>("CFXR Prefabs/Eerie/CFXR2 WW Enemy Explosion");
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("δ�ҵ���ըЧ����Դ��");
        }
        GameObject bloodBag = Resources.Load<GameObject>("Healing Item");
        // ʵ��������
        GameObject instance = Instantiate(bloodBag, transform.position + new Vector3(0, 5f, 0), Quaternion.identity);
        // ����ʵ��������� y ��Ϊ 0
        instance.transform.position = new Vector3(instance.transform.position.x, 0, instance.transform.position.z);
        Destroy(gameObject);  // ���ٵ��˶���
    }

    // ���˵���Ϊ�����������ƶ��ȣ�
    public abstract IEnumerator Move();

    public abstract IEnumerator Rotate();
}
