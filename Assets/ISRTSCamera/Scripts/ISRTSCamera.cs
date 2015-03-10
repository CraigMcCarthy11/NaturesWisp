using UnityEngine;
using System.Collections;

public class ISRTSCamera : MonoBehaviour {
	
	public AnimationCurve scrollXAngle;
	public AnimationCurve scrollHigh;
	public float groundHigh;
	public float distOffset;
	public float distMin;
	public bool unlockWhenMove;
	public float scrollOffset;
	public float moveOffset;
	public float rotOffset;
	public float scrollValue;
	public float moveSpeed;
	public float rotSpeed;
	public float minHigh;
	public bool groundHighTest;
	public int groundLayer;
	public Rect bound;
	public Transform followingTarget;
	public Transform fixedPoint;

	public float touchMoveOffset;
	public float touchScrollOffset;
	public float touchRotateOffset;

	private Transform selfT;
	
	private Vector3 wantPos;
	private float wantYAngle;
	private float wantXAngle;

	public bool keyBoardControl;
	public bool mouseControl;
	public bool touchControl;
	public bool allowFollow;

	static private ISRTSCamera self;

	private LayerMask groundMask;
	private Camera selfC;

	static public ISRTSCamera GetInstantiated(){ return self;}

	public void KeyboardControl(bool enable){
		keyBoardControl = enable;
		if (keyBoardControl) StartCoroutine (UpdateKeyboardControl ());
	}

	public void MouseControl(bool enable){
		mouseControl = enable;
		if (mouseControl) StartCoroutine (UpdateMouseControl ());
	}

	public void TouchControl(bool enable){
		touchControl = enable;
		if (touchControl) StartCoroutine (UpdateTouchControl ());
	}

	static public void LockFixedPoint(Transform pos){
		if(self.allowFollow){
			self.followingTarget = null;
			self.fixedPoint = pos;

			//Set the wantPos to make camera more smooth when leave fixed point.
			self.wantPos.x = pos.position.x;
			self.wantPos.z = pos.position.z;
		}
	}

	static public void UnlockFixedPoint(){
		self.fixedPoint = null;
	}

	static public void Follow(Transform target){
		if(self.allowFollow){
			self.fixedPoint = null;
			self.followingTarget = target;
		}
	}

	static public void CancelFollow(){
		self.followingTarget = null;
	}

	static public Transform GetFollowingTarget(){return self.followingTarget;}
	static public Transform GetFixedPoint(){return self.fixedPoint;}

	void Awake () {
		self = this;
		selfT = transform;
		selfC = GetComponent<Camera>();
		groundMask = 1<<groundLayer;
	}
	
	void Start(){
		wantPos = selfT.position;
		scrollValue = Mathf.Clamp01(scrollValue);

		Vector3 pos = selfT.position;
		pos.y = scrollHigh.Evaluate(scrollValue);
		wantPos = pos;

		Vector3 rot = selfT.eulerAngles;
		rot.x = WrapAngle(rot.x);
		rot.y = WrapAngle(rot.y);
		wantYAngle = rot.y;
		rot.x = scrollXAngle.Evaluate(scrollValue);
		wantXAngle = rot.x;
		selfT.eulerAngles = rot;

		StartCoroutine (UpdateTransform ());
		if(keyBoardControl) StartCoroutine (UpdateKeyboardControl ());
		if(mouseControl) StartCoroutine (UpdateMouseControl ());
		if (touchControl) StartCoroutine (UpdateTouchControl ());
	}

	Vector3 curMoveInertia = Vector3.zero;
	Vector3 curRotInertia = Vector3.zero;

	IEnumerator UpdateKeyboardControl(){
		while (true) {
			if(!keyBoardControl) break;

			if(Input.GetKey(KeyCode.W)) Move(selfT.forward,moveOffset);
			if(Input.GetKey(KeyCode.S)) Move(-selfT.forward,moveOffset);
			if(Input.GetKey(KeyCode.A)) Move(-selfT.right,moveOffset);
			if(Input.GetKey(KeyCode.D)) Move(selfT.right,moveOffset);
			if(Input.GetKey(KeyCode.Q)) Rotate(moveOffset);
			if(Input.GetKey(KeyCode.E)) Rotate(-moveOffset);

			yield return null;
		}
	}

	IEnumerator UpdateMouseControl(){
		while (true) {

			if(!mouseControl) break;
			if(Input.GetMouseButton(2)|| Input.GetMouseButton(1)){
				Rotate(Input.GetAxis("Mouse X")*rotOffset);
			}
			else{
				if(Input.mousePosition.y > Screen.height - 0.01f) Move(selfT.forward,moveOffset);
				if(Input.mousePosition.y < 0.1f) Move(-selfT.forward,moveOffset);
				if(Input.mousePosition.x < 0.1f) Move(-selfT.right,moveOffset);
				if(Input.mousePosition.x > Screen.width - 0.01f) Move(selfT.right,moveOffset);
				Scroll(Input.GetAxis("Mouse ScrollWheel")*scrollOffset);
			}

			yield return null;
		}
	}

	IEnumerator UpdateTouchControl(){
		while (true) {
			Vector2 dV = ISTouchHandler.currentDragValue;
			if(dV != Vector2.zero){
				Quaternion fixedDir = Quaternion.Euler(new Vector3(0,selfT.eulerAngles.y,selfT.eulerAngles.z));
				Vector3 forwardDir = fixedDir*Vector3.forward;
				Move(dV.y*forwardDir,touchMoveOffset);
				Move(dV.x*selfT.right,touchMoveOffset);
			}
			if(ISTouchHandler.currentScaleValue != 0) Scroll(ISTouchHandler.currentScaleValue*touchScrollOffset);
			if(ISTouchHandler.currentRotateValue != 0) Rotate(ISTouchHandler.currentRotateValue*touchRotateOffset);
			yield return null;
		}
	}

	public void Move(Vector3 dir,float offset){
		dir.y = 0;
		dir *= Time.deltaTime;
		dir.Normalize();
		dir*= offset;
		if(unlockWhenMove && dir != Vector3.zero){
			followingTarget = null;
			fixedPoint = null;
		}
		wantPos += dir;

		wantPos.x = Mathf.Clamp(wantPos.x,bound.xMin,bound.xMax);
		wantPos.z = Mathf.Clamp(wantPos.z,bound.yMin,bound.yMax);
	}

	public void Rotate(float dir){
		wantYAngle += dir;
		WrapAngle(wantYAngle);
	}

	public void Scroll(float value){
		scrollValue += value;
		scrollValue = Mathf.Clamp01(scrollValue);
		wantPos.y = scrollHigh.Evaluate(scrollValue);
		wantXAngle = scrollXAngle.Evaluate(scrollValue);
	}

	float WrapAngle (float a){
		while (a < -180.0f) a += 360.0f;
		while (a > 180.0f) a -= 360.0f;
		return a;
	}

	IEnumerator UpdateTransform(){
		//Vector3 mPos = Vector3.zero;
		while (true) {

			Vector3 offset;
			Vector3 finalPos;
			if(!fixedPoint){
				float currentGroundHigh = groundHigh;
				RaycastHit hit;
				if (groundHighTest && Physics.Raycast(selfT.position, -Vector3.up, out hit,Mathf.Infinity,groundMask)){
					currentGroundHigh = selfT.position.y - hit.distance;
				}
				float distToGround = selfT.position.y - currentGroundHigh;

				Quaternion targetRot = Quaternion.Euler(wantXAngle,wantYAngle,0f);
				selfT.rotation = Quaternion.Slerp(selfT.rotation,targetRot,rotSpeed*Time.deltaTime);
				Vector3 centerPos = ScreenPointToWorldPoint(new Vector2(Screen.width/2,Screen.height/2),followingTarget ? followingTarget.position.y : currentGroundHigh);
				float dist = Vector3.Distance(selfT.position,centerPos);

				dist = Mathf.Max(dist,distMin);
				offset = Vector3.forward*distOffset*dist;

				if(followingTarget){
					wantPos.x = followingTarget.position.x;
					wantPos.z = followingTarget.position.z;
				}

				finalPos = wantPos - (targetRot*offset);
				finalPos.y = wantPos.y+currentGroundHigh;
				selfT.position = Vector3.Lerp(selfT.position,finalPos,moveSpeed*Time.deltaTime);
			}
			else{
				selfT.rotation = Quaternion.Slerp(selfT.rotation,fixedPoint.rotation,rotSpeed*Time.deltaTime);
				selfT.position = Vector3.Lerp(selfT.position,fixedPoint.position,moveSpeed*Time.deltaTime);
				wantPos.x = selfT.position.x;
				wantPos.z = selfT.position.z;
			}
			yield return null;
		}
	}

	/// <summary>
	/// Return the position at sea high by the screen point given.
	/// </summary>
	/// <returns>The point to world point.</returns>
	/// <param name="screenPoint">Screen point.</param>
	public Vector3 ScreenPointToWorldPoint(Vector2 screenPoint,float groundHigh){
		Vector3 wp = selfC.ScreenToWorldPoint(new Vector3(screenPoint.x,screenPoint.y,selfT.position.y-groundHigh));
		return wp;
	}

	Vector3 lastReturnedPoint = Vector3.zero;
	/// <summary>
	/// Screens the point to sea point.
	/// This method use the ray to check.
	/// </summary>
	/// <returns>The point to sea point.</returns>
	/// <param name="screenPoint">Screen point.</param>
	public Vector3 ScreenPointToGroundPoint(Vector2 screenPoint){
		RaycastHit rhit;
		Ray ray = selfC.ScreenPointToRay (new Vector3 (screenPoint.x, screenPoint.y, 0));

		if (Physics.Raycast (ray, out rhit,Mathf.Infinity,groundMask)) {
			lastReturnedPoint = rhit.point;
		}

		return lastReturnedPoint;
	}

	void OnDrawGizmosSelected(){
		Vector3 mp = transform.position;
		Gizmos.DrawLine(new Vector3(bound.xMin,mp.y,bound.yMin),new Vector3(bound.xMin,mp.y,bound.yMax));
		Gizmos.DrawLine(new Vector3(bound.xMin,mp.y,bound.yMax),new Vector3(bound.xMax,mp.y,bound.yMax));
		Gizmos.DrawLine(new Vector3(bound.xMax,mp.y,bound.yMax),new Vector3(bound.xMax,mp.y,bound.yMin));
		Gizmos.DrawLine(new Vector3(bound.xMax,mp.y,bound.yMin),new Vector3(bound.xMin,mp.y,bound.yMin));
	}
}
