using UnityEngine;
using DG.Tweening;
using System;

public class PopupController : MonoBehaviour
{
    [SerializeField] private GameObject Popup;
    [SerializeField] private GameObject Overlay;
    private static PopupController CurrentPopup;
    public static Action CurrentPopupCloseCallback;
    private readonly static float AnimationDuration = 0.5f;
    private static Vector3 ClosedPopupRotation = new(90, 0, 0);

    private void Start()
    {
        if (Popup != null)
        {
            Popup.transform.localScale = Vector3.zero;
            Popup.transform.eulerAngles = ClosedPopupRotation;
            Popup.SetActive(false);
        }
        if (Overlay != null)
        {
            Overlay.GetComponent<CanvasGroup>().alpha = 0f;
            Overlay.SetActive(false);
        }
    }

    public void OpenPopup()
    {
        OpenPopup(null);
    }

    public void OpenPopup(Action closeCallback)
    {
        AnimateOpen(Popup.transform, Overlay.GetComponent<CanvasGroup>());
        AudioController.Instance.PlayPopup();
        CurrentPopup = this;        
        CurrentPopupCloseCallback = closeCallback;
    }


    public void ClosePopup()
    {
        AnimateClose(Popup.transform, Overlay.GetComponent<CanvasGroup>());
        AudioController.Instance.PlayPopup();
        CurrentPopup = null;
    }

    public static void CloseCurrentPopup()
    {
        if (CurrentPopup != null)
        {
            AnimateClose(CurrentPopup.Popup.transform, CurrentPopup.Overlay.GetComponent<CanvasGroup>(), true);
            CurrentPopup = null;
        }
    }
    private void AnimateOpen(Transform popup, CanvasGroup overlay)
    {
        popup.gameObject.SetActive(true);
        popup.DOScale(Vector3.one, AnimationDuration).SetEase(Ease.OutBack);
        popup.DORotate(Vector3.zero, AnimationDuration).SetEase(Ease.OutBack);

        overlay.gameObject.SetActive(true);
        overlay.DOFade(1f, AnimationDuration).SetEase(Ease.OutQuad);
    }

    private static void AnimateClose(Transform popup, CanvasGroup overlay, bool isCurrentPopup = false)
    {
        popup.DOScale(Vector3.zero, AnimationDuration).SetEase(Ease.InBack);
        popup.DORotate(ClosedPopupRotation, AnimationDuration).SetEase(Ease.InBack).OnComplete(()=>popup.gameObject.SetActive(false));
        overlay.DOFade(0f, AnimationDuration).SetEase(Ease.InQuad).OnComplete(() =>
        {
            overlay.gameObject.SetActive(false);
            if (isCurrentPopup)
            {
                CurrentPopupCloseCallback?.Invoke();
            }
        });
    }
}
