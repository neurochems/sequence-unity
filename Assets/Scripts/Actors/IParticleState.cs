﻿using UnityEngine;
using System.Collections;

public interface IParticleState 
{
	void UpdateState();

	void OnTriggerEnter(Collider other);

	void Death(bool toLight);

	void ToZero(bool toLight);

	void ToFirst(bool toLight);

	void ToSecond(bool toLight);

	void ToThird(bool toLight);

	void ToFourth(bool toLight);

	void ToFifth(bool toLight, int shape);

	void ToSixth(bool toLight, int shape);

	void ToSeventh(bool toLight, int shape);

	void Evol ();

}
