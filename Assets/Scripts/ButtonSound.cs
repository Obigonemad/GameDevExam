using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    // N�r musen hover over knappen
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button != null && button.interactable)
        {
            MenuAudioManager.Instance?.PlayButtonHover();
        }
    }

    // N�r knappen klikkes
    public void OnPointerClick(PointerEventData eventData)
    {
        if (button != null && button.interactable)
        {
            MenuAudioManager.Instance?.PlayButtonClick();
        }
    }
}