using UnityEngine;
using UnityEngine.UI;

public class ToggleUIPanel : MonoBehaviour
{
    public RectTransform panelRectTransform; // UI面板的RectTransform组件
    public float openPosition = 0f; // 打开时的位置
    public float closedPosition = -200f; // 收起时的位置
    public float transitionDuration = 0.5f; // 动画过渡时间
    public Sprite opensprite;
    public Sprite closedsprite;
    public Image buttonimage;

    public bool isPanelOpen = false; // 标记面板是否打开

    // 调用此方法来切换面板的打开/收起状态
    public void TogglePanel()
    {
        isPanelOpen = !isPanelOpen; // 切换状态
        if (isPanelOpen)
        {
            buttonimage.sprite = opensprite;
        }
        else
        {
            buttonimage.sprite = closedsprite;
        }
        StartCoroutine(MovePanel()); // 开始动画协程
    }

    // 动画协程，用于平滑移动面板
    private System.Collections.IEnumerator MovePanel()
    {
        float elapsedTime = 0f;
        float start = panelRectTransform.anchoredPosition.y; // 起始位置
        float end = isPanelOpen ? openPosition : closedPosition; // 目标位置

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            panelRectTransform.anchoredPosition = new Vector2(panelRectTransform.anchoredPosition.x, Mathf.Lerp(start, end, t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终位置准确
        panelRectTransform.anchoredPosition = new Vector2(panelRectTransform.anchoredPosition.x, end);
    }
}