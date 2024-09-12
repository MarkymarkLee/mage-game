using UnityEngine;
using TMPro;

public class MathOpDisplay : MonoBehaviour
{
    public TextMeshProUGUI operationText;    // Reference to the TextMeshPro component
    public int minNumber = 1;           // Minimum value for the number
    public int maxNumber = 10;          // Maximum value for the number

    private string[] operations = { "+", "-", "×", "÷" };  // Possible operations
    private string currentOperation;
    private int currentNumber;
    
    
    void Start()
    {
        RandomizeOperation();  // Set the initial operation and number
    }

    private void RandomizeOperation()
    {
        // Randomly select an operation
        currentOperation = operations[Random.Range(0, operations.Length)];

        // Randomly select a number within the range
        currentNumber = Random.Range(minNumber, maxNumber + 1);

        // Update the TextMeshPro text to display the operation and number
        operationText.text = currentOperation + " " + currentNumber.ToString();
    }

    // You can call this function when the ball interacts with the area to apply the effect
    public float ApplyOperation(float ballValue)
    {
        switch (currentOperation)
        {
            case "+":
                return ballValue + currentNumber;
            case "-":
                return ballValue - currentNumber;
            case "×":
                return ballValue * currentNumber;
            case "÷":
                return ballValue / currentNumber;
            default:
                return ballValue;
        }
    }
}
