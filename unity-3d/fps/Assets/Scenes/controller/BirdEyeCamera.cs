using UnityEngine;
using UnityEngine.UI;

public class BirdEyeCamera : MonoBehaviour
{
    public Camera birdEyeCamera;     // ���ͼ����ͷ
    public Transform targetObject;   // ��Ҫ���ٵ�Ŀ�����
    public float height = 20f;       // ���ͼ����ͷ�ĸ߶�



    void Start()
    {
        birdEyeCamera = GetComponent<Camera>();
        //if (birdEyeCamera == null)
        //{
        //    birdEyeCamera = gameObject.AddComponent<Camera>();
        //}

        // �������ͼ��ʾ����Ļ���Ͻ�
        birdEyeCamera.rect = new Rect(0f, 0.7f, 0.3f, 0.3f); // Viewport Rect

    }

    void Update()
    {
        if (targetObject != null)
        {
            // ��ȡĿ������λ��
            Vector3 targetPosition = targetObject.position;

            // �������ͼ����ͷ��λ��
            // ����Ը�����Ҫ��������ͷ�ĸ߶�
            birdEyeCamera.transform.position = new Vector3(targetPosition.x, height, targetPosition.z);

            // �����ͼ����ͷʼ�ճ���Ŀ�����
            birdEyeCamera.transform.LookAt(targetObject);
        }
    }
}
