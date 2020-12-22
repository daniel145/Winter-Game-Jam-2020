using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class atkRecharge : MonoBehaviour
{
    public Slider slider; 

    public void SetCD(int cdLength)
    {
        slider.minValue = cdLength;
        slider.value = 0;
    }

    public void Recharge (float cooldown)
    {
        slider.value = 0 - cooldown;
    }
}
