using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuboScript : MonoBehaviour {

	SpriteRenderer spriteRenderer;
	Sprite sprite;
	Animator animator;

	float scale = 0f;
	float maxScale = 200f;
	float scaleSpeed = 200f;
	bool spawning = false;

	// Use this for initialization
	void Start () {

		spriteRenderer = GetComponent<SpriteRenderer> ();
		sprite = gameObject.GetComponent<SpriteRenderer> ().sprite;
		transform.localScale = new Vector3 (scale, scale, 1);

		animator = GetComponent<Animator> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (spawning && scale < maxScale) {
			scale += scaleSpeed * Time.deltaTime;
			transform.localScale = new Vector3 (scale, scale, 1);
			Color tmp = spriteRenderer.color;
			tmp.a = scale / maxScale;
			spriteRenderer.color = tmp;
		}
		
	}

	public void Spawn() {
		scale = 0;
		spawning = true;
	}

	public void Remove() {
		spawning = false;
		scale = 0;
		transform.localScale = new Vector3 (scale, scale, 1);
	}

	IEnumerator PlayAnimation(string newAnimationName, string returnToThisAnimation, float time) {
		if (animator != null) {
			animator.Play (newAnimationName);
			Debug.Log (newAnimationName);

			yield return new WaitForSeconds (time);

			animator.Play (returnToThisAnimation);
			Debug.Log (returnToThisAnimation);
		}
	}
}
