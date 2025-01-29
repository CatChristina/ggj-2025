using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "Scriptable Object / Event")]
public class EventData : ScriptableObject
{
    public string eventTitle;
    [TextArea(3, 7)]
    public string eventText;
    public int money, pr, moral, crim;
}