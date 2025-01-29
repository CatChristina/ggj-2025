using System;
using TMPro;
using UnityEngine;

public class StockMarket : MonoBehaviour
{
    private Company comp;
    private int year;

    [Header("Stocks")]
    public int stockValue;
    public float maxPriceChange;
    private int stockOffset;
    public int ownedStocks;

    [Header("UI")]
    public string stockName;
    public TMP_Text stockTMP;
    public TMP_Text stockPrice;
    public Color positiveColor;
    public Color negativeColor;

    [Header("Purchase")]
    public GameObject buyPrefab;

    [Header("Home Screen")]
    public TMP_Text investment;
    public TMP_Text investmentPrice;
    public TMP_Text investmentAmount;
    public TMP_Text stockOffsetTMP;

    private void Start()
    {
        comp = GameObject.FindGameObjectWithTag("Company").GetComponent<Company>();
        CrashStock();
        UserInterface();
        InvokeRepeating(nameof(CalculateStockValue), 5, 5);
    }

    public void CalculateStockValue()
    {
        float tempValue1 = stockValue;
        float tempValue2 = stockValue;
        float minValue = stockValue - (int)((Convert.ToSingle(tempValue1) / 100f) * maxPriceChange);
        float maxValue = stockValue + (int)((Convert.ToSingle(tempValue2) / 100f) * maxPriceChange);

        stockOffset = (int)(UnityEngine.Random.Range(minValue, maxValue) - stockValue);
        stockValue += stockOffset;
        UserInterface();
    }

    public void BuyStocks()
    {
        if (comp.money >= stockValue)
        {
            ownedStocks++;
            comp.loss += stockValue;

            GameObject obj = Instantiate(buyPrefab);
            obj.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").GetComponent<Transform>(), false);
            obj.transform.position = Input.mousePosition;
            TMP_Text tmp = obj.GetComponent<TMP_Text>();
            tmp.text = $"-${stockValue}";
            tmp.color = negativeColor;
            Destroy(obj, 1f);
            comp.UserInterface();
            UserInterface();
        }
    }

    public void SellStocks()
    {
        if (ownedStocks > 0)
        {
            ownedStocks--;
            comp.profit += stockValue;

            GameObject obj = Instantiate(buyPrefab);
            obj.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").GetComponent<Transform>(), false);
            obj.transform.position = Input.mousePosition;
            TMP_Text tmp = obj.GetComponent<TMP_Text>();
            tmp.text = $"+${stockValue}";
            tmp.color = positiveColor;
            Destroy(obj, 1f);
            comp.UserInterface();
            UserInterface();
        }
    }

    private void UserInterface()
    {
        stockTMP.text = stockName;
        stockPrice.text = "$" + stockValue.ToString();
        investment.text = stockName;
        investmentPrice.text = "$" + stockValue.ToString();
        investmentAmount.text = ownedStocks.ToString();

        if (stockOffset < 0)
        {
            stockOffsetTMP.color = negativeColor;
            stockOffsetTMP.text = stockOffset.ToString();
        }
        else if (stockOffset >= 0)
        {
            stockOffsetTMP.color = positiveColor;
            stockOffsetTMP.text = "+" + stockOffset.ToString();
        }
        else
        {
            stockOffsetTMP.color = Color.white;
        }
    }

    private void CrashStock()
    {
        if (comp.year == 2000 || comp.year >= 2008)
        {
            stockOffset = -stockValue / 2;
            stockValue /= 2;

            if (UnityEngine.Random.Range(0, 15) < 1)
            {
                RecoverStock();
            }

            UserInterface();
        }
        Invoke(nameof(CrashStock), 45);
    }

    private void RecoverStock()
    {
        stockOffset = stockValue *= 20;
        stockValue *= 20;
        UserInterface();
    }
}