using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public float movForce;

	private Rigidbody2D body;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector2 moveVector = new Vector2 (Input.GetAxisRaw ("Horizontal") , Input.GetAxisRaw ("Vertical"));
		body.AddForce (moveVector.normalized * movForce);
	}
}
