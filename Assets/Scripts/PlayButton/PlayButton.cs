using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private GameObject Handle;
    private Quaternion originalRotation, targetRotation;
    [SerializeField] private float rotateAngle = -90f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private ReelScroll[] reels;
    int[] Speed = {500, 600, 700};
    private bool isRotating = false;
    private void Awake()
    {
        originalRotation = Handle.transform.localRotation;
        targetRotation = Quaternion.Euler(rotateAngle, 0, 0);
        gameObject.GetComponent<Button>().onClick.AddListener(rotate);
    }
    void rotate()
    {
        if (!isRotating) StartCoroutine(RotateHandle());
        for (int i = 0; i < reels.Length; i++) {
            reels[i].StartScroll(Speed[i]);
        }
        StartCoroutine(StopReelsSequentially(3f));
    }
    private IEnumerator RotateHandle()
    {
        isRotating = true;
        yield return RotateTo(targetRotation);
        yield return RotateTo(originalRotation);
        isRotating = false;
    }
    private IEnumerator RotateTo(Quaternion target)
    {
        while (Quaternion.Angle(Handle.transform.localRotation, target) > 0.1f)
        {
            Handle.transform.localRotation =
                Quaternion.Lerp(Handle.transform.localRotation, target, Time.deltaTime * rotateSpeed);
            yield return null;
        }
        Handle.transform.localRotation = target;
    }
    public void CheckWin()
    {
        List<string> results = new List<string>();
        foreach (var reel in reels)
        {
            results.Add(reel.CurrentSymbol);
        }
        bool allSame = true;
        for (int i = 1; i < results.Count; i++)
        {
            if (results[i] != results[0])
            {
                allSame = false;
                break;
            }
        }

        if (allSame)
        {
            Debug.Log("WIN! Symbol: " + results[0]);
        }
        else
        {
            Debug.Log("No win this time.");
        }
    }
    private IEnumerator StopReelsSequentially(float delay)
    {
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < reels.Length; i++)
        {
            reels[i].StopScroll();
            yield return new WaitForSeconds(delay);
        }
        CheckWin();
    }
}

