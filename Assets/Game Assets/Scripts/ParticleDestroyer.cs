using UnityEngine;
using System.Collections;

public class ParticleDestroyer : MonoBehaviour {

	void Start () {
		#if UNITY_WEBPLAYER
		Destroy(gameObject);
		#endif
	}
}
