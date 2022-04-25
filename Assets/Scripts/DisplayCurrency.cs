using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCurrency : MonoBehaviour
{
    public string value = "$0";
    public Text currency;
    void Start()
    {
        currency.text = value;
    }
    void Update()
    {
        currency.text = GetComponentInParent<InventorySystem>().money.ToString();
    }
}
