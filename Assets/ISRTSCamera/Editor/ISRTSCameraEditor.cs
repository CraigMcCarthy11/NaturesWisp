using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(ISRTSCamera))]
public class ISRTSCameraEditor : Editor {
    
	private ISRTSCamera mCam;
	private bool baseSetting;
	private bool boundSetting;
	private bool followSetting;
	private bool mouseSetting;
	private bool keyboardSetting;
	private bool touchSetting;

	void Awake () {
		mCam = target as ISRTSCamera;
	}

	public override void OnInspectorGUI () {

		baseSetting = EditorGUILayout.Foldout(baseSetting,"Base Setting");
		if (baseSetting) {
			mCam.moveSpeed = EditorGUILayout.FloatField("\tMove Speed",mCam.moveSpeed);
			mCam.rotSpeed = EditorGUILayout.FloatField("\tRot Speed",mCam.rotSpeed);
			//mCam.groundLayer = EditorGUILayout.LayerField("\tGround Layer",mCam.groundLayer);
			mCam.scrollXAngle = EditorGUILayout.CurveField(new GUIContent("\tScroll X Angle","Scroll X Angle Animation"),mCam.scrollXAngle);
			mCam.scrollHigh = EditorGUILayout.CurveField(new GUIContent("\tScroll High","Scroll High Animation"),mCam.scrollHigh);
			mCam.distOffset = EditorGUILayout.FloatField("\tScreen Center Distance Offset",mCam.distOffset);
			mCam.distMin = EditorGUILayout.FloatField("\tMin Distance",mCam.distMin);
			mCam.scrollValue = EditorGUILayout.Slider("\tStart Scroll Value",mCam.scrollValue,0f,1f);
			mCam.groundHighTest = EditorGUILayout.Toggle("\tGround High Test",mCam.groundHighTest);
			if(mCam.groundHighTest){
				mCam.groundLayer = EditorGUILayout.LayerField("\tGround Layer",mCam.groundLayer);
			}
		}

		boundSetting = EditorGUILayout.Foldout(boundSetting,"Bound");
		if(boundSetting){
			mCam.bound.xMin = EditorGUILayout.FloatField("\tMin X",mCam.bound.xMin);
			mCam.bound.xMax = EditorGUILayout.FloatField("\tMax X",mCam.bound.xMax);
			mCam.bound.yMin = EditorGUILayout.FloatField("\tMin Y",mCam.bound.yMin);
			mCam.bound.yMax = EditorGUILayout.FloatField("\tMax Y",mCam.bound.yMax);
			EditorGUILayout.HelpBox("The white line in scene view will help you configure scene bound.",MessageType.Info);
		}

		followSetting = EditorGUILayout.Foldout(followSetting,"Follow and Fixed Point");
		if(followSetting){
			mCam.allowFollow = EditorGUILayout.Toggle("\tAllow Follow",mCam.allowFollow);
			if(mCam.allowFollow){
				mCam.unlockWhenMove = EditorGUILayout.Toggle("\tUnlock When Move",mCam.unlockWhenMove);
			}
			else{
				EditorGUILayout.HelpBox("Enable Follow to let your camera focus something on center of screen or go to a fixed point.",MessageType.Info);
			}
		}

		mouseSetting = EditorGUILayout.Foldout(mouseSetting,"Mouse Control Setting");
		if (mouseSetting) {
			mCam.mouseControl = EditorGUILayout.Toggle("\tEnabled",mCam.mouseControl);
			if(mCam.mouseControl){
				if(EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android || EditorUserBuildSettings.activeBuildTarget == BuildTarget.iPhone || EditorUserBuildSettings.activeBuildTarget == BuildTarget.MetroPlayer){
					EditorGUILayout.HelpBox("You currently are build for a mobile device,you must disable mouse control when you running on your mobile device.",MessageType.Warning);
				}
				mCam.moveOffset = EditorGUILayout.FloatField("\tMove Offset",mCam.moveOffset);
				mCam.rotOffset = EditorGUILayout.FloatField("\tRotate Offset",mCam.rotOffset);
				mCam.scrollOffset = EditorGUILayout.FloatField("\tScroll Wheel Offset",mCam.scrollOffset);
			}
			else{
				EditorGUILayout.HelpBox("Enable Mouse Control to control camera with mouse.",MessageType.Info);
			}
		}

		keyboardSetting = EditorGUILayout.Foldout(keyboardSetting,"Keyboard Control Setting");
		if (keyboardSetting) {
			mCam.keyBoardControl = EditorGUILayout.Toggle("\tEnabled",mCam.keyBoardControl);
			if(mCam.keyBoardControl){
				mCam.moveOffset = EditorGUILayout.FloatField("\tMove Offset",mCam.moveOffset);
				mCam.rotOffset = EditorGUILayout.FloatField("\tRotate Offset",mCam.rotOffset);
			}
			else{
				EditorGUILayout.HelpBox("Enabel Keyboard Control to control camera with keyboard.",MessageType.Info);
			}
		}

		touchSetting = EditorGUILayout.Foldout (touchSetting, "Touch Control Setting");
		if (touchSetting) {
			mCam.touchControl = EditorGUILayout.Toggle("\tEnabled",mCam.touchControl);
			if(mCam.touchControl){
				mCam.touchMoveOffset = EditorGUILayout.FloatField("\tMove Offset",Mathf.Max(mCam.touchMoveOffset,0.000001f));
				mCam.touchRotateOffset = EditorGUILayout.FloatField("\tRotate Offset",Mathf.Max(mCam.touchRotateOffset,0.000001f));
				mCam.touchScrollOffset = EditorGUILayout.FloatField("\tScroll Offset",Mathf.Max(mCam.touchScrollOffset,0.000001f));
			}
			else{
				EditorGUILayout.HelpBox("Enable Touch control to control camera with touch screen.",MessageType.Info);
			}
		}

		if (GUI.changed || true) {
			EditorPrefs.SetBool("ISRCEBS",baseSetting);
			EditorPrefs.SetBool("ISRCEBOS",boundSetting);
			EditorPrefs.SetBool("ISRCEFS",followSetting);
			EditorPrefs.SetBool("ISRCEMS",mouseSetting);
			EditorPrefs.SetBool("ISRCEKS",keyboardSetting);
			EditorPrefs.SetBool("ISRCETS",touchSetting);
			EditorUtility.SetDirty(mCam);
		}
	}
}
