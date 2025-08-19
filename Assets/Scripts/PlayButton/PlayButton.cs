using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private GameObject Handle;
    private Quaternion originalRotation,targetRotation;
    [SerializeField] private float rotateAngle = -90f;
    [SerializeField] private float rotateSpeed = 5f;
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
}

