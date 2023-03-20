using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NavigationTiles;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

    [CreateAssetMenu(menuName = "Nav Tiles/Nav Tile")]
	public class NavTile : Tile, INavTile
    {
        public int WalkCost => walkCost;
        [SerializeField] private int walkCost = 0;
        public bool Walkable => walkable;
        [SerializeField] private bool walkable = true;
        
    
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
        
