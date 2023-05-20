using UnityEngine;
using System.Collections;

public class AgarrarObjeto : MonoBehaviour {

	ControladorDelJugador cJugador;
	bool objAgarrado;
	int indiceObjAgarrado;
	int cantObjAgarrados;

	void Start () {
		
		cJugador = GameObject.FindGameObjectWithTag ("Player").GetComponent<ControladorDelJugador> ();
		objAgarrado = false;

	}

	void Update () {

		if (Input.GetKeyDown (KeyCode.E)) 
		{
			
			if(objAgarrado == true && cJugador.objetosClaves[indiceObjAgarrado].disponible == true)
			{
				
				objAgarrado = false;
				cJugador.objetosClaves [indiceObjAgarrado].agarrado = true;
				cJugador.objetosClaves [indiceObjAgarrado].objetoBase.SetActive (false);

				//Modificado para que no pueda empujar los objetos que estan en el piso
				cJugador.objetosClaves [indiceObjAgarrado].objetoReal.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
				cJugador.objetosClaves [indiceObjAgarrado].objetoReal.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotation;

			}
		}
	}


	void OnTriggerEnter(Collider Otro)
	{
		
		// Verifica que el objeto clave colisiona con el jugador 
		if (Otro.tag == "Player") 
		{	

			for (int i = 0; i < cJugador.objetosClaves.Length; i++) 
			{
				if (cJugador.objetosClaves [i].agarrado == true) 
				{
					cantObjAgarrados++;
				}
			}
			
			for (int i = 0; i < cJugador.objetosClaves.Length; i++) 
			{
				if (gameObject.GetInstanceID () == cJugador.objetosClaves [i].zonaObjetoReal.GetInstanceID ()) 
				{
					if (cantObjAgarrados == 0) 
					{
						objAgarrado = true;
						indiceObjAgarrado = i;
					}

				}
			}

		}
	}

	void OnTriggerExit(Collider Otro)
	{
		// Revisar solo cuando salga de el personaje por q puede ocurrir cuando salga de otro objeto
		if (Otro.tag == "Player") 
		{
			objAgarrado = false;
			cantObjAgarrados = 0;
		}
	}


}
