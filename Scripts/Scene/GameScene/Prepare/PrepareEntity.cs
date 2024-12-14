using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PreparationType
{
    PERK,
    RESEARCH
}

public class PrepareEntity : MonoBehaviour
{
    private string Name;
    private PreparationType PreparationType;

    private void Start()
    {
        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(TryToAcquire);
    }

    public string GetName()
    {
        return Name;
    }

    public void SetName(string name)
    {
        Name = name;
    }

    public PreparationType GetPreparationType()
    {
        return PreparationType;
    }

    public void SetPreparationType(PreparationType type)
    {
        PreparationType = type;
    }

    private void TryToAcquire()
    {
        PrepareController.instance.TryToAcquirePreparation(GetName(), GetPreparationType());
    }
}
