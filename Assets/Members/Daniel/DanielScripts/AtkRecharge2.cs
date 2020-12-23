using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AtkRecharge2 : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        slider.value = 0;
    }

    public void Recharge (float cooldown)
    {
        slider.value = 0 - cooldown;
    }
}
