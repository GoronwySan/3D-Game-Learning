using UnityEngine;
using Cinemachine;

public class AimController : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera; // �������� Cinemachine Virtual Camera
    private float normalFOV;                        // �洢��ʼ�� FOV ֵ
    public float aimFOV = 20f;                      // ��׼���� FOV
    public float transitionDuration = 0.1f;         // FOV ƽ�����ɵ�ʱ�䣨0.5�룩
    //2
    private float targetFOV;                        // Ŀ�� FOV�����ڹ���
    private float currentFOV;                       // ��ǰ FOV ֵ

    void Start()
    {
        // ��ȡ�����е�һ�� CinemachineVirtualCamera ����
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        if (virtualCamera != null)
        {
            // ��ȡ��ʼ�� FOV ֵ
            normalFOV = virtualCamera.m_Lens.FieldOfView;
            currentFOV = normalFOV;  // ��ʼ��ʱ�ĵ�ǰ FOV Ϊ�����ӽ�
            targetFOV = normalFOV;   // ��ʼ��ʱ��Ŀ�� FOV Ϊ�����ӽ�
        }
        else
        {
            Debug.LogError("CinemachineVirtualCamera not found in the scene!");
        }
    }

    void Update()
    {
        // �����׼�������Ҽ���
        if (Input.GetMouseButtonDown(1)) // �Ҽ�����ʱ����ʼ��׼
        {
            targetFOV = aimFOV;  // ����Ŀ�� FOV Ϊ��׼�� FOV
        }
        else if (Input.GetMouseButtonUp(1)) // �Ҽ��ɿ�ʱ���ָ���ͨ�ӽ�
        {
            targetFOV = normalFOV;  // ����Ŀ�� FOV Ϊ���� FOV
        }

        // ƽ������ FOV ֵ
        if (virtualCamera != null)
        {
            currentFOV = Mathf.MoveTowards(currentFOV, targetFOV, (Mathf.Abs(targetFOV - currentFOV) / transitionDuration) * Time.deltaTime);
            virtualCamera.m_Lens.FieldOfView = currentFOV;
        }
    }
}
