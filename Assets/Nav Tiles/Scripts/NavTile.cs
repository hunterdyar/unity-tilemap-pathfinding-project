using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NavigationTiles;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

    [CreateAssetMenu]
	public class NavTile : Tile
    {
        public int WalkCost => walkCost;
        [SerializeField] private int walkCost = 0;
        public bool Walkable = true;
        public Vector3Int Location;
        public TilemapNavigation TilemapNavigation;
        //dictionaries are not serialized. so it needs to be initiated. Does this happen?
        private readonly Dictionary<Vector3Int, NavTile> neighbors = new Dictionary<Vector3Int, NavTile>();
    
        public override void RefreshTile(Vector3Int location, ITilemap tilemap)
        {
            base.RefreshTile(location,tilemap);
        }
       
        public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(location,tilemap,ref tileData);
        }
        // This determines if the Tile at the position is the same RoadTile.
        private static bool TryGetNavTile(ITilemap tilemap, Vector3Int position, out NavTile navTile)
        {
            navTile = null;
            if (tilemap.GetTile(position) is NavTile tile)
            {
                navTile = tile;
            }

            return false;
        }


    
    #if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a RoadTile Asset
        [MenuItem("Assets/Create/NavTile")]
        public static void CreateNavTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Nav Tile", "New Nav Tile", "Asset", "Save Nav Tile", "Assets");
            if (path == "")
                return;
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<NavTile>(), path);
        }
    #endif
    }
        
