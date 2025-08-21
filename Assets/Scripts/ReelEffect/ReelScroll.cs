using System.Collections;
using UnityEngine;

public class ReelScroll : MonoBehaviour
{
    [SerializeField] private float symbolHeight = 379f;

    [SerializeField] private string[] symbolNames = new string[4];

    private int randomValue;
    private float timeInterval;
    private RectTransform rt;

    public bool RowStopped { get; private set; }
    public string StoppedSlot { get; private set; }


    void Start()
    {
        rt = GetComponent<RectTransform>();
        RowStopped = true;
        PlayButton.HandlePulled += StartRotating;
    }

    private void StartRotating()
    {
        StoppedSlot = "";
        StartCoroutine(Rotate());
    }
    private IEnumerator Rotate()
    {
        RowStopped = false;
        timeInterval = 0.025f;

        int stepsPerSymbol = 10;
        float stepSize = symbolHeight / stepsPerSymbol;
        // This MUST be based on the number of UNIQUE symbols (4 in your case)
        float totalReelHeight = symbolHeight * 4;

        // 1. Initial fast spin
        for (int i = 0; i < stepsPerSymbol * 2; i++)
        {
            rt.anchoredPosition -= new Vector2(0, stepSize);
            if (rt.anchoredPosition.y <= -totalReelHeight)
            {
                rt.anchoredPosition += new Vector2(0, totalReelHeight);
            }
            yield return new WaitForSeconds(timeInterval);
        }

        // 2. Main spin
        int symbolSpins = Random.Range(4, 9);
        randomValue = symbolSpins * stepsPerSymbol;

        for (int i = 0; i < randomValue; i++)
        {
            rt.anchoredPosition -= new Vector2(0, stepSize);
            if (rt.anchoredPosition.y <= -totalReelHeight)
            {
                rt.anchoredPosition += new Vector2(0, totalReelHeight);
            }

            if (i > Mathf.Round(randomValue * 0.5f)) timeInterval = 0.05f;
            if (i > Mathf.Round(randomValue * 0.75f)) timeInterval = 0.1f;
            if (i > Mathf.Round(randomValue * 0.95f)) timeInterval = 0.2f;

            yield return new WaitForSeconds(timeInterval);
        }

        // --- THIS IS THE MOST IMPORTANT FIX ---
        // 3. Snap to the final position
        float currentY = rt.anchoredPosition.y;
        int symbolIndex = Mathf.RoundToInt(-currentY / symbolHeight);

        // FIX: Removed the incorrect offset " - (symbolHeight / 2f) "
        float targetY = -symbolIndex * symbolHeight;

        // 4. Smoothly tween to the target position
        float tweenDuration = 0.2f;
        float elapsedTime = 0f;
        Vector2 startPos = rt.anchoredPosition;

        while (elapsedTime < tweenDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / tweenDuration;
            rt.anchoredPosition = Vector2.Lerp(startPos, new Vector2(0, targetY), progress);
            yield return null;
        }

        rt.anchoredPosition = new Vector2(0, targetY);

        // 5. Set the final stopped symbol name
        // This ensures the index is always positive and within the bounds of your 4 unique symbols
        symbolIndex = (symbolIndex % 4 + 4) % 4;
        StoppedSlot = symbolNames[symbolIndex];
        RowStopped = true;
    }
    private void OnDestroy()
    {
        PlayButton.HandlePulled -= StartRotating;
    }
}
