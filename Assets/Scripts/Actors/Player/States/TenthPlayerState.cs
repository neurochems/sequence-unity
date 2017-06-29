using UnityEngine;
using System.Collections;

public class TenthPlayerState : IParticleState 
{

	private readonly PlayerStatePattern psp;											// reference to pattern/monobehaviour class

	public bool isLight = true;															// 'is light' flag
	public bool circle, triangle, square;												// shape flags flag
	public float evol, deltaDark, deltaLight;											// evol tracking refs
	private bool checkEvol;																// check evol flag

	private bool canCollide = false;													// can collide flag (init false to begin stunned)
	private float collisionTimer;														// reset collision timer

	public TenthPlayerState (PlayerStatePattern playerStatePattern)					// constructor
	{
		psp = playerStatePattern;														// attach state pattern to this state 
	}

	public void UpdateState()															// updated each frame in PlayerStatePattern
	{
		// check evol
		if (checkEvol) {
			Evol();																		// check evol logic
			Debug.Log("check player evol");
			checkEvol = false;															// reset check evol flag
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		circle = psp.circle;																	// set current circle flag
		triangle = psp.triangle;																// set current triangle flag
		square = psp.square;																	// set current square flag
	}

	public void ToZero(bool toLight)
	{
		
	}

	public void ToFirst(bool toLight)
	{

	}

	public void ToSecond(bool toLight)
	{
		
	}

	public void ToThird(bool toLight)
	{
		
	}

	public void ToFourth(bool toLight)
	{
		
	}

	public void ToFifth(bool toLight, int shape)
	{
		
	}

	public void ToSixth(bool toLight, int shape)
	{
		
	}

	public void ToSeventh(bool toLight, int shape)
	{
		
	}

	public void ToEighth(bool toLight, int shape)
	{
		
	}

	public void ToNinth(bool toLight, int shape)
	{
		
	}

	public void ToTenth(bool toLight, int shape)
	{
		
	}

	public void Evol()
	{
		evol = psp.evol;																					// local evol check			
		isLight = psp.isLight;																					// update light value
		deltaDark = psp.deltaDark;																			// local dark check
		deltaLight = psp.deltaLight;																		// local light check
	}
}
