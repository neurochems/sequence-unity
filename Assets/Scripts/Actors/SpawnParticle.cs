using UnityEngine;
using System.Collections;

public class SpawnParticle : MonoBehaviour {

	public GameObject newParticle;					// reference to prefab to be instantiated

	// public methods

	public void SpawnPhoton(int numSpawn) {
		// sfx.PlayOneShot (Death, 1.0f);
		int i = 0;
		do {
			if (i == 0){
				GameObject particle = Instantiate																			// create new photon
					(newParticle, new Vector3(transform.position.x + Random.Range(-3.5f, -1.5f), transform.position.y, transform.position.z + Random.Range(0.5f, 2.5f)), Quaternion.identity) as GameObject;		
				NewPhotonSetUp(particle);																					// set up particle 
				//particle.GetComponent<ParticleStatePattern> ().stunned = true;												// stun new particle for 3 sec
			}
			else if (i == 1){
				GameObject particle = Instantiate																			// create new electron
					(newParticle, new Vector3(transform.position.x + Random.Range(-3.5f, -1.5f), transform.position.y, transform.position.z + Random.Range(1.5f, -2.5f)), Quaternion.identity) as GameObject;		
				NewPhotonSetUp(particle);																					// set up particle 
				//particle.GetComponent<ParticleStatePattern> ().stunned = true;												// stun new particle for 3 sec
			}
			else {
				GameObject particle = Instantiate																			// create new particle
					(newParticle, new Vector3(transform.position.x + Random.Range(-3.5f, -1.5f), transform.position.y, transform.position.z + Random.Range(-3.5f, 2.5f)), Quaternion.identity) as GameObject;	
				NewPhotonSetUp(particle);																					// set up particle 
				//particle.GetComponent<ParticleStatePattern> ().stunned = true;												// stun new particle for 3 sec
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
					(newParticle, new Vector3(transform.position.x + Random.Range(-3.5f, -1.5f), transform.position.y, transform.position.z + Random.Range(-3.5f, -1.5f)), Quaternion.identity) as GameObject;		
				NewElectronSetUp(particle);																					// set up new electron
				//particle.GetComponent<ParticleStatePattern> ().stunned = true;												// stun new particle for 3 sec
			}
			else if (i == 1){
				GameObject particle = Instantiate																			// create new particle at same position randomly offset from centre
					(newParticle, new Vector3(transform.position.x + Random.Range(-3.5f, -1.5f), transform.position.y, transform.position.z + Random.Range(-3.5f, -1.5f)), Quaternion.identity) as GameObject;		
				NewElectronSetUp(particle);																					// set up new electron
				//particle.GetComponent<ParticleStatePattern> ().stunned = true;												// stun new particle for 3 sec
			}
			else if (i == 2){
				GameObject particle = Instantiate																			// create new particle at same position randomly offset from centre
					(newParticle, new Vector3(transform.position.x + Random.Range(-3.5f, -1.5f), transform.position.y, transform.position.z + Random.Range(-3.5f, -1.5f)), Quaternion.identity) as GameObject;		
				NewElectronSetUp(particle);																					// set up new electron
				//particle.GetComponent<ParticleStatePattern> ().stunned = true;												// stun new particle for 3 sec
			}
			else {
				GameObject particle = Instantiate																			// create new particle at same position randomly offset from centre
					(newParticle, new Vector3(transform.position.x + Random.Range(-3.5f, -1.5f), transform.position.y, transform.position.z + Random.Range(-3.5f, -1.5f)), Quaternion.identity) as GameObject;		
				NewElectronSetUp(particle);																					// set up new electron
				//particle.GetComponent<ParticleStatePattern> ().stunned = true;												// stun new particle for 3 sec
			}

			i+=1;																											// # of particle spawn count

			//Debug.Log("particle lose electron");																				// debug
		} while (i != numSpawn);
	}

	// private methods

	private void NewPhotonSetUp(GameObject newParticle) 
	{
		newParticle.transform.SetParent(GameObject.FindGameObjectWithTag("Particles").transform);																// sort new electron under 'Particles'
		newParticle.GetComponent<SphereCollider>().enabled = true;														// enable sphere collider
		newParticle.GetComponent<ParticlePhysicsManager> ().attractor 
			= GameObject.FindGameObjectWithTag("World").GetComponent<FauxGravityAttractor>();							// manually set new electron FauxGravityAttractor as World

		newParticle.GetComponent<Animator>().SetBool("black", false);													// reset fade trigger
		newParticle.GetComponent<Animator>().SetTrigger("fadein");														// fade to white
		newParticle.GetComponent<Animator>().SetBool("photon", true);													// photon: set photon condition
		newParticle.GetComponent<Animator>().SetTrigger("scaledown");													// scale to photon

	}

	private void NewElectronSetUp (GameObject newParticle)
	{
		newParticle.transform.SetParent(GameObject.FindGameObjectWithTag("Particles").transform);						// sort new electron under 'Particles'
		newParticle.GetComponent<ParticlePhysicsManager> ().attractor 
			= GameObject.FindGameObjectWithTag("World").GetComponent<FauxGravityAttractor>();							// manually set new electron FauxGravityAttractor as World

		newParticle.GetComponent<ParticleStatePattern>().evol = 1f;														// set new particle evol to 1
		
		newParticle.GetComponent<Animator>().SetBool("black", false);													// set fade trigger
		newParticle.GetComponent<Animator>().SetTrigger("fadein");														// fade to white
	}
}
