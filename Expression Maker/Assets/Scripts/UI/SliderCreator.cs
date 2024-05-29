using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniVRM10.VRM10Viewer;

public class SliderCreator : MonoBehaviour
{
    public GameObject sliderSetPrefab;
    public SliderAreaExpand sliderAreaExpand;
    public SlidersSets[] slidersSets;

    public void CreateSlider(GameObject humanoid)
    {
        if (humanoid != null)
        {
            List<string> blendShapes = new List<string>();

            // Get the SkinnedMeshRenderer component of the humanoid
            SkinnedMeshRenderer skinnedMeshRenderer = humanoid.GetComponentInChildren<SkinnedMeshRenderer>();

            slidersSets = new SlidersSets[skinnedMeshRenderer.sharedMesh.blendShapeCount];
            // Get the blend shapes of the SkinnedMeshRenderer
            int i = 0;
            for (i = 0; i < skinnedMeshRenderer.sharedMesh.blendShapeCount; i++)
            {
                blendShapes.Add(EditName(skinnedMeshRenderer.sharedMesh.GetBlendShapeName(i)));

                //Create Slider Set Prefabs
                var sliderSet = Instantiate(sliderSetPrefab, transform).GetComponent<SlidersSets>();
                sliderSet.blendshapeIndex = i;
                sliderSet.humanoid = skinnedMeshRenderer;

                string editedName = blendShapes[i];
                sliderSet.gameObject.name = EditName(editedName);

                slidersSets[i] = sliderSet;
            }

            sliderAreaExpand.UpdateSliderAreaHeight(i);
        }
    }

    public string EditName(string input)
    {
        var inputString = input + " ";
        inputString = inputString.Replace("_", " ");
        inputString = inputString.Replace("Fcl ", "");
        inputString = inputString.Replace("ALL", "Full");
        inputString = inputString.Replace("BRW", "Brow");
        inputString = inputString.Replace("MTH", "Mouth");
        inputString = inputString.Replace("EYE", "Eye");
        inputString = inputString.Replace("HA ", "Teeth ");
        inputString = inputString.Replace("Fung", "Fang ");
        inputString = inputString.Replace(" L ", " Left");
        inputString = inputString.Replace(" R ", " Right");

        return inputString;
    }

    public void ClearSliders()
    {
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();
        slidersSets = null;

        // Loop through the children and delete them
        foreach (Transform child in children)
        {
            if (child.gameObject != gameObject)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void SetSlidersValue(float value)
    {
        SlidersSets[] children = gameObject.GetComponentsInChildren<SlidersSets>();

        // Loop through the children and delete them
        foreach (SlidersSets child in children)
        {
            child.SetSliderValue(value);
            child.ResetTimeValue();
        }
    }
}
