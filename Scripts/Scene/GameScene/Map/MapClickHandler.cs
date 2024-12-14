using UnityEngine;
using UnityEngine.UI;

public class MapClickHandler : MonoBehaviour
{
    private void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnTileClick);
    }

    public void OnTileClick()
    {
        TileEntity tileEntity = GetComponent<TileEntity>();
        Tile tile = tileEntity.GetTile();

        MapHUDController.instance.SetSelectedTile(tile);
    }
}
