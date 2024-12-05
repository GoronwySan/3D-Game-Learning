using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public Canvas canvas;          // Canvas 用于承载 UI 元素
    public GameObject crosshair;   // 准星的父物体（通常是空的 GameObject，作为准星的容器）

    void Start()
    {
        // 初始化时创建样式1的准星
        CreateStyle1();
    }

    // 样式 1：中心点 + 十字准星
    void CreateStyle1()
    {
        GameObject centerPoint = CreatePoint(Color.red, new Vector2(5f, 5f));
        GameObject crossHorizontal = CreateLine(Color.red, new Vector2(50f, 5f)); // 横线
        GameObject crossVertical = CreateLine(Color.red, new Vector2(5f, 50f)); // 竖线

        // 将这些元素设置为准星容器的子物体
        crossHorizontal.transform.SetParent(crosshair.transform, false);
        crossVertical.transform.SetParent(crosshair.transform, false);
        centerPoint.transform.SetParent(crosshair.transform, false);

        // 所有元素的局部位置设为零，确保它们居中对齐
        crossHorizontal.transform.localPosition = Vector3.zero;
        crossVertical.transform.localPosition = Vector3.zero;
        centerPoint.transform.localPosition = Vector3.zero;
    }

    // 样式 2：十字准星
    void CreateStyle2()
    {
        GameObject crossHorizontal = CreateLine(Color.green, new Vector2(50f, 5f)); // 横线
        GameObject crossVertical = CreateLine(Color.green, new Vector2(5f, 50f)); // 竖线

        // 将这些元素设置为准星容器的子物体
        crossHorizontal.transform.SetParent(crosshair.transform, false);
        crossVertical.transform.SetParent(crosshair.transform, false);

        // 所有元素的局部位置设为零，确保它们居中对齐
        crossHorizontal.transform.localPosition = Vector3.zero;
        crossVertical.transform.localPosition = Vector3.zero;
    }

    // 样式 3：中心点 + 圆形准星
    void CreateStyle3()
    {
        GameObject centerPoint = CreatePoint(Color.blue, new Vector2(5f, 5f));
        GameObject circle = CreateCircle(Color.blue, 30f); // 使用 Line 创建圆形

        // 将这些元素设置为准星容器的子物体
        circle.transform.SetParent(crosshair.transform, false);
        centerPoint.transform.SetParent(crosshair.transform, false);

        // 所有元素的局部位置设为零，确保它们居中对齐
        centerPoint.transform.localPosition = Vector3.zero;
        circle.transform.localPosition = Vector3.zero;
    }

    // 创建准星的中心点（小点）
    GameObject CreatePoint(Color color, Vector2 size)
    {
        GameObject point = new GameObject("CenterPoint");
        Image image = point.AddComponent<Image>();
        image.color = color;
        image.rectTransform.sizeDelta = size;

        return point;
    }

    // 创建准星的线条
    GameObject CreateLine(Color color, Vector2 size)
    {
        GameObject line = new GameObject("Line");
        Image image = line.AddComponent<Image>();
        image.color = color;
        image.rectTransform.sizeDelta = size;

        return line;
    }

    // 创建一个圆形效果（通过多个小线段模拟圆形）
    GameObject CreateCircle(Color color, float radius)
    {
        GameObject circle = new GameObject("Circle");
        int segments = 36; // 圆形分段数，分段越多越圆
        float angleStep = 360f / segments; // 每个小线段的角度

        // 创建每个线段
        for (int i = 0; i < segments; i++)
        {
            GameObject segment = CreateLine(color, new Vector2(1f, radius));
            segment.transform.SetParent(circle.transform, false);

            // 计算每个线段的旋转角度
            segment.transform.localRotation = Quaternion.Euler(0, 0, i * angleStep);
            segment.transform.localPosition = Vector3.zero;
        }

        return circle;
    }

    // 动态切换准星样式
    public void ChangeCrosshairStyle(int style)
    {
        // 清除当前的准星
        foreach (Transform child in crosshair.transform)
        {
            Destroy(child.gameObject);
        }

        // 根据选择的样式创建准星
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
                CreateStyle1(); // 默认使用样式1
                break;
        }
    }
}
