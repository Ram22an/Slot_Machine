using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class PlayButton : MonoBehaviour
{
    [SerializeField] private Image Handle;
    private float targetAngle = -90f;
    private float duration = 0.5f;
    public static event Action HandlePulled;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(RotateHandleFunction);
    }
    private void RotateHandleFunction()
    {
        StartCoroutine(RotateHandle());
    }
    private IEnumerator RotateHandle()
    {
        yield return StartCoroutine(RotateCoroutine(
            Handle.transform.rotation,
            Quaternion.Euler(targetAngle, 0, 0)
        ));

        yield return StartCoroutine(RotateCoroutine(
            Handle.transform.rotation,
            Quaternion.Euler(0, 0, 0)
        ));
        HandlePulled?.Invoke();
    }

    private IEnumerator RotateCoroutine(Quaternion startRotation, Quaternion endRotation)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            Handle.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }

        Handle.transform.rotation = endRotation;
    }
}