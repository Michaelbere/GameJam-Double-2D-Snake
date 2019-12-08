using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for managing a bar. 
/// </summary>
public class BarController : MonoBehaviour
{
    private Image _barImage;

//    private bool _fill;

    private BarLogic _barLogic;

    private const float WarningPercentage = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
//        _barImage = transform.Find("DryBar").GetComponent<Image>();
        _barImage = transform.GetComponent<Image>();
        _barLogic = new BarLogic(_barImage.fillAmount);
        GameManager.Instance.Activate();
    }

    // Update is called once per frame
    void Update()
    {
//        UpdateBar(false);
    }

    public void UpdateBar(bool increase)
    {
        if (_barImage == null) return;
        _barLogic.UpdateAmount(increase);
        var currPercentFilled = _barLogic.GetNormalizedAmount();
        _barImage.fillAmount = currPercentFilled;
        //If the percentage is below 30% and going down
        //So as to not flash while on the wrong side
        if (currPercentFilled <= WarningPercentage && !increase)
        {
            EventManager.ChangeColors();
        }
    }

//    public void setBar(float amount)
//    {
//        _barLogic.SetAmount(amount);
//        _barImage.fillAmount = _barLogic.GetNormalizedAmount();
//    }
}

/// <summary>
/// Class Implementing the logic of a Bar.
/// </summary>
public class BarLogic
{
    private const int MaxAmount = 100;
    private float _currentAmount;
    private const float Growth = 10; // Change this to change fill speed

    public BarLogic(float currentAmount)
    {
        //TODO: make this take the fill amount * MaxAmount
        _currentAmount = Mathf.Round(currentAmount * MaxAmount);
    }

    /// <summary>
    /// Updates the amount in the bar according to increase: if true, the value is increase, otherwise decreased.
    /// </summary>
    /// <param name="increase"> Boolean determining whether to increase or decrease the bar</param>
    public void UpdateAmount(bool increase)
    {
        if (increase)
        {
            if (_currentAmount < MaxAmount)
            {
                _currentAmount += Growth * Time.deltaTime;
            }
        }
        else
        {
            if (_currentAmount > 0)
            {
                _currentAmount -= Growth * Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Getter for percent of bar filled.
    /// </summary>
    /// <returns> The percentage of the bar that is currently filled.</returns>
    public float GetNormalizedAmount()
    {
        return _currentAmount / MaxAmount;
    }

//    public void SetAmount(float amount)
//    {
//        _currentAmount = amount;
//    }
}