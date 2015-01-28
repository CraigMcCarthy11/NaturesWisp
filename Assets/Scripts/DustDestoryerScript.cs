using UnityEngine;
using System.Collections;

public class DustDestoryerScript : MonoBehaviour {

    private ParticleEmitter emitter;

	// Use this for initialization
	IEnumerator Start () {
        emitter = this.GetComponent<ParticleEmitter>();
        yield return new WaitForSeconds(5);
        emitter.emit = false;
        Destroy(this.gameObject, 3f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
