using UnityEngine;
using System.Collections;

public class ISTouchHandler : MonoBehaviour {

	public Vector2 dragValue;
	public float scaleValue;
	public float rotateValue;

	static private ISTouchHandler self;

    static public Vector2 currentDragValue{get{ return self.dragValue;}}
	static public float currentScaleValue{get{ return self.scaleValue;}}
	static public float currentRotateValue{get{ return self.rotateValue;}}

	void Awake(){
		self = this;
	}

	void Update(){
		HandlerDrag();
		HandlerScale();
		HandlerRotate();
	}

	void HandlerDrag(){
		if(Input.touchCount != 1){
			dragValue = Vector2.zero;
			return;
		}
		if(Input.touches[0].phase != TouchPhase.Moved){
			dragValue = Vector2.zero;
			return;
		}

		if(Input.touchCount > 0){
			dragValue = -Input.touches[0].deltaPosition/(Mathf.Max(Screen.dpi,1));
		}
	}

	float lastDist = 0;
	void HandlerScale(){
		if(Input.touchCount != 2){
			scaleValue = 0f;
			lastDist = 0;
			return;
		}

		if(Input.touches[0].phase != TouchPhase.Moved && Input.touches[1].phase != TouchPhase.Moved){
			scaleValue = 0;
			//lastDist = 0;
			return;
		}

		float curDist = (Input.touches[0].position-Input.touches[1].position).magnitude;
		if(lastDist == 0) lastDist = curDist;
		scaleValue = (curDist - lastDist)* -0.01f;
		lastDist = curDist;
	}

	float lastAngle;
	void HandlerRotate(){
		if(Input.touchCount != 2){
			rotateValue = 0;
			lastAngle = 0;
			return;
		}
		Vector2 v2 = Input.touches[1].position-Input.touches[0].position;
		float curAngle = Mathf.Atan2(v2.y,v2.x)*Mathf.Rad2Deg;
		if(lastAngle == 0) lastAngle = curAngle;

		rotateValue = curAngle-lastAngle;
		lastAngle = curAngle;
	}
	
}
