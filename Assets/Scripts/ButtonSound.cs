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

    // Når musen hover over knappen
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button != null && button.interactable)
        {
            MenuAudioManager.Instance?.PlayButtonHover();
        }
    }

    // Når knappen klikkes
    public void OnPointerClick(PointerEventData eventData)
    {
        if (button != null && button.interactable)
        {
            MenuAudioManager.Instance?.PlayButtonClick();
        }
    }
}