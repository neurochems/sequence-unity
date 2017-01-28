using UnityEngine;
using System.Collections;

public class ZeroPlayerState : IParticleState
{

	private readonly PlayerStatePattern particle;										// reference to pattern/monobehaviour class

	private bool canCollide = true;														// can collide flag
	private float collisionTimer;														// reset collision timer

	// constructor
	public ZeroPlayerState (PlayerStatePattern playerStatePattern)					// constructor
	{
		particle = playerStatePattern;													// attach state pattern to this state 
	}

	public void UpdateState()															// called every frame in PlayerStatePattern
	{
		Evol ();																		// check evol

		// allow collisions timer
		if (!canCollide) collisionTimer += Time.deltaTime;								// start timer
		if (collisionTimer >= particle.stunDuration) {									// if timer is up
			canCollide = true;																// set collision ability			
			particle.stunned = false;														// reset stunned flag
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		ParticleStatePattern otherParticle 
			= other.gameObject.GetComponent<ParticleStatePattern>();							// ref other ParticleStatePattern

		if (canCollide) {																		// if collision allowed

/*			if (other == zero)
				AddDark(otherDark)
				AddLight(otherLight)
			else
				SubDark(otherDark)
				SubLight(otherLight)	*/

			if (other.gameObject.CompareTag ("Zero")) {												// collide with zero
				particle.stunned = true;																// stun for duration

				//particle.AddEvol (0.5f);																// add 0.5 evol
				//otherParticle.SubtractEvol (0.5f);														// subtract 0.5 evol from other

				if (otherParticle.darkEvol > 0)	{														// if other is dark
					particle.AddDark(0.5f);																	// add 0.5 dark
					otherParticle.SubtractDark(0.5f);														// subtract 0.5 dark from other
				}
				else if (otherParticle.lightEvol > 0) {													// if other is light
					particle.AddLight(0.5f);																// add 0.5 light
					otherParticle.SubtractLight(0.5f);														// subtract 0.5 light from other
				}
				
				canCollide = false;																		// reset has collided trigger
			} 
			else if (other.gameObject.CompareTag ("First")) {										// collide with first
				particle.stunned = true;																// stun for duration

				//particle.SubtractEvol (1.0f);															// subtract 1 evol
				//otherParticle.AddEvol (0.5f);															// add 1 evol from other

				if (particle.darkEvol > 0) {															// if dark
					particle.SubtractDark(0.5f);															// subtract 0.5 dark
					otherParticle.AddDark(0.5f);															// add 0.5 dark to other
				}
				else if (particle.lightEvol > 0) {														// if light
					particle.SubtractLight(0.5f);															// subtract 0.5 light
					otherParticle.AddLight(0.5f);															// add 0.5 light to other
				}

				canCollide = false;																		// reset has collided trigger
			} 
			else if (other.gameObject.CompareTag ("Second")) {										// collide with second
				particle.stunned = true;																// stun for duration

				particle.SubtractEvol (1.0f);															// subtract 1 evol
				otherParticle.AddEvol (0.5f);															// add 1 evol from other

				if (particle.darkEvol > 0) {															// if dark
					particle.SubtractDark(0.5f);															// subtract 0.5 dark
					otherParticle.AddDark(0.5f);															// add 0.5 dark to other
				}
				else if (particle.lightEvol > 0) {														// if light
					particle.SubtractLight(0.5f);															// subtract 0.5 light
					otherParticle.AddLight(0.5f);															// add 0.5 light to other
				}

				canCollide = false;																		// reset has collided trigger
			} 
			else if (other.gameObject.CompareTag ("Third")) {										// collide with third
				particle.stunned = true;																// stun for duration

				particle.SubtractEvol (1.0f);															// subtract 1 evol
				otherParticle.AddEvol (0.5f);															// add 1 evol from other

				if (particle.darkEvol > 0) {															// if dark
					particle.SubtractDark(0.5f);															// subtract 0.5 dark
					otherParticle.AddDark(0.5f);															// add 0.5 dark to other
				}
				else if (particle.lightEvol > 0) {														// if light
					particle.SubtractLight(0.5f);															// subtract 0.5 light
					otherParticle.AddLight(0.5f);															// add 0.5 light to other
				}
				
				canCollide = false;																		// reset has collided trigger
			} 
			else if (other.gameObject.CompareTag ("Fourth")) {										// collide with fourth
				particle.stunned = true;																// stun for duration

				particle.SubtractEvol (1.5f);															// subtract 1.5 evol
				otherParticle.AddEvol (0.5f);															// add 1 evol from other

				if (particle.darkEvol > 0) {															// if dark
					particle.SubtractDark(0.5f);															// subtract 0.5 dark
					otherParticle.AddDark(0.5f);															// add 0.5 dark to other
				}
				else if (particle.lightEvol > 0) {														// if light
					particle.SubtractLight(0.5f);															// subtract 0.5 light
					otherParticle.AddLight(0.5f);															// add 0.5 light to other
				}

				canCollide = false;																		// reset has collided trigger
			} 
			else if (other.gameObject.CompareTag ("Fifth")) {										// collide with fifth
				particle.stunned = true;																// stun for duration

				particle.SubtractEvol (2.5f);															// subtract 2.5 evol
				otherParticle.AddEvol (0.5f);															// add 1 evol from other

				if (particle.darkEvol > 0) {															// if dark
					particle.SubtractDark(0.5f);															// subtract 0.5 dark
					otherParticle.AddDark(0.5f);															// add 0.5 dark to other
				}
				else if (particle.lightEvol > 0) {														// if light
					particle.SubtractLight(0.5f);															// subtract 0.5 light
					otherParticle.AddLight(0.5f);															// add 0.5 light to other
				}

				canCollide = false;																		// reset has collided trigger
			}
			else if (other.gameObject.CompareTag ("Sixth")) {										// collide with sixth
				particle.stunned = true;																// stun for duration

				particle.SubtractEvol (4.0f);															// subtract 4 evol
				otherParticle.AddEvol (0.5f);															// add 1 evol from other

				if (particle.darkEvol > 0) {															// if dark
					particle.SubtractDark(0.5f);															// subtract 0.5 dark
					otherParticle.AddDark(0.5f);															// add 0.5 dark to other
				}
				else if (particle.lightEvol > 0) {														// if light
					particle.SubtractLight(0.5f);															// subtract 0.5 light
					otherParticle.AddLight(0.5f);															// add 0.5 light to other
				}

				canCollide = false;																		// reset has collided trigger
			}
			else if (other.gameObject.CompareTag ("Seventh")) {										// collide with seventh
				particle.stunned = true;																// stun for duration

				particle.SubtractEvol (6.5f);															// subtract 6.5 evol
				otherParticle.AddEvol (0.5f);															// add 1 evol from other

				if (particle.darkEvol > 0) {															// if dark
					particle.SubtractDark(0.5f);															// subtract 0.5 dark
					otherParticle.AddDark(0.5f);															// add 0.5 dark to other
				}
				else if (particle.lightEvol > 0) {														// if light
					particle.SubtractLight(0.5f);															// subtract 0.5 light
					otherParticle.AddLight(0.5f);															// add 0.5 light to other
				}

				canCollide = false;																		// reset has collided trigger
			}
		}
	}

	public void Death()
	{
		particle.TransitionToDead(0, particle.self);										// trigger transition effects
		ParticleStateEvents.toDead += particle.TransitionToDead;						// flag transition in delegate
		particle.currentState = particle.deadState;										// set to new state
	}

	public void ToZero()
	{
		Debug.Log ("Can't transition to same state");
	}

	public void ToFirst()
	{
		particle.TransitionToFirst(0, particle.self);									// trigger transition effects
		ParticleStateEvents.toFirst += particle.TransitionToFirst;						// flag transition in delegate
		particle.currentState = particle.firstState;									// set to new state
	}

	public void ToSecond()
	{
		particle.TransitionToSecond(0, particle.self);									// trigger transition effects
		ParticleStateEvents.toSecond += particle.TransitionToSecond;					// flag transition in delegate
		particle.currentState = particle.secondState;									// set to new state
	}

	public void ToThird()
	{
		particle.TransitionToThird(0, particle.self);									// trigger transition effects
		ParticleStateEvents.toThird += particle.TransitionToThird;						// flag transition in delegate
		particle.currentState = particle.thirdState;									// set to new state
	}

	public void ToFourth()
	{
		particle.TransitionToFourth(0, particle.self);									// trigger transition effects
		ParticleStateEvents.toFourth += particle.TransitionToFourth;					// flag transition in delegate
		particle.currentState = particle.fourthState;									// set to new state
	}

	public void ToFifth()
	{
		particle.TransitionToFifth(0, particle.self);									// trigger transition effects
		ParticleStateEvents.toFifth += particle.TransitionToFifth;						// flag transition in delegate
		particle.currentState = particle.fifthState;										// set to new state
	}

	public void ToSixth()
	{
		particle.TransitionToSixth(0, particle.self);									// trigger transition effects
		ParticleStateEvents.toSixth += particle.TransitionToSixth;						// flag transition in delegate
		particle.currentState = particle.sixthState;									// set to new state
	}

	public void ToSeventh()
	{
		particle.TransitionToSeventh(0, particle.self);									// trigger transition effects
		ParticleStateEvents.toSeventh += particle.TransitionToSeventh;					// flag transition in delegate
		particle.currentState = particle.seventhState;									// set to new stateebug.Log ("Can't transition to same state");
	}

	public void Evol()									// all states here for init 
	{
		if (particle.evol < 0f)															// if evol < 0
			Death ();																		// change to dead state
		else if (particle.evol == 1f)													// if evol == 1
			ToFirst ();																		// evolve to first
		else if (particle.evol == 1.5f)													// if evol == 1.5
			ToSecond ();																	// evolve to second
		else if (particle.evol == 2f)													// if evol == 2
			ToThird ();																		// evolve to third
		else if (particle.evol == 3f)													// if evol == 3
			ToFourth ();																	// evolve to fourth
		else if (particle.evol == 5f)													// if evol == 5
			ToFifth ();																		// evolve to fifth
		else if (particle.evol == 8f)													// if evol == 8
			ToSixth ();																		// evolve to sixth
		else if (particle.evol == 13f)													// if evol == 13
			ToSeventh ();																	// evolve to seventh
	}
}
