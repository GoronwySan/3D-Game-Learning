using UnityEngine;
using UnityEngine.UI;

public class StartSceneController : MonoBehaviour
{
    private GameObject canvas;
    private GameObject coverImage;
    private Texture2D buttonTexture;
    private GUIStyle buttonStyle;

    // ������Ҫ��ӵĽű�
    private FirstController firstController;
    private CCActionManager ccActionManager;
    private PickupObject pickupObject;
    private View view;
    private GroundController GR;

    private bool isGameStarted = false;

    void Start()
    {
        // ���ذ�ť����
        buttonTexture = Resources.Load<Texture2D>("Textures/ButtonImage");
        if (buttonTexture == null)
        {
            Debug.LogError("ButtonImage not found. Make sure the image is placed in 'Assets/Scenes/Textures' and the path is correct.");
            return;
        }

        // ��ʼ����ť��ʽ
        buttonStyle = new GUIStyle
        {
            normal = { background = buttonTexture, textColor = Color.black },
            fontSize = 24,
            alignment = TextAnchor.MiddleCenter,
            hover = { textColor = Color.yellow }
        };

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
            Debug.LogError("CoverImage not found. Make sure the image is placed in 'Assets/Scenes/Textures' and the path is correct.");
            return;
        }
        img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        // ���� RectTransform �Ը���������Ļ
        RectTransform rectTransform = img.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.anchoredPosition = Vector2.zero;

        img.raycastTarget = false;

        Debug.Log("UI �������");
    }

    void OnGUI()
    {
        // �����Ϸ��δ��������ʾ��ʼ��ť
        if (!isGameStarted)
        {
            if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.65f, Screen.width * 0.2f, Screen.height * 0.1f), "��ʼ��Ϸ", buttonStyle))
            {
                Debug.Log("��ť������������ű�");

                // ���������Ҫ�Ľű�
                AddControllerScripts();

                // �����Ϸ�����������ذ�ť
                isGameStarted = true;

                // ���ؿ�ʼҳ��� UI
                canvas.SetActive(false);
            }
        }
    }

    private void AddControllerScripts()
    {
        //firstController = gameObject.AddComponent<FirstController>();

        //ccActionManager = gameObject.AddComponent<CCActionManager>();
        pickupObject = gameObject.AddComponent<PickupObject>();
        view = gameObject.AddComponent<View>();
        GR = gameObject.AddComponent<GroundController>();

        // ��˳���ʼ����ȷ�������ű��������
        //firstController.Init(SSDirector.getInstance());
        //ccActionManager.Init();
        pickupObject.Init();
        view.Init();

        Debug.Log("���п��ƽű��ѳɹ����ز���ʼ����");
    }

}
