using UnityEngine;

public class ReelScroll : MonoBehaviour
{
    [SerializeField]private float speed = 500f;
    [SerializeField]private float symbolHeight = 200f;

    RectTransform rt;

    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        rt.anchoredPosition += Vector2.down * speed * Time.deltaTime;

        if (rt.anchoredPosition.y <= -symbolHeight)
        {
            rt.anchoredPosition += new Vector2(0, symbolHeight);
            Transform topChild = rt.GetChild(0);
            topChild.SetAsLastSibling();
        }
    }
}
