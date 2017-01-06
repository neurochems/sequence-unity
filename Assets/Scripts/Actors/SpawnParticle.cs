using UnityEngine;
using System.Collections;

public class SpawnParticle : MonoBehaviour {

	public GameObject lostParticle;					// reference to prefab to be instantiated

	private float stunTimer = 0f;					// stun timer

	// public methods

	public void SpawnPhoton(int numSpawn) {
		// sfx.PlayOneShot (Death, 1.0f);
		int i = 0;
		do {
			if (i == 0){
				GameObject particle = Instantiate																			// create new photon
					(lostParticle, new Vector3(transform.position.x + Random.Range(-3.5f, -1.5f), transform.position.y, transform.position.z + Random.Range(0.5f, 2.5f)), Quaternion.identity) as GameObject;		
				NewPhotonSetUp(particle);																					// set up particle 
				Stun(particle);																								// stun new particle for 3 sec
			}
			else if (i == 1){
				GameObject particle = Instantiate																			// create new electron
					(lostParticle, new Vector3(transform.position.x + Random.Range(-3.5f, -1.5f), transform.position.y, transform.position.z + Random.Range(1.5f, -2.5f)), Quaternion.identity) as GameObject;		
				NewPhotonSetUp(particle);																					// set up particle 
				Stun(particle);																								// stun new particle for 3 sec
			}
			else {
				GameObject particle = Instantiate																			// create new particle
					(lostParticle, new Vector3(transform.position.x + Random.Range(-3.5f, -1.5f), transform.position.y, transform.position.z + Random.Range(-3.5f, 2.5f)), Quaternion.identity) as GameObject;	
				NewPhotonSetUp(particle);																					// set up particle 
				Stun(particle);																								// stun new particle for 3 sec
			}

			i+=1;																											// # of particle spawn count

			//Debug.Log("particle lose electron");																				// debug
		} while (i != numSpawn);
	}

	public void SpawnElectron(int numSpawn) {
		// sfx.PlayOneShot (Death, 1.0f);
		int i = 0;
		do {
			if (i == 0){
				GameObject particle = Instantiate																			// create new particle at same position randomly offset from centre
					(lostParticle, new Vector3(transform.position.x + Random.Range(-3.5f, -1.5f), transform.position.y, transform.position.z + Random.Range(-3.5f, -1.5f)), Quaternion.identity) as GameObject;		
				NewElectronSetUp(particle);																					// set up new electron
				Stun(particle);																								// stun new particle for 3 sec
			}
			else if (i == 1){
				GameObject particle = Instantiate																			// create new particle at same position randomly offset from centre
					(lostParticle, new Vector3(transform.position.x + Random.Range(-3.5f, -1.5f), transform.position.y, transform.position.z + Random.Range(-3.5f, -1.5f)), Quaternion.identity) as GameObject;		
				NewElectronSetUp(particle);																					// set up new electron
				Stun(particle);																								// stun new particle for 3 sec
			}
			else if (i == 2){
				GameObject particle = Instantiate																			// create new particle at same position randomly offset from centre
					(lostParticle, new Vector3(transform.position.x + Random.Range(-3.5f, -1.5f), transform.position.y, transform.position.z + Random.Range(-3.5f, -1.5f)), Quaternion.identity) as GameObject;		
				NewElectronSetUp(particle);																					// set up new electron
				Stun(particle);																								// stun new particle for 3 sec
			}
			else {
				GameObject particle = Instantiate																			// create new particle at same position randomly offset from centre
					(lostParticle, new Vector3(transform.position.x + Random.Range(-3.5f, -1.5f), transform.position.y, transform.position.z + Random.Range(-3.5f, -1.5f)), Quaternion.identity) as GameObject;		
				NewElectronSetUp(particle);																					// set up new electron
				Stun(particle);																								// stun new particle for 3 sec
			}

			i+=1;																											// # of particle spawn count

			//Debug.Log("particle lose electron");																				// debug
		} while (i != numSpawn);
	}

	// private methods

	private void NewPhotonSetUp(GameObject newParticle) 
	{
		newParticle.transform.parent = gameObject.transform.parent.transform;											// sort new electron under 'Collectables'
		//newParticle.GetComponent<FauxGravityBody> ().attractor = gameObject.GetComponent<FauxGravityBody>().attractor;	// manually set new electron FauxGravityAttractor as World

		newParticle.GetComponent<Animator>().SetBool("black", false);													// reset fade trigger
		newParticle.GetComponent<Animator>().SetTrigger("fadein");														// fade to white
		newParticle.GetComponent<Animator>().SetBool("photon", true);													// photon: set photon condition
		newParticle.GetComponent<Animator>().SetTrigger("scaledown");													// scale to photon

		//particle.GetComponent<ParticleManager>().initState = ParticleState.Photon;									// set particle to evol 0
	}

	private void NewElectronSetUp (GameObject newParticle)
	{
		newParticle.transform.parent = gameObject.transform.parent.transform;											// sort new particle under 'Collectables'
		//newParticle.GetComponent<FauxGravityBody> ().attractor = gameObject.GetComponent<FauxGravityBody>().attractor;	// manually set new particle FauxGravityAttractor as World
		newParticle.GetComponent<Animator>().SetBool("black", false);													// set fade trigger
		newParticle.GetComponent<Animator>().SetTrigger("fadein");														// fade to white

		newParticle.GetComponent<ParticleStatePattern>().evol = 1f;														// set new particle evol to 1
	}

	private void Stun(GameObject newParticle)
	{
		newParticle.GetComponent<ParticleStatePattern>().Stun(true);													// disable collider
		stunTimer += Time.deltaTime;																				// start timer
		if (stunTimer >= 3f) {																						// if timer >= duration
			newParticle.GetComponent<ParticleStatePattern>().Stun(true);													// enable collider
			stunTimer = 0f;																								// reset timer
		}}

}
