using System.Collections;
using UnityEngine;

public class ReelScroll : MonoBehaviour
{
    [SerializeField] private float symbolHeight = 377.5f;

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
        float totalReelHeight = symbolHeight * 4;

        // 1. Initial fast spin
        for (int i = 0; i < stepsPerSymbol * 2; i++)
        {
            rt.anchoredPosition -= new Vector2(0, stepSize);
            if (rt.anchoredPosition.y <= -totalReelHeight)
            {
                rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, rt.anchoredPosition.y + totalReelHeight);
            }
            yield return new WaitForSeconds(timeInterval);
        }

        int symbolSpins = Random.Range(4, 9);
        randomValue = symbolSpins * stepsPerSymbol;

        for (int i = 0; i < randomValue; i++)
        {
            rt.anchoredPosition -= new Vector2(0, stepSize);
            if (rt.anchoredPosition.y <= -totalReelHeight)
            {
                rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, rt.anchoredPosition.y + totalReelHeight);
            }

            if (i > Mathf.Round(randomValue * 0.5f)) timeInterval = 0.05f;
            if (i > Mathf.Round(randomValue * 0.75f)) timeInterval = 0.1f;
            if (i > Mathf.Round(randomValue * 0.95f)) timeInterval = 0.2f;

            yield return new WaitForSeconds(timeInterval);
        }

        float currentY = rt.anchoredPosition.y;
        int symbolIndex = Mathf.RoundToInt(-currentY / symbolHeight);
        float targetY = -symbolIndex * symbolHeight - (symbolHeight / 2f);

        float tweenDuration = 0.2f;
        float elapsedTime = 0f;

        while (elapsedTime < tweenDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / tweenDuration;

            float newY = Mathf.SmoothStep(currentY, targetY, progress);
            rt.anchoredPosition = new Vector2(0, newY);

            yield return null;
        }

        rt.anchoredPosition = new Vector2(0, targetY);

        symbolIndex = (symbolIndex % 4 + 4) % 4;
        StoppedSlot = symbolNames[symbolIndex];
        RowStopped = true;
    }

    private void OnDestroy()
    {
        PlayButton.HandlePulled -= StartRotating;
    }
}
