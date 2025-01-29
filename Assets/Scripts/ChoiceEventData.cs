using UnityEngine;

[CreateAssetMenu(fileName = "New Choice Event", menuName = "Scriptable Object / Choice Event")]
public class ChoiceEventData : ScriptableObject
{
    public string eventTitle;
    [TextArea(3, 7)]
    public string eventText;
    [Header("Option 1 Chosen")]
    public string option1ButtonName;
    public int money;
    public int pr;
    public int moral;
    public int crim;
    public int dailyIncome;
    [Header("Option 2 Chosen")]
    public string option2ButtonName;
    public int money2;
    public int pr2;
    public int moral2;
    public int crim2;
    public int dailyIncome2;
}