using UnityEngine;
using System.Collections;

public interface IParticleState 
{
	void UpdateState();

	void OnTriggerEnter(Collider other);

	void Death();

	void ToZero();

	void ToFirst();

	void ToSecond();

	void ToThird();

	void ToFourth();

	void ToFifth();

	void ToSixth();

	void ToSeventh();

	void Evol ();

}
