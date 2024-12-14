using UnityEngine;
using UnityEngine.UI;

public class VariantEntity : MonoBehaviour
{
    private BuildingVariant Variant;

    private void Start()
    {
        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(TryToAcquire);
    }

    public BuildingVariant GetVariant()
    {
        return Variant;
    }

    public void SetVariant(BuildingVariant variant)
    {
        Variant = variant;
    }

    private void TryToAcquire()
    {
        CampController.instance.TryToAcquireBuilding(GetVariant());
    }
}
