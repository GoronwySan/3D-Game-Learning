using UnityEngine;

public class TargetController : MonoBehaviour
{
    public GameObject targetPrefab;  // Ŀ��Ԥ����
    public GameObject a;  // Ŀ��Ԥ����

    public int targetCount = 10;     // ���ɵ�Ŀ������
    public Vector3 startPosition = new Vector3(100f, 0f, -150f); // Ŀ����ʼλ��
    public Vector3 rotation = new Vector3(90f, 0f, 0f);  // Ŀ����ת�Ƕ�
    public Vector3 scale = new Vector3(50f, 1f, 50f);    // Ŀ�����ű���

    public Vector3 s = new Vector3(105f, 0f, -180f); // Ŀ����ʼλ��
    public Vector3 r = new Vector3(-90f, 0f, 0f);  // Ŀ����ת�Ƕ�
    public Vector3 sc = new Vector3(200f, 20f, 20f);    // Ŀ�����ű���

    public int totalTargetsDestroyed = 0;  // �����ٵ�Ŀ������
    private GameObject[] allTargets;  // ����Ŀ��

    void Start()
    {
        // ������Դ
        targetPrefab = Resources.Load<GameObject>("target/Military target");
        a = Resources.Load<GameObject>("target/Training table");

        if (targetPrefab == null)
        {
            Debug.LogError("Ŀ��Ԥ�������ʧ�ܣ�ȷ��·����ȷ������Ŀ����Դ��");
            return;
        }

        // ����Ŀ��
        allTargets = new GameObject[targetCount];
        for (int i = 0; i < targetCount; i++)
        {
            SpawnTarget(i);
        }
        for (int i = 0; i < 9; i++)
        {
            SpawnTable(i);
        }
    }

    void SpawnTarget(int index)
    {
        // ����Ŀ��λ��
        Vector3 spawnPosition = startPosition + new Vector3(index * 10f, 0f, 0f);  // ÿ��Ŀ�갴 X ��ƫ��

        // ʵ����Ŀ��
        GameObject target = Instantiate(targetPrefab, spawnPosition, Quaternion.Euler(rotation));

        // ����Ŀ���ǩ
        target.tag = "Target";

        // ����Ŀ������
        target.transform.localScale = scale;

        // ��Ŀ����ӵ� allTargets ����
        allTargets[index] = target;
    }

    void SpawnTable(int index)
    {
        // ����Ŀ��λ��
        Vector3 ss = s + new Vector3(index * 10f, 0f, 0f);  // ÿ��Ŀ�갴 X ��ƫ��

        // ʵ����Ŀ��
        GameObject target = Instantiate(a, ss, Quaternion.Euler(r));

        // ����Ŀ������
        target.transform.localScale = sc;
    }

    public void OnTargetDestroyed()
    {
        // �������ٵ�Ŀ������
        totalTargetsDestroyed++;

        // ����Ƿ�����Ŀ�궼������
        if (totalTargetsDestroyed >= targetCount)
        {
            Debug.Log("����Ŀ�������٣�");
        }
    }

    // ��������Ŀ�����Ĺ��к���
    public void DestroyAllTargets()
    {
        // ��������Ŀ�겢����
        for (int i = 0; i < allTargets.Length; i++)
        {
            if (allTargets[i] != null)
            {
                Destroy(allTargets[i]);
            }
        }

        // ��� allTargets ����
        allTargets = new GameObject[targetCount];
        Debug.Log("����Ŀ�������٣�");
    }

    // �ⲿ�ű����Ե��ô˷��������� TargetController �ű�
    public void DestroyController()
    {
        Debug.Log("���� TargetController �ű���");
        Destroy(this);  // ���� TargetController �ű�
    }
}

public class Destroyable : MonoBehaviour
{
    public delegate void DestroyedHandler();
    public event DestroyedHandler onDestroyed;

    // ���ٶ���ʱ����
    void OnDestroy()
    {
        // �����ע��������¼����ʹ�����
        onDestroyed?.Invoke();
    }
}
