using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

	public GameObject target;

	void FixedUpdate () {
		transform.Translate ((Vector2)(target.transform.localPosition - transform.localPosition) * 0.1f);
	}
}
