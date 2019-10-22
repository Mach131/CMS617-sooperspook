using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Interactable : MonoBehaviour
{
    public GameObject buttonHint;
    public InteractionMenu interactionMenu;

    // Start is called before the first frame update
    void Start()
    {
        HideButtonHint();
        HideInteractionMenu();
        PopulateInteractionMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact") && IsButtonHintActive())
        {
            OnButtonHintPress();
        }

        if (Input.GetButtonDown("Cancel") && IsInteractionMenuActive())
        {
            CloseMenu();
        }
    }

    public void OnButtonHintPress()
    {
        HideButtonHint();
        ShowInteractionMenu();
    }

    public void CloseMenu()
    {
        ShowButtonHint();
        HideInteractionMenu();
    }

    public HashSet<string> GetState()
    {
        HashSet<string> stateSet = new HashSet<string>();
        foreach (Interaction interaction in GetComponents<Interaction>())
        {
            stateSet.Add(interaction.GetMenuName() + "." + interaction.GetState());
        }
        return stateSet;
    }

    private void ShowButtonHint()
    {
        buttonHint.gameObject.SetActive(true);
    }

    private void HideButtonHint()
    {
        buttonHint.gameObject.SetActive(false);
    }

    private void PopulateInteractionMenu()
    {
        foreach (Interaction interaction in GetComponents<Interaction>())
        {
            interactionMenu.AddInteraction(interaction);
        }
    }

    private void ShowInteractionMenu()
    {
        interactionMenu.gameObject.SetActive(true);
    }

    private void HideInteractionMenu()
    {
        interactionMenu.gameObject.SetActive(false);
    }

    private bool IsButtonHintActive()
    {
        return buttonHint.gameObject.activeInHierarchy;
    }

    private bool IsInteractionMenuActive()
    {
        return interactionMenu.gameObject.activeInHierarchy;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !IsInteractionMenuActive() && IsLooking(other.gameObject)) {
            ShowButtonHint();
        }
        if (other.CompareTag("Player") && IsButtonHintActive() && !IsLooking(other.gameObject))
        {
            HideButtonHint();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HideButtonHint();
            HideInteractionMenu();
        }
    }

    private bool IsLooking(GameObject other)
    {
        return Vector3.Dot(other.transform.forward.normalized, (transform.position - other.transform.position).normalized) > 0.75f; // Corresponds to 45 deg either direction of straight
    }
}
