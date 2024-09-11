using UnityEngine;
using TMPro;

public class MathOpDisplay : MonoBehaviour
{
    public string mathOperation = "+";   // The operation (+, -, ×, ÷)
    public int number = 1;               // The number for the operation
    public TextMeshProUGUI operationText;    // Reference to the TextMeshPro component

    void Start()
    {
        // Set the initial text to display the operation and number
        UpdateOperationDisplay();
    }

    // Function to update the displayed text
    public void UpdateOperationDisplay()
    {
        operationText.text = mathOperation + " " + number.ToString();
    }

    // Function to dynamically change the operation and number
    public void SetOperation(string newOperation, int newNumber)
    {
        mathOperation = newOperation;
        number = newNumber;
        UpdateOperationDisplay();
    }

    // You can call this function when the ball interacts with the area to apply the effect
    public int ApplyOperation(int ballValue)
    {
        switch (mathOperation)
        {
            case "+":
                return ballValue + number;
            case "-":
                return ballValue - number;
            case "×":
                return ballValue * number;
            case "÷":
                return ballValue / number;
            default:
                return ballValue;
        }
    }
}
