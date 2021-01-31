using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Subtitles : MonoBehaviour
{
    public TextMeshProUGUI textBox;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Story());
    }

    IEnumerator Story()
    {
        yield return new WaitForSeconds(1);
        textBox.text = "A terrible news struck the house of the Dubois family.";
        yield return new WaitForSeconds(4);
        textBox.text = "The story of the death of Madame Dubois supposedly cut up by her husband intrigued our detective.";
        yield return new WaitForSeconds(4);
        textBox.text = "Detective Stun, persuaded that Mr Dubois is innocent, offered his services and went to the crime scene.";
        yield return new WaitForSeconds(7);
        textBox.text = "The evidence proving Mr Dubois' innocence being hidden in a room he must search the house.";
        yield return new WaitForSeconds(5);
        textBox.text = "By looking for that evidence, he’s gonna find something that’s gonna change everything about his investigation.";
        yield return new WaitForSeconds(5);
        textBox.enabled = false;
    }
}
