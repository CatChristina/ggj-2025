using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchItem : MonoBehaviour
{
    [Header("Research")]
    public string itemName;
    public int research;
    public int researchRequired;
    private bool researched = false;
    public Color finishedColor;

    [Header("Impact")]
    public int moneyPerDay;
    public int costPerDay;
    public int publicRelations;
    public int morality;
    public int criminality;

    [Header("Unlocks")]
    public GameObject[] researchItem;
    public GameObject[] unlockedItems;
    private Slider slider;
    private Company comp;

    public static bool isResearching;
    public static ResearchItem currentResearch;

    private Button button;

    private void Start()
    {
        transform.Find("Text (TMP)").GetComponent<TMP_Text>().text = itemName;
        slider = transform.Find("Slider").GetComponent<Slider>();
        comp = GameObject.FindGameObjectWithTag("Company").GetComponent<Company>();
        button = gameObject.GetComponent<Button>();

        slider.maxValue = researchRequired;

        if (gameObject.name != "Button")
        {
            button.interactable = false;
        }
    }

    public void StartResearch()
    {
        if (researched || (currentResearch != this && isResearching))
        {
            return;
        }
        else
        {
            DoResearch();

            var colors = button.colors;
            colors.normalColor = finishedColor + new Color(0.1f, 0.3f, 0.1f);
            colors.selectedColor = finishedColor + new Color(0.1f, 0.3f, 0.1f);
            colors.pressedColor = finishedColor + new Color(0.1f, 0.3f, 0.1f);
            colors.highlightedColor = finishedColor + new Color(0.1f, 0.3f, 0.1f);
            button.colors = colors;
        }
    }

    public void DoResearch()
    {
        research++;
        isResearching = true;
        slider.value = research;
        CheckResearch();
        comp.researchItem = this;
        comp.UpdateHomeResearch();
        currentResearch = this;

        if (!researched)
        {
            if (comp.timeScale != 0)
            {
                CancelInvoke();
                Invoke(nameof(DoResearch), (1f / comp.timeScale));
                return;
            }
        }
        return;
    }

    private void CheckResearch()
    {
        if (research >= researchRequired)
        {
            researched = true;
            if (researchItem.Length != 0)
            {
                for (int i = 0; i < researchItem.Length; i++)
                {
                    researchItem[i].SetActive(true);
                }
            }
            gameObject.GetComponent<Button>().interactable = false;

            var colors = button.colors;
            colors.normalColor = Color.white;
            colors.disabledColor = finishedColor;
            button.colors = colors;
            
            Company comp = GameObject.FindGameObjectWithTag("Company").GetComponent<Company>();
            comp.publicRelations += publicRelations;
            comp.morality += morality;
            comp.criminality += criminality;
            comp.UserInterface();
            comp.moneyPerDay += moneyPerDay;
            comp.costPerDay += costPerDay;

            foreach(GameObject obj in unlockedItems)
            {
                obj.GetComponent<Button>().interactable = true;
            }

            CancelInvoke();
            isResearching = false;
            currentResearch = null;
        }
    }
}
