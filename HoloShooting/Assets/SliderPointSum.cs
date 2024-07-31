using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Examples.Demos;
using TMPro;

public class SliderPointSum : MonoBehaviour {

    [SerializeField]
    public int statusPoint = 10;

    [SerializeField]
    public PinchSlider speedSlider = null;

    [SerializeField]
    public PinchSlider intervalSlider = null;

    [SerializeField]
    public PinchSlider sizeSlider = null;

    [SerializeField]
    private TextMeshPro textMesh = null;

    public void OnSliderReleased(PinchSlider slider) {
        // feat.Oki
        var sum = speedSlider.SliderValue + intervalSlider.SliderValue + sizeSlider.SliderValue;
        var max = statusPoint - (sum - slider.SliderValue);
        if (sum > statusPoint) {
            slider.SliderValue = max;
        }
    }

    public void OnValueUpdated() {
        var sum = speedSlider.SliderValue + intervalSlider.SliderValue + sizeSlider.SliderValue;
        var remain = statusPoint - sum;
        textMesh.SetText(remain.ToString());
        if (remain < 0) {
            textMesh.color = Color.red;
        }
        else {
            textMesh.color = Color.white;
        }
    }
}
