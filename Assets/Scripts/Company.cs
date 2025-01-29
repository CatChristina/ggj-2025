using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Company : MonoBehaviour
{
    // Holy moly
    [Header("Currency")]
    public int money;
    public int moneyPerDay;
    public int costPerDay;
    public int profit;
    public int loss;

    [Header("Legality")]
    public int publicRelations;
    public int morality;
    public int criminality;
    [SerializeField] private string companyName;

    [Header("Time")]
    public int day;
    public int month;
    public int year;
    public int timeScale;

    [Header("Components")]
    public Slider prSlider;
    public Slider moralSlider;
    public Slider crimSlider;
    private Graph graph;

    [SerializeField] private List<int> itemsToSell;

    [Header("Fast Forward")]
    public GameObject button;
    public Sprite normalSpeed;
    public Sprite fastSpeed;
    public Sprite maxSpeed;

    [Header("UI")]
    public TMP_Text date;
    public TMP_Text currentMoney;
    public GameObject eventBox;
    public GameObject choiceEventBox;
    public GameObject FBIBox;
    private int eventCooldown;
    public GameObject purchaseObj;
    public Color positiveColor;
    public Color negativeColor;
    [SerializeField] private Image pauseButton;

    [Header("Company Overview")]
    public TMP_Text moneyTMP;
    public TMP_Text incomeTMP;
    public TMP_Text expensesTMP;
    public GameObject currentResearch;
    public ResearchItem researchItem;

    [Header("End Game")]
    public GameObject endGameScreen;

    [SerializeField] private TMP_Text researchName;
    private Slider researchSlider;

    private void Start()
    {
        profit = 0; loss = 0; publicRelations = 50; morality = 50; criminality = 0; costPerDay = 0;
        eventCooldown = 90;
        day = 1; month = 1; year = 1980;

        itemsToSell = new List<int>();

        graph = GameObject.FindGameObjectWithTag("Graph").GetComponent<Graph>();

        UserInterface();
        Invoke(nameof(ProgressTime), 1);

        researchName = currentResearch.transform.Find("Text (TMP)").GetComponent<TMP_Text>();
        researchSlider = currentResearch.transform.Find("Slider").GetComponent<Slider>();

        InitialRent();
    }

    private void InitialRent()
    {
        costPerDay += 10;
    }

    public void PauseTime()
    {
        int tempTimeScale = timeScale;
        if (timeScale != 0)
        {
            timeScale = 0;
            pauseButton.color = negativeColor;
        }
        else
        {
            timeScale = 1;
            button.transform.Find("Image").GetComponent<Image>().sprite = normalSpeed;
            pauseButton.color = Color.white;
        }

        CancelInvoke();
        ProgressTime();

        if (researchItem != null)
        {
            researchItem.DoResearch();
        }
    }

    private void ProgressTime()
    {
        if (timeScale != 0)
        {
            day++;

            if (day == 31)
            {
                day = 1;
                month++;
                profit += money;
                money -= loss;
                costPerDay += 10;

                if (graph.valueList.Count == 12)
                {
                    graph.NewYear();
                }
                graph.valueList.Add(profit - loss);
                graph.GenerateGraph(graph.valueList);

                profit = 0;
                loss = 0;
            }

            if (month == 13)
            {
                month = 1;
                year++;

                if (criminality > 85)
                {
                    FBIEvent();
                }

                if (year == 2011)
                {
                    EndGame();
                }

                costPerDay += Mathf.CeilToInt(costPerDay * 1.1f);
            }

            SellItems();
            GainMoneyPerDay();
            LoseMoneyPerDay();
            UserInterface();

            eventCooldown--;

            if (Random.Range(0, 125) < 1 && eventCooldown <= 0)
            {
                if (Random.Range(0, 3) <= 1)
                {
                    NormalEvent();
                    eventCooldown = 50;
                }
                else if (eventCooldown <= 0)
                {
                    ChoiceEvent();
                    eventCooldown = 50;
                }
            }

            Invoke(nameof(ProgressTime), (1f / timeScale));
            return;
        }

        Invoke(nameof(ProgressTime), 1f);
    }

    // This code sucks lol
    public void ChangeTimeScale()
    {
        if (timeScale >= 9)
        {
            timeScale = 1;
            button.transform.Find("Image").GetComponent<Image>().sprite = normalSpeed;
        }
        else if (timeScale != 0)
        {

            if (timeScale == 1)
            {
                button.transform.Find("Image").GetComponent<Image>().sprite = fastSpeed;
            }
            else if (timeScale == 3)
            {
                button.transform.Find("Image").GetComponent<Image>().sprite = maxSpeed;
            }

            timeScale *= 3;
        }
        else
        {
            timeScale = 1;
            button.transform.Find("Image").GetComponent<Image>().sprite = normalSpeed;

            if (researchItem != null)
            {
                researchItem.DoResearch();
            }
        }
        pauseButton.color = Color.white;
    }

    private void SellItems()
    {
        if (itemsToSell.Count > 0 && Random.Range(0, 5) <= 1)
        {
            money += itemsToSell[0];
            itemsToSell.Remove(itemsToSell[0]);
        }
    }

    public void IncreaseIncomePerDay(int amount)
    {
        moneyPerDay += amount;
    }

    private void GainMoneyPerDay()
    {
        profit += moneyPerDay;
    }

    private void LoseMoneyPerDay()
    {
        loss += costPerDay;
    }

    public void UserInterface()
    {
        ResetNegativeValues();

        prSlider.value = publicRelations;
        moralSlider.value = morality;
        crimSlider.value = criminality;
        date.text = "Date: " + day + "/" + month + "/" + year;
        moneyTMP.text = $"${profit}";
        expensesTMP.text = $"-${loss}";
        incomeTMP.text = $"${moneyPerDay}";
        currentMoney.text = $"Funds: ${money - loss}";

        if (money - loss < 0)
        {
            currentMoney.color = negativeColor;

            currentMoney.text = moneyTMP.text.Replace("-", "");

            currentMoney.text = $"Funds: ${money - loss}";
        }
        else
        {
            moneyTMP.color = positiveColor;
            currentMoney.color = positiveColor;
        }
    }

    private void ResetNegativeValues()
    {
        if (publicRelations > 100)
        {
            publicRelations = 100;
        }
        else if (publicRelations < 0)
        {
            publicRelations = 0;
        }

        if (morality > 100)
        {
            morality = 100;
        }
        else if (morality < 0)
        {
            morality = 0;
        }

        if (criminality > 100)
        {
            criminality = 100;
        }
        else if (criminality < 0)
        {
            criminality = 0;
        }
    }

    private void NormalEvent()
    {
        eventBox.SetActive(true);
        PauseTime();
    }

    private void ChoiceEvent()
    {
        choiceEventBox.SetActive(true);
        PauseTime();
    }

    private void FBIEvent()
    {
        FBIBox.SetActive(true);
        PauseTime();
    }

    public void Event()
    {
        UserInterface();
        timeScale = 1;
        ChangeTimeScale();
        researchItem.DoResearch();
    }

    public void BuyItem(BaseEventData data)
    {
        ItemData itemData = data.selectedObject.GetComponent<ItemData>();
        int cost = itemData.cost;
        int salePrice = itemData.salePrice;
        int stock = itemData.stock;

        if (money >= loss)
        {
            loss += cost;

            for (int i = 0; i < stock; i++)
            {
                itemsToSell.Add(salePrice);
            }

            GameObject obj = Instantiate(purchaseObj, data.selectedObject.transform);
            obj.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").GetComponent<Transform>(), false);
            obj.transform.position = Input.mousePosition;
            TMP_Text tmp = obj.GetComponent<TMP_Text>();
            tmp.text = $"-${cost}";
            tmp.color = negativeColor;
            Destroy(obj, 1f);

            UserInterface();
        }
    }

    public void ChangeCompanyName()
    {
        companyName = GameObject.FindGameObjectWithTag("CompanyName").GetComponent<TMP_InputField>().text;
    }

    public void UpdateHomeResearch()
    {
        researchName.text = researchItem.itemName;
        researchSlider.maxValue = researchItem.researchRequired;
        researchSlider.value = researchItem.research;
    }

    public void TakeLoan(BaseEventData data)
    {
        if (data == null)
        {
            return;
        }
        LoanStats loan = data.selectedObject.GetComponent<LoanStats>();
        money += loan.loanAmount;
        costPerDay += loan.loanAmount / 100;
        loan.gameObject.GetComponent<Button>().interactable = false;

        UserInterface();
    }

    private void EndGame()
    {
        endGameScreen.SetActive(true);
        endGameScreen.transform.Find("Description").GetComponent<TMP_Text>().text = "Over four decades, " + companyName + " accumulated $" + money.ToString();
        PauseTime();
        crimSlider.transform.parent.transform.parent = endGameScreen.transform;
        crimSlider.transform.parent.transform.localScale *= 2;
        GameObject obj = endGameScreen.transform.Find("Button").gameObject;
        obj.transform.parent = crimSlider.transform;
        obj.transform.parent = endGameScreen.transform;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }
}
