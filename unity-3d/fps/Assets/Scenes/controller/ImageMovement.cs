using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ImageMovement : MonoBehaviour
{
    public Canvas canvas;  // ��Ҫ����Canvas
    private bool isMoving; // ������ʶ�Ƿ����ƶ�
    public Transform targetObject;   // ��Ҫ���ٵ�Ŀ�����
    private Vector3 targetPosition; // Ŀ��λ��
    private Vector3 old;

    // ���ڹ���ͼ����ʾ״̬
    //private bool imagesVisible = true;

    private List<GameObject> group1Images = new List<GameObject>(); // �洢 1, 2, 3, 4 ���ͼ��
    private List<GameObject> group2Images = new List<GameObject>(); // �洢 5, 6, 7, 8 ���ͼ��


    void Start()
    {
        // ��ʼ������ȡCanvas�µ�����Image����
        Image[] images = canvas.GetComponentsInChildren<Image>();

        foreach (Image img in images)
        {
            // ��ʼ��ÿ��Image��ԭʼλ��
            img.gameObject.AddComponent<MovementData>();
            MovementData data = img.GetComponent<MovementData>();
            data.originalPosition = img.rectTransform.localPosition;

            if (img.name == "1" || img.name == "2" || img.name == "3" || img.name == "4")
            {
                group1Images.Add(img.gameObject);
            }
            else if (img.name == "5" || img.name == "6" || img.name == "7" || img.name == "8")
            {
                group2Images.Add(img.gameObject);
                img.gameObject.SetActive(false);
            }

            // ���������Ϣ
            Debug.Log("Image " + img.name + " initialized at position: " + data.originalPosition);
        }
    }

    void Update()
    {
        // ��¼֮ǰ��λ�ò��ж��Ƿ����ƶ�
        Vector3 neww = targetObject.position;

        // ���Ŀ��λ�ñ仯������Ϊ�����ƶ�
        if (neww != old)
        {
            isMoving = true;
            old = neww;
        }
        else
        {
            isMoving = false;
        }
        move();

    }
    
    void move()
    {
        // ��ȡCanvas�����е�Image����
        Image[] images = canvas.GetComponentsInChildren<Image>();

        foreach (Image img in images)
        {
            MovementData data = img.GetComponent<MovementData>();



            // ����Image���ֽ��в�ͬ��λ�ƴ���
            if (isMoving)
            {
                if (img.name == "1")
                {
                    // ͼ��1��λ������ (0, 30, 0)
                    targetPosition = data.originalPosition + new Vector3(0, 30, 0);
                    Debug.Log("Image " + img.name + " moving up. Target position: " + targetPosition);
                }
                else if (img.name == "2")
                {
                    // ͼ��2��λ�ü��� (0, 30, 0)
                    targetPosition = data.originalPosition - new Vector3(0, 30, 0);
                    Debug.Log("Image " + img.name + " moving down. Target position: " + targetPosition);
                }
                else if (img.name == "3")
                {
                    // ͼ��3��λ������ (30, 0, 0)
                    targetPosition = data.originalPosition + new Vector3(30, 0, 0);
                    Debug.Log("Image " + img.name + " moving right. Target position: " + targetPosition);
                }
                else if (img.name == "4")
                {
                    // ͼ��4��λ�ü��� (30, 0, 0)
                    targetPosition = data.originalPosition - new Vector3(30, 0, 0);
                    Debug.Log("Image " + img.name + " moving left. Target position: " + targetPosition);
                }
                else if (img.name == "5")
                {
                    // ͼ��1��λ������ (0, 30, 0)
                    targetPosition = data.originalPosition + new Vector3(-15, 15, 0);
                    Debug.Log("Image " + img.name + " moving up. Target position: " + targetPosition);
                }
                else if (img.name == "6")
                {
                    // ͼ��2��λ�ü��� (0, 30, 0)
                    targetPosition = data.originalPosition - new Vector3(15, 15, 0);
                    Debug.Log("Image " + img.name + " moving down. Target position: " + targetPosition);
                }
                else if (img.name == "7")
                {
                    // ͼ��3��λ������ (30, 0, 0)
                    targetPosition = data.originalPosition + new Vector3(15, 15, 0);
                    Debug.Log("Image " + img.name + " moving right. Target position: " + targetPosition);
                }
                else if (img.name == "8")
                {
                    // ͼ��4��λ�ü��� (30, 0, 0)
                    targetPosition = data.originalPosition - new Vector3(-15, 15, 0);
                    Debug.Log("Image " + img.name + " moving left. Target position: " + targetPosition);
                }
                else
                {
                    continue;
                }

                // ʹ��Lerp��ƽ������
                img.rectTransform.localPosition = Vector3.Lerp(img.rectTransform.localPosition, targetPosition, 0.5f); // ���Ե����ٶ�ϵ��
            }
            else
            {
                //// ���û���ƶ�����ԭ
                img.rectTransform.localPosition = Vector3.Lerp(img.rectTransform.localPosition, data.originalPosition, 0.5f);
            }
        }
    }

    // �����������л�ͼ����ʾ״̬
    // �л�ͼ����ʾ״̬
    private bool isbool = true;
    public void SwitchImages()
    {
        // �л� 1, 2, 3, 4 �� 5, 6, 7, 8 ����ʾ״̬
        foreach (GameObject img in group1Images)
        {
            img.SetActive(!isbool); // ��� 5, 6, 7, 8 �ɼ��������� 1, 2, 3, 4
        }

        foreach (GameObject img in group2Images)
        {
            img.SetActive(isbool); // ��� 1, 2, 3, 4 �ɼ��������� 5, 6, 7, 8
        }
        isbool = !isbool;
    }
}


// ���ڴ洢ÿ��Image�����ԭʼλ��
public class MovementData : MonoBehaviour
{
    public Vector3 originalPosition;
}
