using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ChoiceEventController : MonoBehaviour
{
    public List<ChoiceEventData> events = new List<ChoiceEventData>();
    private ChoiceEventData chosenChoiceEvent;

    private Company comp;
    public TMP_Text eventDesc;
    private int money, pr, moral, crim;

    public TMP_Text button1;
    public TMP_Text button2;
    public TMP_Text breakingNews;
    private void OnEnable()
    {
        chosenChoiceEvent = events[Random.Range(0, events.Count)];
        comp = GameObject.FindGameObjectWithTag("Company").GetComponent<Company>();

        eventDesc.text = chosenChoiceEvent.eventText;
        button1.text = chosenChoiceEvent.option1ButtonName;
        button2.text = chosenChoiceEvent.option2ButtonName;

        breakingNews.text = chosenChoiceEvent.eventText;
        //events.Remove(chosenChoiceEvent);
    }

    public void Choice1()
    {
        ChosenOption(chosenChoiceEvent.money, chosenChoiceEvent.pr, chosenChoiceEvent.moral, chosenChoiceEvent.crim, chosenChoiceEvent.dailyIncome);
    }

    public void Choice2()
    {
        ChosenOption(chosenChoiceEvent.money2, chosenChoiceEvent.pr2, chosenChoiceEvent.moral2, chosenChoiceEvent.crim2, chosenChoiceEvent.dailyIncome2);
    }

    private void ChosenOption(int curr, int pr, int moral, int crim, int dailyIncome)
    {
        if (curr >= 0)
        {
            comp.money += Mathf.CeilToInt(curr * (comp.publicRelations / 20));
        }
        else
        {
            comp.money += curr;
        }
        comp.publicRelations += pr;
        comp.morality += moral;
        comp.criminality += crim;
        comp.IncreaseIncomePerDay(dailyIncome);
        comp.UserInterface();

        CloseMenu();
    }

    private void CloseMenu()
    {
        gameObject.SetActive(false);
        comp.Event();
    }
}
