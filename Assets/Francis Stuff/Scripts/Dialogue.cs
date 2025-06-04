using UnityEngine;
using TMPro;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    private int index;
    private bool isDialogueActive = false;
    //private bool isTyping = false;
    private float oldSpeed;

    void Start()
    {
        textComponent.text = string.Empty;
        oldSpeed = Movement.Instance.getSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isDialogueActive)
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    public void StartDialogue()
    {
        Movement.Instance.setSpeed(0);
        if (isDialogueActive) return;
        isDialogueActive = true;
        index = 0;
        StartCoroutine(TypeLine());
    }

    public IEnumerator TypeLine()
    {
        if (index == 0)
        {
            
            yield return new WaitForSeconds(1.2f);
        }
        else
        {
            yield return new WaitForSeconds(0.25f);
        }

        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else 
        {
            
            isDialogueActive = false;
            DeleteHelper();
        }
    }
    public void DeleteHelper()
    {
        StartCoroutine(DialogDelete());
    }
    
    public IEnumerator DialogDelete()
    {
        Movement.Instance.setSpeed(oldSpeed);
        textComponent.text = string.Empty;
        TextBoxVisual textBoxVisual = gameObject.transform.GetChild(3).GetComponent<TextBoxVisual>();
        textBoxVisual.GoAway();
        //Debug.Log("going away");

        yield return new WaitForSeconds(0.777f);
        //Debug.Log("waited 3 seconds");
        gameObject.SetActive(false);
    }
}
