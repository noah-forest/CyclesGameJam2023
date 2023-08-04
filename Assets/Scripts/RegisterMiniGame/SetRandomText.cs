using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class SetRandomText : MonoBehaviour
{
    public string[] dialogueChoices;

    public void ChooseDialogueOption(TextMeshPro text)
    {
        var index = Random.Range(0, dialogueChoices.Length);
        text.SetText(dialogueChoices[index]);
    }
}
