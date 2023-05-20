using UnityEngine;
using System.Collections;

public class PersonajeEnPlataforma : MonoBehaviour {

	public int tipo; // 1. PersonajeDentro de la plataforma - 2. PersonajeFuera de la plataforma

	void OnTriggerEnter(Collider otro)
	{
		if(tipo == 1)
		{
			otro.transform.parent = gameObject.transform;	
		}
		if(tipo == 2)
		{
			otro.transform.parent = null;
		}

	}

	void OnTriggerStay(Collider otro)
	{
		if(tipo == 2)
		{
			otro.transform.parent = null;
		}
	}

	void OnTriggerExit(Collider otro)
	{
		otro.transform.parent = null;
	}


}
