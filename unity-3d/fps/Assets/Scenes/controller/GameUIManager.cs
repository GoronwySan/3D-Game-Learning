using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class GameUIManager : MonoBehaviour
{
    // UIԪ��
    private Image healthBarImage;
    private TextMeshProUGUI healthText;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI timeText;
    private TextMeshProUGUI arrowCountText;  // ������ʾ��������
    private Image arrowIcon;  // ������ʾͼ�꣨������������

    // ��ҽű�����
    private Player player;

    // ��������������
    private ScoreManager scoreManager;

    private float gameTime = 0f;

    // UIԪ�ص��Զ���λ�ñ���
    private Vector2 healthBarPosition = new Vector2(-400, 20);
    private Vector2 healthTextPosition = new Vector2(200, 80);
    private Vector2 scoreTextPosition = new Vector2(-220, -5);
    private Vector2 timeTextPosition = new Vector2(30, -17);

    // �����ļ�·��aa
    public string fontPath = "Fonts/NotoSansSC-Black SDF";  // �����ļ�·��
    private CrossbowController crossbowController;
    // ������Դ
    private TMP_FontAsset customFont;

    void Start()
    {
        // �����Զ�������
        customFont = Resources.Load<TMP_FontAsset>(fontPath);
        // ��̬����UIԪ��
        CreateUIElements();

        // ��ȡ��Һͷ���������������
        player = FindObjectOfType<Player>();
        scoreManager = FindObjectOfType<ScoreManager>();
        crossbowController = FindObjectOfType<CrossbowController>(); // ��ȡCrossbowController������


        // ��ʼ��UI
        UpdateHealthUI();
        UpdateScoreUI();
        //UpdateTimeUI();
        UpdateArrowUI();  // ���¼���UI
    }

    void Update()
    {
        // ������Ϸʱ��
        gameTime += Time.deltaTime;
        //UpdateTimeUI();

        // ����Ѫ��UI
        UpdateHealthUI();

        // ���·���UI
        UpdateScoreUI();

        UpdateArrowUI();  // ���¼���UI
    }

    // ����UIԪ��
    void CreateUIElements()
    {
        // ���� Canvas
        GameObject canvasObj = new GameObject("Canvas");
        canvasObj.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        Canvas canvas = canvasObj.GetComponent<Canvas>();
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        // ����Ѫ���� (Image ģ��)
        GameObject healthBarObj = new GameObject("HealthBar");
        healthBarObj.transform.SetParent(canvasObj.transform);
        healthBarImage = healthBarObj.AddComponent<Image>();
        RectTransform healthBarRect = healthBarObj.GetComponent<RectTransform>();
        healthBarRect.sizeDelta = new Vector2(80, 30); // �趨Ѫ������С
        // ������תΪ (0, 0, 180)
        healthBarRect.rotation = Quaternion.Euler(0, 0, 180);
        // ���� pivot Ϊ (1, 0.5)����ʾ���ұߵ��м�λ��
        healthBarRect.pivot = new Vector2(1f, 0.5f);
        // ���� anchorMax �� anchorMin��ȷ��ͼ���ê��ʹ�С����Ӱ��
        healthBarRect.anchorMin = new Vector2(0, 0.0f); // ê�����߽翪ʼ
        healthBarRect.anchorMax = new Vector2(0.4f, 0f); // ê�㵽�ұ߽�
        healthBarRect.anchoredPosition = healthBarPosition; // ʹ���Զ���λ��
        healthBarImage.color = Color.red; // ������ɫΪ��ɫ

        // ����Ѫ������ (TextMeshPro)
        GameObject healthTextObj = new GameObject("HealthText");
        healthTextObj.transform.SetParent(canvasObj.transform);
        healthText = healthTextObj.AddComponent<TextMeshProUGUI>();
        healthText.fontSize = 36;
        healthText.font = customFont; // ����Ϊ�Զ�������
        healthText.alignment = TextAlignmentOptions.Left;
        RectTransform healthTextRect = healthTextObj.GetComponent<RectTransform>();
        healthTextRect.sizeDelta = new Vector2(400, 30);
        healthTextRect.anchorMin = new Vector2(0, 0);
        healthTextRect.anchorMax = new Vector2(0, 0);
        healthTextRect.anchoredPosition = healthTextPosition; // ʹ���Զ���λ��

        // ������������ (TextMeshPro)
        GameObject scoreTextObj = new GameObject("ScoreText");
        scoreTextObj.transform.SetParent(canvasObj.transform);
        scoreText = scoreTextObj.AddComponent<TextMeshProUGUI>();
        scoreText.fontSize = 36;
        scoreText.font = customFont; // ����Ϊ�Զ�������
        scoreText.alignment = TextAlignmentOptions.TopRight;
        RectTransform scoreTextRect = scoreTextObj.GetComponent<RectTransform>();
        scoreTextRect.sizeDelta = new Vector2(400, 30);
        scoreTextRect.anchorMin = new Vector2(1, 1);
        scoreTextRect.anchorMax = new Vector2(1, 1);
        scoreTextRect.anchoredPosition = scoreTextPosition; // ʹ���Զ���λ��

        // ����ʱ������ (TextMeshPro)
        //GameObject timeTextObj = new GameObject("TimeText");
        //timeTextObj.transform.SetParent(canvasObj.transform);
        //timeText = timeTextObj.AddComponent<TextMeshProUGUI>();
        //timeText.fontSize = 36;
        //timeText.font = customFont; // ����Ϊ�Զ�������
        //timeText.alignment = TextAlignmentOptions.Center;
        //RectTransform timeTextRect = timeTextObj.GetComponent<RectTransform>();
        //timeTextRect.sizeDelta = new Vector2(400, 30);
        //timeTextRect.anchorMin = new Vector2(0.5f, 1);
        //timeTextRect.anchorMax = new Vector2(0.5f, 1);
        //timeTextRect.anchoredPosition = timeTextPosition; // ʹ���Զ���λ��

        // �������������� (TextMeshPro)
        GameObject arrowCountObj = new GameObject("ArrowCountText");
        arrowCountObj.transform.SetParent(canvasObj.transform);
        arrowCountText = arrowCountObj.AddComponent<TextMeshProUGUI>();
        arrowCountText.fontSize = 36;
        arrowCountText.font = customFont; // ����Ϊ�Զ�������
        arrowCountText.alignment = TextAlignmentOptions.BottomRight;
        RectTransform arrowCountRect = arrowCountObj.GetComponent<RectTransform>();
        arrowCountRect.sizeDelta = new Vector2(200, 30);
        arrowCountRect.anchorMin = new Vector2(1, 0);
        arrowCountRect.anchorMax = new Vector2(1, 0);
        arrowCountRect.anchoredPosition = new Vector2(-100, 20);  // ���������½�ƫ��λ��

        // ������ͼ�� (Image) - ������ʾ�������ʱ��ͼ��
        GameObject arrowIconObj = new GameObject("ArrowIcon");
        arrowIconObj.transform.SetParent(canvasObj.transform);
        arrowIcon = arrowIconObj.AddComponent<Image>();
        RectTransform arrowIconRect = arrowIconObj.GetComponent<RectTransform>();
        arrowIconRect.sizeDelta = new Vector2(50, 50); // ͼ���С
        arrowIconRect.anchorMin = new Vector2(1, 0);  // �������½�
        arrowIconRect.anchorMax = new Vector2(1, 0);
        arrowIconRect.anchoredPosition = new Vector2(-100, 30);  // ���������½�ƫ��λ��
        arrowIcon.enabled = false;  // Ĭ�ϲ���ʾͼ��
    }

    // ����Ѫ��UI
    void UpdateHealthUI()
    {
        if (player == null) return;

        // ��ȡ��ǰѪ��
        float currentHealth = player.GetCurrentHealth();
        float healthPercentage = currentHealth / player.maxHealth;  // Ѫ���ٷֱ�

        // ����Ѫ�������
        healthBarImage.rectTransform.localScale = new Vector3(healthPercentage, 1f, 1f);  // ����������

        // ����Ѫ���ٷֱ�����̬�ı���ɫ
        healthBarImage.color = Color.Lerp(Color.red, Color.green, healthPercentage);

        // ����Ѫ������
        healthText.text = "Ѫ��: " + currentHealth.ToString("F0") + " / " + player.maxHealth.ToString("F0");
    }



    // ���·���UI
    void UpdateScoreUI()
    {
        if (scoreManager == null) return;

        // ���·�������
        scoreText.text = "����: " + scoreManager.GetScore().ToString();
    }

    // ���¼���UI
    void UpdateArrowUI()
    {
        if (crossbowController == null)
        {
            Debug.Log("dafdsads");
            return;
        }

        int remainingArrows = crossbowController.GetRemainingArrows();  // ��ȡʣ�������
        Debug.Log("asdfasdk" + remainingArrows);

        // �ж��Ƿ����������������������ʾͼ�꣬���ؼ�������
        if (crossbowController.CanShootArrow())
        {
            arrowIcon.enabled = false;  // ����ͼ��
            arrowCountText.text = "ʣ���: " + remainingArrows.ToString();  // ��ʾ��������
            arrowCountText.enabled = true;  // ��ʾ��������
        }
        else
        {
            arrowIcon.enabled = true;   // ��ʾͼ�꣨�������ʱ��
            arrowCountText.enabled = false;  // ���ؼ���������ʾ
                                             // ����ͼ�꣨���粻�������ͼ�꣩
            arrowIcon.sprite = Resources.Load<Sprite>("stop");  // ����ͼ�꣨��ȷ��ͼ��·����ȷ��
        }
    }


    // ������Ϸʱ��UI
    void UpdateTimeUI()
    {
        int minutes = Mathf.FloorToInt(gameTime / 60f);
        int seconds = Mathf.FloorToInt(gameTime % 60f);
        timeText.text = "ʱ��: " + minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
