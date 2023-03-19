using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NavigationTiles.GridShapes.Editor
{
	[CustomPropertyDrawer(typeof(ShapeFromSet))]
	public class ShapeFromSetPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			
			var w = 80;//width of enum
			var p = 5;//padding
			var operationRect = new Rect(position.x, position.y, w, position.height);
			var shapeRect = new Rect(position.x + w+p, position.y, position.width -w, position.height);

			//GUIContent.none = no labels
			EditorGUI.PropertyField(operationRect, property.FindPropertyRelative("Operation"), GUIContent.none);
			EditorGUI.PropertyField(shapeRect, property.FindPropertyRelative("_shape"), GUIContent.none);
			
			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight(property, label)+2;
		}
	}
}