using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public List<EventData> events = new List<EventData>();
    private EventData chosenEvent;

    public TMP_Text eventDesc;
    public TMP_Text effects;
    private int money, pr, moral, crim;

    public TMP_Text breakingNews;

    private Company comp;

    private void OnEnable()
    {
        chosenEvent = events[Random.Range(0, events.Count)];
        eventDesc.text = chosenEvent.eventText;
        money = chosenEvent.money;
        pr = chosenEvent.pr;
        moral = chosenEvent.moral;
        crim = chosenEvent.crim;

        comp = GameObject.FindGameObjectWithTag("Company").GetComponent<Company>();

        effects.text = "$" + Mathf.CeilToInt(money * (comp.publicRelations / 20)).ToString() + " Public Relations: " + pr.ToString() + " Morality: " + moral + "  Criminality: "+ crim.ToString();

        comp.money += Mathf.CeilToInt(money * (comp.publicRelations / 20));
        comp.publicRelations += pr;
        comp.morality += moral;
        comp.criminality += crim;

        breakingNews.text = chosenEvent.eventText;
        //events.Remove(chosenEvent);
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
        comp.Event();
    }
}