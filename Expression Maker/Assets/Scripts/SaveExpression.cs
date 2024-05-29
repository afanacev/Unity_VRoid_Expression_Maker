using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class SaveExpression : MonoBehaviour
{
    public SkinnedMeshRenderer vrmMesh;
    public ScreenshotTaker screenshot;
    public string fileName = "Facial Expression";
    public string filePath;

    public List<BlendshapeValue> blendshapeList;
    public SliderCreator sliderCreator;
    public DropdownsController dropdownsController;

    public bool OnSaveClicked()
    {
        //Clear previous save files
        filePath = null;
        fileName = null;

#if UNITY_EDITOR
        filePath = EditorUtility.SaveFilePanel("Save Facial Expression", "", "Facial Expression 1", "fcl");
#elif UNITY_STANDALONE_WIN
        filePath = VRM10FileDialogForWindows.SaveDialog("Save Facial Expression", "Facial Expression 1" + ".fcl");
#else
        filePath = Application.persistentDataPath + "/" + fileName + ".fcl";
#endif

        //Analyze filePath and add extension if possible
        if (filePath.Length > 0 && filePath.Substring(filePath.Length - 4, 4) != ".fcl")
        {
            filePath += ".fcl";
        }

        Debug.Log(filePath);

        if (!string.IsNullOrEmpty(filePath))
        {
            var temp = Path.GetFileName(filePath);
            fileName = temp.Substring(0, temp.Length - 5);

            GetBlendshapes();
            SaveToJSON();

            screenshot.TakeScreenshot(filePath.Substring(0, filePath.Length - 4));
            return true;
        }

        return false;
    }



    private void GetBlendshapes()
    {
        blendshapeList.Clear();

        int blendShapeCount = vrmMesh.sharedMesh.blendShapeCount;

        // Loop through all blendshapes
        for (int i = 0; i < blendShapeCount; i++)
        {
            // Get the current blendshape name and value
            string blendShapeName = vrmMesh.sharedMesh.GetBlendShapeName(i);
            int blendShapeIndex = vrmMesh.sharedMesh.GetBlendShapeIndex(blendShapeName);
            float blendShapeValue = vrmMesh.GetBlendShapeWeight(i);

            // If the blendshape value is not 0, add it to the animation clip
            if (blendShapeValue != 0)
            {
                var slidersSets = sliderCreator.slidersSets.First(x => x.blendshapeIndex == blendShapeIndex);
                blendshapeList.Add(new BlendshapeValue(blendShapeName, Mathf.Round(blendShapeValue), blendShapeIndex, slidersSets.GetTime()));
            }
        }
    }

    private void SaveToJSON()

    {
        //Because Unity doesn't allow serializing JSON with list directly as its root
        //I have to use a class that contains the list
        FacialFile facialFile = new FacialFile(blendshapeList, 
            (CharacterNameEnum)dropdownsController.CharacterNameDropdown.value, 
            (FaceExpressionEnum)dropdownsController.CharacterNameDropdown.value);

        // Serialize the list to a JSON string.
        string json = JsonUtility.ToJson(facialFile);

        // Save the JSON string to a file.
        System.IO.File.WriteAllText(filePath, json);
    }

    public void UpdateVRMMesh(GameObject target)
    {
        vrmMesh = target.transform.Find("Face").GetComponent<SkinnedMeshRenderer>();
    }
}

[System.Serializable]
public class FacialFile
{
    public CharacterNameEnum CharacterName;
    public FaceExpressionEnum EmotionName;
    public List<BlendshapeValue> root;

    public FacialFile(List<BlendshapeValue> blendshapeList, CharacterNameEnum characterName, FaceExpressionEnum emotionName)
    {
        this.root = blendshapeList;
        CharacterName = characterName;
        EmotionName = emotionName;
    }
}


[System.Serializable]
public class BlendshapeValue
{
    public string name;
    public float value;
    public int index;
    public float time;

    public BlendshapeValue(string name, float value, int index, float time)
    {
        this.name = name;
        this.value = value;
        this.index = index;
        this.time = time;
    }
}