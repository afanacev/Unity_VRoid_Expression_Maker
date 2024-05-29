using System;
using TMPro;
using UnityEngine;

public class DropdownsController : MonoBehaviour
{
    public TMP_Dropdown CharacterNameDropdown;
    public TMP_Dropdown EmotionNameDropdown;

    public void Start()
    {
        FillCharacterNameDropdown();
        FillEmotionNameDropDown();
    }

    private void FillCharacterNameDropdown()
    {
        var enumValues = Enum.GetValues(typeof(CharacterNameEnum));

        CharacterNameDropdown.ClearOptions();

        foreach (var enumValue in enumValues)
        {
            var option = new TMP_Dropdown.OptionData(enumValue.ToString());
            CharacterNameDropdown.options.Add(option);
        }

        CharacterNameDropdown.value = 0;
        CharacterNameDropdown.RefreshShownValue();
    }
    
    private void FillEmotionNameDropDown()
    {
        var enumValues = Enum.GetValues(typeof(FaceExpressionEnum));

        EmotionNameDropdown.ClearOptions();

        foreach (var enumValue in enumValues)
        {
            var option = new TMP_Dropdown.OptionData(enumValue.ToString());
            EmotionNameDropdown.options.Add(option);
        }

        CharacterNameDropdown.value = 0;
        CharacterNameDropdown.RefreshShownValue();
    }
}
