using UnityEngine;

public class ReelScroll : MonoBehaviour
{
    public float speed = 500f;        // pixels per second
    public float symbolHeight = 200f; // height of one symbol (set in Inspector)

    RectTransform rt;

    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Move the reel downward
        rt.anchoredPosition += Vector2.down * speed * Time.deltaTime;

        // If we've scrolled down past one symbol height, recycle
        if (rt.anchoredPosition.y <= -symbolHeight)
        {
            // Snap back up by one symbol height
            rt.anchoredPosition += new Vector2(0, symbolHeight);

            // Take the topmost child and move it to the bottom
            Transform topChild = rt.GetChild(0);
            topChild.SetAsLastSibling();
        }
    }
}
