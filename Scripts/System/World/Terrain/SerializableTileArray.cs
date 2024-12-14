[System.Serializable]
public class SerializableTileArray
{
	public Tile[] Tiles;
	public int Width;
	public int Height;

	public SerializableTileArray( Tile[,] mapTiles )
	{
		Width = mapTiles.GetLength( 0 );
		Height = mapTiles.GetLength( 1 );
		Tiles = new Tile[Width * Height];
		for ( int x = 0; x < Width; x++ ) {
			for ( int y = 0; y < Height; y++ ) {
				Tiles[y * Width + x] = mapTiles[x, y];
			}
		}
	}

	public Tile[,] ToMultidimensionalArray()
	{
		Tile[,] result = new Tile[Width, Height];
		for ( int x = 0; x < Width; x++ ) {
			for ( int y = 0; y < Height; y++ ) {
				result[x, y] = Tiles[y * Width + x];
			}
		}
		return result;
	}
}
