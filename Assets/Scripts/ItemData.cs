using TMPro;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public string itemName;
    public int cost;
    public int salePrice;
    public int stock;

    private Company comp;
    private int year;

    private void Start()
    {
        comp = GameObject.FindGameObjectWithTag("Company").GetComponent<Company>();
        year = comp.year;
        UpdateText();
        InvokeRepeating(nameof(Inflation), 30, 30);
        PriceCollapse();
    }

    public void UpdateText()
    {
        transform.Find("Name").GetComponent<TMP_Text>().text = itemName;
        transform.Find("Cost").GetComponent<TMP_Text>().text = $"${cost}";
        transform.Find("Value").GetComponent<TMP_Text>().text = $"${salePrice}";
        transform.Find("Quantity").GetComponent<TMP_Text>().text = $"{stock}";
    }

    public void Inflation()
    {
        if (comp.year != year)
        {
            cost += cost / 20;
            salePrice += salePrice / 20;
            year = comp.year;
            UpdateText();
        }
    }

    public void PriceCollapse()
    {
        if (comp.year == 2000 || comp.year >= 2008)
        {
            cost /= 2;
            salePrice /= 2;

            if (UnityEngine.Random.Range(0, 15) < 1)
            {
                salePrice *= 8;
            }

            UpdateText();
        }
        Invoke(nameof(PriceCollapse), 45);
    }
}
