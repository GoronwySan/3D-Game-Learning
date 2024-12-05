using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionView : MonoBehaviour
{
    public event Action<int> OnLevelSelectedEvent;  // ����һ���¼�

    public GameObject levelSelectionPanel;  // �������
    private LevelModel levelModel;

    private void Awake()
    {
        // ʹ�Æ���ģʽֱ�ӫ@ȡ LevelModel ����
        levelModel = LevelModel.Instance;

        if (levelModel == null)
        {
            Debug.LogError("LevelModel not found! Ensure the LevelModel is properly initialized.");
            return;
        }

        Debug.Log("LevelModel successfully loaded.");

        levelSelectionPanel = CreatePanel();

        Debug.Log("Level Selection Panel created.");

        CreateLevelButtons(levelSelectionPanel.transform);

        levelSelectionPanel.SetActive(false);
    }

    // �����������
    private GameObject CreatePanel()
    {
        // ����һ���µ� Canvas
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080); // �����ֱ��ʣ��������H��r�O��
        canvasObj.AddComponent<GraphicRaycaster>(); // ������߼���Դ��� UI �¼�

        // �������
        GameObject panelObj = new GameObject("LevelSelectionPanel");
        panelObj.transform.SetParent(canvasObj.transform);  // �������ص� Canvas ��

        RectTransform rectTransform = panelObj.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(1200, 800);  // ��������С
        panelObj.AddComponent<Image>().color = new Color(0, 0, 0, 0.7f); // ���ñ���ɫ

        // ׌�������Ļ�����@ʾ
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);  // �i�������c
        rectTransform.anchoredPosition = Vector2.zero;

        return panelObj;
    }

    // �����P�����o
    private void CreateLevelButtons(Transform parent)
    {
        // ���o�Ĵ�С
        Vector2 buttonSize = new Vector2(300, 140);
        int buttonsPerRow = 3;  // ÿ���Ă����o
        float horizontalSpacing = 10f;  // ˮƽ�g��
        float verticalSpacing = 10f;    // ��ֱ�g��

        // Ӌ�㿂�Д�
        int totalRows = Mathf.CeilToInt((float)levelModel.TotalLevels / buttonsPerRow);

        // ����ÿ���P���İ��o
        for (int i = 0; i < levelModel.TotalLevels; i++)
        {
            Debug.Log($"Creating button for Level {i + 1}...");

            // �������o�K�O�������
            Button levelButton = CreateButton(parent, $"�ؿ� {i + 1}");
            RectTransform buttonRect = levelButton.GetComponent<RectTransform>();
            buttonRect.sizeDelta = buttonSize;  // �O�����o��С

            // Ӌ��ÿ�����o��λ��
            int row = i / buttonsPerRow; // �@�ǰ��o���ڵ���
            int col = i % buttonsPerRow; // �@�ǰ��o���ڵ���

            float xPos = col * (buttonSize.x + horizontalSpacing) - (buttonsPerRow * (buttonSize.x + horizontalSpacing) / 2) + 150; // Ӌ��X����
            float yPos = -row * (buttonSize.y + verticalSpacing) + (totalRows * (buttonSize.y + verticalSpacing) / 2) - 150; // Ӌ��Y����

            buttonRect.anchoredPosition = new Vector2(xPos, yPos);  // �O�����o��λ��

            // �������i��B�O�����o
            Text buttonText = levelButton.GetComponentInChildren<Text>();
            if (levelModel.IsLevelUnlocked(i))
            {
                levelButton.interactable = true;
                buttonText.color = Color.white;  // �ѽ��i�@ʾ���ɫ
                levelButton.onClick.AddListener(() => OnLevelSelected(i));

                Debug.Log($"Level {i + 1} is unlocked. Button is interactable.");
            }
            else
            {
                levelButton.interactable = false;
                buttonText.color = Color.gray;  // δ���i�@ʾ���ɫ

                Debug.Log($"Level {i + 1} is locked. Button is non-interactive.");
            }
        }
    }

    // �����Զ��x���o
    private Button CreateButton(Transform parent, string buttonText)
    {
        // �������o�� GameObject
        GameObject buttonObj = new GameObject(buttonText);
        buttonObj.transform.SetParent(parent);

        // ��� RectTransform �K�O�ô�С
        RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(300, 140); // �O�����o��С

        // �����ı�
        GameObject textObj = new GameObject();
        textObj.transform.SetParent(buttonObj.transform);
        Text text = textObj.AddComponent<Text>();
        text.text = buttonText;
        text.fontSize = 50;  // �O�����w��С��������Ҫ�{��
        text.font = Resources.Load<Font>("Fonts/NotoSansSC-Black"); // ʹ�ÃȽ����w�����Զ��x���w��
        text.color = Color.black;
        text.alignment = TextAnchor.MiddleCenter;

        // �O���ı����С
        RectTransform textRectTransform = textObj.GetComponent<RectTransform>();
        textRectTransform.sizeDelta = new Vector2(300, 140);  // �O���ı��򹠇�

        // �O�����o�����DƬ
        RawImage buttonImage = buttonObj.AddComponent<RawImage>();
        buttonImage.texture = Resources.Load<Texture>("ButtonImage"); // ���d ButtonImage.png �DƬ���_����λ� Resources �Y�ϊA��
        buttonImage.color = Color.white; // �_���DƬ���ɫ�����ɔ_�DƬ�@ʾ

        // ��� Button �M���K�O������
        Button button = buttonObj.AddComponent<Button>();
        button.targetGraphic = buttonImage;

        // �O�����o�Ľ���
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;

        return button;
    }




    // �P���x����Ļ��{
    private void OnLevelSelected(int levelIndex)
    {
        levelIndex -= 9;
        Debug.Log($"Level {levelIndex + 1 - 10 } selected.");
        // �@�e�����|�l�������d����������
        OnLevelSelectedEvent?.Invoke(levelIndex);  // �����¼���֪ͨ������
    }
}
