using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrumentoScript : MonoBehaviour {

	SpriteRenderer spriteRenderer;
	Animator animator;

	float scale = 0f;
	float maxScale = 90f;
	float scaleSpeed = 90f;
	bool spawning = false;

	// Use this for initialization
	void Start () {

		spriteRenderer = GetComponent<SpriteRenderer> ();
		spriteRenderer.sprite = Resources.Load ("Sprites/Strumenti/strum_0", typeof(Sprite)) as Sprite;
		transform.localScale = new Vector3 (scale, scale, 1);

		animator = GetComponent<Animator> ();

		Spawn ();

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

	public void ChangeStrum(int strum) {
		Debug.Log (strum);
		spriteRenderer.sprite = Resources.Load ("Sprites/Strumenti/strum_" + strum, typeof(Sprite)) as Sprite;
	}

	public void Spawn() {
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

			yield return new WaitForSeconds (time);

			animator.Play (returnToThisAnimation);
		}
	}
}
