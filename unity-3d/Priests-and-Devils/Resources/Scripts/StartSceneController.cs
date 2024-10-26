using DevilBoatGame;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneController : MonoBehaviour
{
    private GameObject canvas;
    private GameObject coverImage;
    private Texture2D buttonTexture;
    private GUIStyle buttonStyle;

    // �������� Controller �ű�
    public Controller controllerGameObject;

    private bool isGameStarted = false;

    void Start()
    {
        // ���ذ�ť����
        buttonTexture = Resources.Load<Texture2D>("Textures/ButtonImage");
        if (buttonTexture == null)
        {
            Debug.LogError("ButtonImage not found. Make sure the image is placed in 'Assets/Resources/Textures' and the path is correct.");
            return;
        }

        // ��ʼ����ť��ʽ
        buttonStyle = new GUIStyle();
        buttonStyle.normal.background = buttonTexture; // ���ð�ť�ı���ͼƬ
        buttonStyle.fontSize = 24; // ���������С
        buttonStyle.alignment = TextAnchor.MiddleCenter; // �������ֶ��뷽ʽ
        buttonStyle.normal.textColor = Color.black; // ����������ɫ
        buttonStyle.hover.textColor = Color.yellow;  // ��ͣʱ������ɫΪ��ɫ

        CreateUI();
    }

    // ���� UI Ԫ��
    private void CreateUI()
    {
        // ���� Canvas
        canvas = new GameObject("Canvas");
        Canvas c = canvas.AddComponent<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.AddComponent<CanvasScaler>();
        canvas.AddComponent<GraphicRaycaster>();

        // ��������ͼƬ
        coverImage = new GameObject("CoverImage");
        coverImage.transform.SetParent(canvas.transform);
        Image img = coverImage.AddComponent<Image>();

            // ���÷���ͼƬ·��
            Texture2D texture = Resources.Load<Texture2D>("Textures/CoverImage");
        if (texture == null)
        {
            Debug.LogError("CoverImage not found. Make sure the image is placed in 'Assets/Resources/Textures' and the path is correct.");
            return; // ��ǰ���أ��������ִ��
        }
        img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        // ���� RectTransform �Ը���������Ļ
        RectTransform rectTransform = img.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero; // ���½�ê�� (0, 0)
        rectTransform.anchorMax = Vector2.one; // ���Ͻ�ê�� (1, 1)
        rectTransform.offsetMin = Vector2.zero; // ���½�ƫ��
        rectTransform.offsetMax = Vector2.zero; // ���Ͻ�ƫ��
        rectTransform.anchoredPosition = Vector2.zero; // ȷ��λ�þ���

        // ȷ�� Raycast Target ����
        img.raycastTarget = false; // ��ֹ����ͼƬ���ǰ�ť�ĵ��


        Debug.Log("UI �������");
    }

    void OnGUI()
    {
        // �����Ϸ��δ��������ʾ��ʼ��ť
        if (!isGameStarted)
        {
            // ����һ�������Զ�����ʽ�İ�ť
            if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.65f, Screen.width * 0.2f, Screen.height * 0.1f), "��ʼ��Ϸ", buttonStyle))
            {
                Debug.Log("��ť����������� Controller �ű�");
                // ���� Controller �ű�
                if (controllerGameObject == null)
                {
                    controllerGameObject = gameObject.AddComponent<Controller>();
                }

                // �����Ϸ�����������ذ�ť
                isGameStarted = true;

                // ���ؿ�ʼҳ��� UI
                canvas.SetActive(false);
            }
        }
    }
}
