using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button != null && button.interactable)
        {
            GameAudioManager.Instance?.PlayButtonHover();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (button != null && button.interactable)
        {
            GameAudioManager.Instance?.PlayButtonClick();
        }
    }
}