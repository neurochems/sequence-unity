using UnityEngine;
using System.Collections;

public interface IParticleState 
{
	void UpdateState();

	void OnTriggerEnter(Collider other);

	void Death();

	void ToPhoton();

	void ToElectron();

	void ToElectron2();

	void ToShell();

	void ToShell2();

	void ToAtom();

//	void ToAtom2();

	void Evol ();

}
