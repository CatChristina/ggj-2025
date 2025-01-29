using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Graph : MonoBehaviour
{
    [SerializeField] private Sprite pointSprite;
    [SerializeField] private int pointSize;
    [SerializeField] private int lineThickness;

    private RectTransform axisX;
    private RectTransform axisY;

    private RectTransform guideX;
    private RectTransform guideY;

    public Color pointColor = new Color(1, 1, 1, 1);
    public Color lineColorPositive = new Color(0, 1, 0, 1);
    public Color lineColorNegative = new Color(1, 0, 0, 1);

    private RectTransform graphContainer;

    private string[] months;

    public List<int> valueList;
    private Animator animator;

    private void Awake()
    {
        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
        axisX = graphContainer.transform.Find("AxisX").GetComponent<RectTransform>();
        axisY = graphContainer.transform.Find("AxisY").GetComponent<RectTransform>();
        guideX = graphContainer.transform.Find("GuideX").GetComponent<RectTransform>();
        guideY = graphContainer.transform.Find("GuideY").GetComponent<RectTransform>();
        animator = GetComponent<Animator>();

        months = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        GenerateGraph(valueList);
    }

    public void GenerateGraph(List<int> values)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMax = Mathf.Max(values.ToArray());
        float xLength = graphContainer.sizeDelta.x / 13;
        Vector2 lastPointPosition = Vector2.zero;

        for (int i  = 0; i < graphContainer.childCount; i++)
        {
            var child = graphContainer.GetChild(i);

            if (child.gameObject.activeInHierarchy)
            {
                Destroy(child.gameObject);
            }
        }

        for (int i = 0; i < values.Count; i++)
        {
            float xPosition = xLength + i * xLength;
            float yPosition = (values[i] / yMax) * graphHeight;
            
            if (yPosition > graphHeight - 32)
            {
                yPosition = graphHeight - 32;
            }

            if (yPosition < 96)
            {
                yPosition = 96;
            }
            Vector2 currentPointPosition = new Vector2(xPosition, yPosition);
            CreatePoint(currentPointPosition);

            if (i > 0)
            {
                CreateLine(lastPointPosition, currentPointPosition);
            }

            lastPointPosition = currentPointPosition;

            RectTransform labelX = Instantiate(axisX, graphContainer);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, 30);

            RectTransform labelY = Instantiate(axisY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            labelY.anchoredPosition = new Vector2(xPosition, yPosition + 32);
            labelY.GetComponent<TMP_Text>().text = $"${values[i]}";

            /*RectTransform dashX = Instantiate(guideX, graphContainer);
            dashX.SetParent(graphContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, 38);
            */

            if (i >= 0 && i <= months.Length)
            {
                labelX.GetComponent<TMP_Text>().text = months[i];
            }
        }

        for (int i = 0; i < values.Count; i++)
        {
            /*RectTransform labelY = Instantiate(axisY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / values.Count;
            labelY.anchoredPosition = new Vector2(32, ((graphHeight / values.Count) + (values[i])));
            labelY.GetComponent<TMP_Text>().text = $"${values[i]}";
            */

            /*RectTransform dashY = Instantiate(guideY, graphContainer);
            dashY.SetParent(graphContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(32, (graphHeight * normalizedValue));
            */
        }
    }

    private void CreatePoint(Vector2 anchorPos)
    {
        GameObject obj = new GameObject("Point", typeof(Image));
        obj.transform.SetParent(graphContainer, false);
        obj.GetComponent<Image>().sprite = pointSprite;
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        obj.GetComponent<Image>().color = pointColor;
        rectTransform.anchoredPosition = anchorPos;
        rectTransform.sizeDelta = new Vector2(pointSize, pointSize);
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;
        rectTransform.anchoredPosition = anchorPos;
    }

    private void CreateLine(Vector2 posA, Vector2 posB)
    {
        GameObject obj = new GameObject("Line", typeof(Image));
        obj.transform.SetParent(graphContainer, false);
        RectTransform rectTransform = obj.GetComponent<RectTransform>();

        Vector2 dir = (posB - posA).normalized;
        float distance = Vector2.Distance(posA, posB);
        rectTransform.sizeDelta = new Vector2(distance, lineThickness);
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;
        rectTransform.pivot = new Vector2(0f, 0.5f);
        rectTransform.anchoredPosition = posA;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rectTransform.rotation = Quaternion.Euler(0, 0, angle);

        obj.GetComponent<Image>().color = posA.y <= posB.y ? lineColorPositive : lineColorNegative;
    }

    public void ToggleGraph()
    {
        animator.SetBool("Open", !animator.GetBool("Open"));
        GenerateGraph(valueList);
    }

    public void NewYear()
    {
        valueList.Clear();
    }
}
