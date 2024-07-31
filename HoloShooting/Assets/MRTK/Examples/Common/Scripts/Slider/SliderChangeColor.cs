// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.Examples.Demos
{
    [AddComponentMenu("Scripts/MRTK/Examples/SliderChangeColor")]
    public class SliderChangeColor : MonoBehaviour
    {
        [SerializeField]
        private Renderer TargetRenderer;
        [SerializeField]
        private int scaleFactor = 100;
        [SerializeField]
        private int sliderStepDivisions = 10;

        public void OnSliderUpdatedRed(SliderEventData eventData)
        {
            //TargetRenderer = GetComponentInChildren<Renderer>();
            //if ((TargetRenderer != null) && (TargetRenderer.material != null))
            //{
            //    TargetRenderer.material.color = new Color(eventData.NewValue, TargetRenderer.sharedMaterial.color.g, TargetRenderer.sharedMaterial.color.b);
            //}
        }

        public void OnSliderUpdatedGreen(SliderEventData eventData)
        {
            //TargetRenderer = GetComponentInChildren<Renderer>();
            //if ((TargetRenderer != null) && (TargetRenderer.material != null))
            //{
            //    TargetRenderer.material.color = new Color(TargetRenderer.sharedMaterial.color.r, eventData.NewValue, TargetRenderer.sharedMaterial.color.b);
            //}
        }

        public void OnSliderUpdateBlue(SliderEventData eventData)
        {
            Transform objectTransform = GetComponent<Transform>();
            float size = (eventData.NewValue + (0.1f / sliderStepDivisions * scaleFactor)) / scaleFactor;
            objectTransform.localScale = new Vector3(size, size, size);
            //TargetRenderer = GetComponentInChildren<Renderer>();
            //if ((TargetRenderer != null) && (TargetRenderer.material != null))
            //{
            //    TargetRenderer.material.color = new Color(TargetRenderer.sharedMaterial.color.r, TargetRenderer.sharedMaterial.color.g, eventData.NewValue);
            //}
        }
    }
}