using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(ISTouchHandler))]
public class ISTouchHandlerEditor : Editor {

	private ISTouchHandler mTH;

	void Awake () {
		mTH = target as ISTouchHandler;
	}

	public override void OnInspectorGUI () {
		EditorGUILayout.LabelField("Current Drag Value :");
		EditorGUILayout.LabelField ("\tX : " + mTH.dragValue.x + "  Y : " + mTH.dragValue.y);
		EditorGUILayout.LabelField ("Current Scale Value : " + mTH.scaleValue);
		EditorGUILayout.LabelField("Current Rotote Value : "+mTH.rotateValue);

		if (GUI.changed) {
			EditorUtility.SetDirty(mTH);
		}
	}
}
