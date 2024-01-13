using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RadioButtonBehaviour : MonoBehaviour
{
    private GameObject[] buttons;
    void Start()
    {
        //find all game objects with the tag "RadioButton"
        buttons = GameObject.FindGameObjectsWithTag("RadioButton");
    }

    public void HandleRadioButtonClick(GameObject button)
    {
        foreach (GameObject b in buttons)
        {
            if (b != button)
            {
                Button buttonComponent = b.GetComponent<Button>();
                buttonComponent.image.color = Color.white;
                buttonComponent.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
                buttonComponent.GetComponentInChildren<TextMeshProUGUI>().text = "Select";
                b.GetComponent<Outline>().enabled = false;

            }
        }
        //set gray background for clicked button
        Button currentButtonComponent = button.GetComponent<Button>();
        currentButtonComponent.image.color = new Color(0.1f, 0.1f, 0.1f, 1);
        currentButtonComponent.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        currentButtonComponent.GetComponentInChildren<TextMeshProUGUI>().text = "Selected";
        button.GetComponent<Outline>().enabled = true;
    }
}
