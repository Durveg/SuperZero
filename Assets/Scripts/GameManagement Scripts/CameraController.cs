using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	[SerializeField]
	private float speed; 
	private Vector3 cameraTarget;
	private Transform followTarget;
	private float startY;

	// Use this for initialization
	void Start () {

		followTarget = GameObject.FindGameObjectWithTag("Player").transform;
		this.startY = this.transform.position.y;
	}

	// Update is called once per frame
	void Update () {

		this.cameraTarget = followTarget.transform.position;
		this.cameraTarget.z = this.transform.position.z;
		Vector3 newPos = Vector3.Lerp(this.transform.position, cameraTarget, Time.deltaTime * speed);

		newPos.x = Mathf.Clamp(newPos.x, -87, 87);
		newPos.y = startY;

		this.transform.position = newPos;
	}
}