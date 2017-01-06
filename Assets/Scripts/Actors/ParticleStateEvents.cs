using UnityEngine;
using System.Collections;

public class ParticleStateEvents : MonoBehaviour {

	public delegate void ParticleEventHandler (GameObject particle);

	public static event ParticleEventHandler toDead;
	public static event ParticleEventHandler toPhoton;
	public static event ParticleEventHandler toElectron;
	public static event ParticleEventHandler toElectron2;
	public static event ParticleEventHandler toShell;
	public static event ParticleEventHandler toShell2;
	public static event ParticleEventHandler toAtom;

	public static void TransitionToDead (GameObject particle)
	{
		if (toDead != null)
			toDead (particle);
	}

	public static void TransitionToPhoton (GameObject particle)
	{
		if (toPhoton != null)
			toPhoton (particle);
	}

	public static void TransitionToElectron (GameObject particle)
	{
		if (toElectron != null)
			toElectron (particle);
	}

	public static void TransitionToElectron2 (GameObject particle)
	{
		if (toElectron2 != null)
			toElectron2 (particle);
	}

	public static void TransitionToShell (GameObject particle)
	{
		if (toShell != null)
			toShell (particle);
	}

	public static void TransitionToShell2 (GameObject particle)
	{
		if (toShell2 != null)
			toShell2 (particle);
	}

	public static void TransitionToAtom (GameObject particle)
	{
		if (toAtom != null)
			toAtom (particle);
	}

}
