using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerController : MonoBehaviour
{
   [SerializeField] private Slider _slider;


   public void SetMaxPower(float power)
   {
      _slider.maxValue = power;
      _slider.value = 0f;
   }
   public void SetPower(float power)
   {
      _slider.value = power;
   }
}
