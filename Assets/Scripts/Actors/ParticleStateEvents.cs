using UnityEngine;
using System.Collections;

public class ParticleStateEvents : MonoBehaviour {

	public delegate void ParticleEventHandler (int prevState, GameObject particle);

	public static event ParticleEventHandler toDead;
	public static event ParticleEventHandler toZero;
	public static event ParticleEventHandler toFirst;
	public static event ParticleEventHandler toSecond;
	public static event ParticleEventHandler toThird;
	public static event ParticleEventHandler toFourth;
	public static event ParticleEventHandler toFifth;
	public static event ParticleEventHandler toSixth;
	public static event ParticleEventHandler toSeventh;

	public static void TransitionToDead (int prevState, GameObject particle)
	{
		if (toDead != null)
			toDead (prevState, particle);
	}

	public static void TransitionToPhoton (int prevState, GameObject particle)
	{
		if (toZero != null)
			toZero (prevState, particle);
	}

	public static void TransitionToElectron (int prevState, GameObject particle)
	{
		if (toFirst != null)
			toFirst (prevState, particle);
	}

	public static void TransitionToElectron2 (int prevState, GameObject particle)
	{
		if (toSecond != null)
			toSecond (prevState, particle);
	}

	public static void TransitionToShell (int prevState, GameObject particle)
	{
		if (toThird != null)
			toThird (prevState, particle);
	}

	public static void TransitionToShell2 (int prevState, GameObject particle)
	{
		if (toFourth != null)
			toFourth (prevState, particle);
	}

	public static void TransitionToAtom (int prevState, GameObject particle)
	{
		if (toFifth != null)
			toFifth (prevState, particle);
	}
	public static void TransitionToAtom2 (int prevState, GameObject particle)
	{
		if (toSixth != null)
			toSixth (prevState, particle);
	}
	public static void TransitionToElement (int prevState, GameObject particle)
	{
		if (toSeventh != null)
			toSeventh (prevState, particle);
	}

}
