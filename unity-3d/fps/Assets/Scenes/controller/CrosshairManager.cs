using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public Canvas canvas;          // Canvas ���ڳ��� UI Ԫ��
    public GameObject crosshair;   // ׼�ǵĸ����壨ͨ���ǿյ� GameObject����Ϊ׼�ǵ�������

    void Start()
    {
        // ��ʼ��ʱ������ʽ1��׼��
        CreateStyle1();
    }

    // ��ʽ 1�����ĵ� + ʮ��׼��
    void CreateStyle1()
    {
        GameObject centerPoint = CreatePoint(Color.red, new Vector2(5f, 5f));
        GameObject crossHorizontal = CreateLine(Color.red, new Vector2(50f, 5f)); // ����
        GameObject crossVertical = CreateLine(Color.red, new Vector2(5f, 50f)); // ����

        // ����ЩԪ������Ϊ׼��������������
        crossHorizontal.transform.SetParent(crosshair.transform, false);
        crossVertical.transform.SetParent(crosshair.transform, false);
        centerPoint.transform.SetParent(crosshair.transform, false);

        // ����Ԫ�صľֲ�λ����Ϊ�㣬ȷ�����Ǿ��ж���
        crossHorizontal.transform.localPosition = Vector3.zero;
        crossVertical.transform.localPosition = Vector3.zero;
        centerPoint.transform.localPosition = Vector3.zero;
    }

    // ��ʽ 2��ʮ��׼��
    void CreateStyle2()
    {
        GameObject crossHorizontal = CreateLine(Color.green, new Vector2(50f, 5f)); // ����
        GameObject crossVertical = CreateLine(Color.green, new Vector2(5f, 50f)); // ����

        // ����ЩԪ������Ϊ׼��������������
        crossHorizontal.transform.SetParent(crosshair.transform, false);
        crossVertical.transform.SetParent(crosshair.transform, false);

        // ����Ԫ�صľֲ�λ����Ϊ�㣬ȷ�����Ǿ��ж���
        crossHorizontal.transform.localPosition = Vector3.zero;
        crossVertical.transform.localPosition = Vector3.zero;
    }

    // ��ʽ 3�����ĵ� + Բ��׼��
    void CreateStyle3()
    {
        GameObject centerPoint = CreatePoint(Color.blue, new Vector2(5f, 5f));
        GameObject circle = CreateCircle(Color.blue, 30f); // ʹ�� Line ����Բ��

        // ����ЩԪ������Ϊ׼��������������
        circle.transform.SetParent(crosshair.transform, false);
        centerPoint.transform.SetParent(crosshair.transform, false);

        // ����Ԫ�صľֲ�λ����Ϊ�㣬ȷ�����Ǿ��ж���
        centerPoint.transform.localPosition = Vector3.zero;
        circle.transform.localPosition = Vector3.zero;
    }

    // ����׼�ǵ����ĵ㣨С�㣩
    GameObject CreatePoint(Color color, Vector2 size)
    {
        GameObject point = new GameObject("CenterPoint");
        Image image = point.AddComponent<Image>();
        image.color = color;
        image.rectTransform.sizeDelta = size;

        return point;
    }

    // ����׼�ǵ�����
    GameObject CreateLine(Color color, Vector2 size)
    {
        GameObject line = new GameObject("Line");
        Image image = line.AddComponent<Image>();
        image.color = color;
        image.rectTransform.sizeDelta = size;

        return line;
    }

    // ����һ��Բ��Ч����ͨ�����С�߶�ģ��Բ�Σ�
    GameObject CreateCircle(Color color, float radius)
    {
        GameObject circle = new GameObject("Circle");
        int segments = 36; // Բ�ηֶ������ֶ�Խ��ԽԲ
        float angleStep = 360f / segments; // ÿ��С�߶εĽǶ�

        // ����ÿ���߶�
        for (int i = 0; i < segments; i++)
        {
            GameObject segment = CreateLine(color, new Vector2(1f, radius));
            segment.transform.SetParent(circle.transform, false);

            // ����ÿ���߶ε���ת�Ƕ�
            segment.transform.localRotation = Quaternion.Euler(0, 0, i * angleStep);
            segment.transform.localPosition = Vector3.zero;
        }

        return circle;
    }

    // ��̬�л�׼����ʽ
    public void ChangeCrosshairStyle(int style)
    {
        // �����ǰ��׼��
        foreach (Transform child in crosshair.transform)
        {
            Destroy(child.gameObject);
        }

        // ����ѡ�����ʽ����׼��
        switch (style)
        {
            case 1:
                CreateStyle1();
                break;
            case 2:
                CreateStyle2();
                break;
            case 3:
                CreateStyle3();
                break;
            default:
                CreateStyle1(); // Ĭ��ʹ����ʽ1
                break;
        }
    }
}
