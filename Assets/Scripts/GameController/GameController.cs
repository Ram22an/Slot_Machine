using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private ReelScroll[] reels;
    [SerializeField] private TextMeshProUGUI[] prizeText;
    private bool resultsChecked = true;
    void Start()
    {
        PlayButton.HandlePulled += StartSpin;
    }
    void Update()
    {

        if (resultsChecked)
        {
            return;
        }


        foreach (ReelScroll reel in reels)
        {
            if (!reel.RowStopped)
            {

                return;
            }
        }

        resultsChecked = true;
        CheckResults();
    }

    private void StartSpin()
    {

        resultsChecked = false;

        foreach (TextMeshProUGUI Text in prizeText) {
            if (Text != null)
            {
                Text.text = "Spinning...";
            }
        }
    }

    private void CheckResults()
    {
        string result1 = reels[0].StoppedSlot;
        string result2 = reels[1].StoppedSlot;
        string result3 = reels[2].StoppedSlot;
        Debug.Log(result1+" "+result2+" "+result3);

        if (!string.IsNullOrEmpty(result1) && result1 == result2 && result2 == result3)
        {
            Debug.Log("WINNER! You got three " + result1 + "s!");
            foreach (TextMeshProUGUI Text in prizeText)
            {
                if (Text != null)
                {
                    Text.text = "WINNER! 3 x " + result1;
                }
            }
        }
        else
        {
            Debug.Log("Sorry, try again!");
            foreach (TextMeshProUGUI Text in prizeText)
            {
                if (Text != null)
                {
                    Text.text = "Try Again!";
                }
            }
        }
    }

    private void OnDestroy()
    {

        PlayButton.HandlePulled -= StartSpin;
    }
}
