using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ActivarZonaPuzle : MonoBehaviour {

	GestorDeActividades gActividades;
	AudioSource audioZonaPuzle;
	AudioSource audioJugador;

	void Start () {

		gActividades = GameObject.FindGameObjectWithTag ("Player").GetComponent<GestorDeActividades> ();

		audioZonaPuzle = GetComponent<AudioSource>();
		audioJugador = GameObject.FindGameObjectWithTag ("Player").GetComponent<AudioSource>();

	}

	void OnTriggerEnter(Collider Otro)
	{
		if (Otro.tag == "Player") 
		{
			audioJugador.Stop ();
			audioZonaPuzle.Play();
		}

	}

	void OnTriggerStay(Collider Otro)
	{

		for(int i=0; i < gActividades.actividades.Length; i++)
		{

			if (gameObject.GetInstanceID () == gActividades.actividades [i].zona.GetInstanceID ()) 
			{
				gActividades.actividades [i].revisarZonaPuzle = true;
			}

		}
	}

	void OnTriggerExit(Collider Otro)
	{
		for(int i=0; i < gActividades.actividades.Length; i++)
		{

			if (gameObject.GetInstanceID () == gActividades.actividades [i].zona.GetInstanceID ()) 
			{
				gActividades.actividades [i].revisarZonaPuzle  = false;
			}

		}

		if (Otro.tag == "Player") 
		{
			audioZonaPuzle.Stop();
			audioJugador.Play ();
		}

	}


}
