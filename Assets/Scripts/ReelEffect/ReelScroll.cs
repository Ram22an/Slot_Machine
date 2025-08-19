using UnityEngine;

public class ReelScroll : MonoBehaviour
{
    [SerializeField] private float symbolHeight = 200f;
    private RectTransform rt;
    private bool isScrolling = false;
    private float scrollSpeed = 0f;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0, rt.anchoredPosition.y);
    }

    void Update()
    {
        if (isScrolling)
        {
            rt.anchoredPosition += Vector2.down * scrollSpeed * Time.deltaTime;

            if (rt.anchoredPosition.y <= -symbolHeight)
            {
                rt.anchoredPosition += new Vector2(0, symbolHeight);
                Transform topChild = rt.GetChild(0);
                topChild.SetAsLastSibling();
            }
        }
    }

    public void StartScroll(float speed)
    {
        Debug.Log(gameObject.name + " is starting scroll at speed " + speed);
        scrollSpeed = speed;
        isScrolling = true;
    }

    public void StopScroll()
    {
        isScrolling = false;
        float y = rt.anchoredPosition.y;
        float snappedY = Mathf.Round(y / symbolHeight) * symbolHeight;
        rt.anchoredPosition = new Vector2(0, snappedY); ;

    }
    public string CurrentSymbol
    {
        get
        {
            return rt.GetChild(0).name;
        }
    }
}
