using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionMenu : MonoBehaviour
{
    public GameObject menuItemPrefab;
    
    private List<Interaction> interactions = new List<Interaction>();
    private List<GameObject> menuItems = new List<GameObject>();
    private bool interactionListChanged = true;
    private int selectionIndex = 0;
    private float menuItemOffset = .6375f;

    // Update is called once per frame
    void Update()
    {
        if (interactionListChanged)
        {
            interactions.Sort();
            RefreshMenu();
            interactionListChanged = false;
        }

        if (Input.GetButtonDown("Up"))
        {
            SelectUp();
        } else if (Input.GetButtonDown("Down"))
        {
            SelectDown();
        }

        if (Input.GetButtonDown("Interact"))
        {
            interactions[selectionIndex].DoInteraction();
            GetComponentInParent<Interactable>().CloseMenu();
        }
    }

    public void OnInteractionChange()
    {
        interactionListChanged = true;
    }

    public void AddInteraction(Interaction interaction)
    {
        interactions.Add(interaction);
        OnInteractionChange();
    }

    void OnEnable()
    {
        //TODO: Disable player movement
    }

    void OnDisable()
    {
        //TODO: Re-enable player movement
    }

    private void DeleteMenu()
    {
        foreach (GameObject menuItem in menuItems)
        {
            Destroy(menuItem);
        }
        menuItems.Clear();
    }

    private void RefreshMenu()
    {
        DeleteMenu();
        if (interactions.Count == 0)
        {
            return;
        }
        for (int i = 0; i < interactions.Count; i++)
        {
            GameObject newMenuItem = Instantiate(menuItemPrefab, transform.position + transform.up * menuItemOffset * (interactions.Count - i - 1), transform.rotation, transform);
            newMenuItem.GetComponentInChildren<TMPro.TextMeshPro>().text = interactions[i].GetMenuName();
            menuItems.Add(newMenuItem);
        }
        ResetSelection();
    }

    private void ResetSelection()
    {
        EnableCursor(0);
        selectionIndex = 0;
    }

    private void SelectDown()
    {
        DisableCursor(selectionIndex);
        selectionIndex++;
        if (selectionIndex >= interactions.Count)
        {
            selectionIndex = 0;
        }
        EnableCursor(selectionIndex);
    }

    private void SelectUp()
    {
        DisableCursor(selectionIndex);
        selectionIndex--;
        if (selectionIndex < 0)
        {
            selectionIndex = interactions.Count - 1;
        }
        EnableCursor(selectionIndex);
    }

    private void DisableCursor(int index)
    {
        if (index < menuItems.Count)
        {
            menuItems[index].transform.Find("Menu Cursor").gameObject.SetActive(false);
        }
    }

    private void EnableCursor(int index)
    {
        menuItems[index].transform.Find("Menu Cursor").gameObject.SetActive(true);
    }
}
