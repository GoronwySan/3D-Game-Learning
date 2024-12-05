using UnityEngine;
using UnityEngine.UI;

public class StartMenuView : MonoBehaviour
{
    public GameObject startMenuPanel;
    public Button startGameButton;

    private void Awake()
    {
        // ���� Canvas
        Canvas canvas = CreateCanvas();

        // ���� StartMenuPanel
        startMenuPanel = CreatePanel(canvas.transform);

        // ���� StartGame ���o
        startGameButton = CreateButton(startMenuPanel.transform, "��ʼ��Ϸ");
    }

    // ���� Canvas
    private Canvas CreateCanvas()
    {
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080); // �����ֱ��ʣ��������H��r�O��

        canvasObj.AddComponent<GraphicRaycaster>();  // ������@������̎�� UI ���o�¼�
        return canvas;
    }

    // �������
    private GameObject CreatePanel(Transform parent)
    {
        GameObject panelObj = new GameObject("StartMenuPanel");
        panelObj.transform.SetParent(parent);

        RectTransform rectTransform = panelObj.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(2160, 1600); // �O������С

        // ���� RawImage ���@ʾ�DƬ����
        RawImage rawImage = panelObj.AddComponent<RawImage>();
        rawImage.texture = Resources.Load<Texture>("backgroundImage"); // ���YԴ�м��d�DƬ
        rawImage.rectTransform.sizeDelta = new Vector2(1920, 1080);  // �{�������D��Ĵ�С��׌�����w�������
        // ׌�������Ļ�����@ʾ
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);  // �i�������c
        rectTransform.anchoredPosition = Vector2.zero;

        return panelObj;
    }


    // �������o
    private Button CreateButton(Transform parent, string buttonText)
    {
        GameObject buttonObj = new GameObject(buttonText);
        buttonObj.transform.SetParent(parent);

        RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(400, 140); // �O�����o��С

        // �O�����o�ı�
        GameObject textObj = new GameObject();
        textObj.transform.SetParent(buttonObj.transform);
        Text text = textObj.AddComponent<Text>();
        text.text = buttonText;
        // �O�����w��С
        text.fontSize = 50;  // �O�����w��С��������Ҫ�{��
        text.font = Resources.Load<Font>("Fonts/NotoSansSC-Black"); // ʹ�ÃȽ����w�����Զ��x���w��
        text.color = Color.black;
        text.alignment = TextAnchor.MiddleCenter;

        // �O���ı����С
        RectTransform textRectTransform = textObj.GetComponent<RectTransform>();
        textRectTransform.sizeDelta = new Vector2(400, 140);  // �O���ı��򹠇�


        // �O�����o�����DƬ
        RawImage buttonImage = buttonObj.AddComponent<RawImage>();
        buttonImage.texture = Resources.Load<Texture>("ButtonImage"); // ���d ButtonImage.png �DƬ���_����λ� Resources �Y�ϊA��

        // �_���DƬ���ɫ�����ɔ_�DƬ�@ʾ
        buttonImage.color = Color.white;

        // �O�����o�Ľ���
        Button button = buttonObj.AddComponent<Button>();
        button.targetGraphic = buttonImage;

        // ׌���oҲλ���Ļ����
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;

        return button;
    }


}
