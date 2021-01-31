using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigitalDisplay : MonoBehaviour
{
    [SerializeField]
    private Sprite[] digits;
    [SerializeField]
    private Image[] characters;
    private string codeSequence;
    public Game GameManager;

    [SerializeField]
    private Sprite[] symbolArray;
    private string password;
    private int dividerPosition;
    private string buttonName, buttonValue;

    // Start is called before the first frame update
    void Start()
    {
        codeSequence = "";

        SetPassword();

        KeypadButtonPush.ButtonPressed += AddDigitToCodeSequence;
    }

    private void AddDigitToCodeSequence(string digitEntered) {
        if (codeSequence.Length < 4) {
            switch(digitEntered){
                case "Zero":
                    codeSequence += "0";
                    DisplayCodeSequence(0);
                    break;
                case "One":
                    codeSequence += "1";
                    DisplayCodeSequence(1);
                    break;
                case "Two":
                    codeSequence += "2";
                    DisplayCodeSequence(2);
                    break;
                case "Three":
                    codeSequence += "3";
                    DisplayCodeSequence(3);
                    break;
                case "Four":
                    codeSequence += "4";
                    DisplayCodeSequence(4);
                    break;
                case "Five":
                    codeSequence += "5";
                    DisplayCodeSequence(5);
                    break;
                case "Six":
                    codeSequence += "6";
                    DisplayCodeSequence(6);
                    break;
                case "Seven":
                    codeSequence += "7";
                    DisplayCodeSequence(7);
                    break;
                case "Eight":
                    codeSequence += "8";
                    DisplayCodeSequence(8);
                    break;
                case "Nine":
                    codeSequence += "9";
                    DisplayCodeSequence(9);
                    break;
            }
        }
    }

    private void DisplayCodeSequence(int digitEntered) {
        switch (codeSequence.Length) {
            case 1:
                characters[3].sprite = digits[digitEntered];
                break;
            case 2:
                characters[2].sprite = characters[3].sprite;
                characters[3].sprite = digits[digitEntered];
                break;
            case 3:
                characters[1].sprite = characters[2].sprite;
                characters[2].sprite = characters[3].sprite;
                characters[3].sprite = digits[digitEntered];
                break;
            case 4:
                characters[0].sprite = characters[1].sprite;
                characters[1].sprite = characters[2].sprite;
                characters[2].sprite = characters[3].sprite;
                characters[3].sprite = digits[digitEntered];
                CheckResults();
                break;
        }
    }

    private void CheckResults() {
        if (codeSequence == password) {
            GameManager.GenerateMaze();
            ResetDisplay();
        }
        else {
            ResetDisplay();
        }
    }

    private void SetPassword() {
        for (int i=0; i<4; i++) {
            int rand = Random.Range(0, 9);
            buttonName = symbolArray[rand].name;
            dividerPosition = buttonName.IndexOf('_');
            buttonValue = buttonName.Substring(0, dividerPosition);

            characters[i].sprite = symbolArray[rand];
            password += rand;
        }
        Debug.Log(password);
    }

    private void ResetDisplay() {
        codeSequence = "";
        password = "";
        SetPassword();
    }

    private void OnDestroy() {
        KeypadButtonPush.ButtonPressed -= AddDigitToCodeSequence;
    }
}
