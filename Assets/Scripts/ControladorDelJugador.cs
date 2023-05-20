using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class ObjetosClaves
{
	// Son todos los objetos que el jugador puede agarrar y necesita para cumplir una actividad
	public int idObjeto;
	public GameObject objetoBase; 			// Objeto del piso
	public GameObject zonaObjetoReal;
	public GameObject objetoReal;
	public GameObject objetoAnimacion;		// Carga el jugador
	public GameObject objetoTransparente;
	public int tipo; 						// 1. Regadera - 2.Suelos - 3. Cajas - 4. Planta - 5. Tronco 
	public bool agarrado;
	public bool disponible; 				// siempre esta disponible pero cuando ya este en la zona no estara mas disponible
	public bool colocadoEnDestino; 			// Si el objeto se encuentra en la zona de destino

}

public class ControladorDelJugador : MonoBehaviour 
{
	public GameObject camara;				// Camara
	[Range(1.0f,20.0f)] 					
	public float velocidadJugador = 5f;
	public float suavidadRotacion = 10f;

	Animator animJugador;
	bool detenerJugador;


	// salto
	Rigidbody rb;

	// Agarrar Objetos
	public ObjetosClaves[] objetosClaves;
	int indiceAuxObjAgarrado;

	//Identificar cuando el personaje carga un objeto
	bool cargaObjeto;

	// Salto
	float saltoMax = 7.5f; // Fuerzas de salto
	float distanciaSuelo = 0.1f;
	bool estaEnElSuelo;
	private bool salto; 
	public float gravedadAumentada = 7.0f;
	bool congelarControl;	// congela control dependiendo del salto
	bool moverEnElAire; 
	Vector3 direccionSalto; //direccion salto
	float velocidadDeSalto;
	float velocidadX;
	float velocidadZ; 

	bool activarCorrutinaEnElAire;
	bool verificarSaltoConVelocidad;


	public bool DetenerJugador 
	{
		get { return detenerJugador; }
		set { detenerJugador = value; }
	}
		
	void Start () 
	{
		animJugador = GetComponent<Animator>();
		detenerJugador = false;

		// salto
		rb = GetComponent<Rigidbody>();


		// Agarrar Objetos
		for(int i=0; i<objetosClaves.Length; i++)
		{
			objetosClaves [i].objetoAnimacion.SetActive (false);
			objetosClaves [i].disponible = false;
			objetosClaves [i].agarrado = false;

			//Modificado para que no pueda empujar los objetos que estan en el piso
			objetosClaves [i].objetoReal.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;

			if(objetosClaves[i].tipo ==3)
			{
				objetosClaves [i].disponible = true;
			}
		}

		velocidadDeSalto = 0;

		cargaObjeto = false;

		activarCorrutinaEnElAire = false;
		verificarSaltoConVelocidad = false;
	}
		
	void Update ()
	{
		if (detenerJugador == false) 
		{
			
			if (!salto)
			{
				salto = Input.GetKeyUp(KeyCode.Space);

			}

		}

		if (Input.GetKeyDown (KeyCode.E)) 
		{
			if(objetosClaves[indiceAuxObjAgarrado].agarrado == true)
			{
				cargaObjeto = false;
				objetosClaves [indiceAuxObjAgarrado].agarrado = false;
				objetosClaves [indiceAuxObjAgarrado].objetoAnimacion.SetActive (false);
				objetosClaves [indiceAuxObjAgarrado].objetoReal.transform.position = objetosClaves [indiceAuxObjAgarrado].objetoAnimacion.transform.position;

				//Modificado para que cuando el jugador suelte el objetos conserve su rotacion
				objetosClaves[indiceAuxObjAgarrado].objetoReal.transform.rotation = objetosClaves [indiceAuxObjAgarrado].objetoAnimacion.transform.rotation;
				objetosClaves [indiceAuxObjAgarrado].objetoBase.SetActive (true);
			}
		}
	}

	void FixedUpdate()
	{
		VerificadorDeColisionConElSuelo();

		if(estaEnElSuelo == false)
		{
			if(activarCorrutinaEnElAire == false && verificarSaltoConVelocidad ==true)
			{
				activarCorrutinaEnElAire = true;

				//Tiempo en el aire
				StartCoroutine(CorrutinaEnElAire(1.0f));
			}
		}


		float moverHorizontal = Input.GetAxis("Horizontal");
		float moverVertical = Input.GetAxis("Vertical");

		if(detenerJugador == false)
		{
				MoverJugador(moverHorizontal, moverVertical,salto);        

		}

		salto = false;

		int numObjAgarrados = 0;

		// Agarrar Objeto identifica el agarrado y recalcula la zona para agarrar el objeto
		for(int i=0; i<objetosClaves.Length; i++)
		{
			if(objetosClaves[i].agarrado == true)
			{
				objetosClaves [i].objetoAnimacion.SetActive (true);
				indiceAuxObjAgarrado = i;

				numObjAgarrados++;

			}
				
			objetosClaves [i].zonaObjetoReal.transform.position = objetosClaves [i].objetoReal.transform.position;

			//Modificado para que cuando el jugador suelte el objeto conserve su rotacion
			objetosClaves [i].zonaObjetoReal.transform.rotation = objetosClaves [i].objetoReal.transform.rotation;


			//Modificado para que no pueda empujar los objetos que estan en el piso
			if(objetosClaves [i].zonaObjetoReal.transform.position.y < (transform.position.y + 0.2f) && objetosClaves[i].agarrado == false)
			{
				objetosClaves [i].objetoReal.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
			}

		}

		if(numObjAgarrados>0)
		{
			cargaObjeto = true;
		}
		else
		{
			cargaObjeto = false;
		}


		AnimarJugador(moverHorizontal, moverVertical,detenerJugador, cargaObjeto);

		if (!estaEnElSuelo) 
		{
			// acelera la caida
			rb.AddForce(new Vector3(0,-gravedadAumentada,0), ForceMode.Acceleration);
		}

	}

	void VerificadorDeColisionConElSuelo()
	{

		// posicion del personaje y el piso
		if (Physics.Raycast(transform.position, -Vector3.up, distanciaSuelo))
		{	
			estaEnElSuelo = true;	
			congelarControl = false;
			moverEnElAire = false;
		
			//print ("Esta en el piso :");
		}
		else
		{
			
			estaEnElSuelo = false;	
			//print ("Esta en el aire :");
		}
	}

	void MoverJugador(float h, float v,bool salto)
	{
		// Accion de Correr 
		if ( (h != 0 || v != 0) && !congelarControl) 
		{
			Vector3 direccion = new Vector3 (h, 0f, v);

			// nueva direccion respecto a la camara rotacion de la camara
			Vector3 nuevaDireccion = Quaternion.LookRotation(Camera.main.transform.position - transform.position).eulerAngles;
			nuevaDireccion.x = 0;
			nuevaDireccion.z = 0;
			camara.transform.rotation = Quaternion.Euler(nuevaDireccion);
			// direccion salto

			// Mueve de posicion al jugador
			transform.Translate(-direccion * Time.deltaTime * velocidadJugador, camara.transform);
			// rota al jugador
			Quaternion nuevaRotacion = Quaternion.LookRotation(-direccion);
			// interpola los resultador ente 0 - 1
			transform.rotation = Quaternion.Slerp(transform.rotation, nuevaRotacion * camara.transform.rotation, suavidadRotacion * Time.deltaTime);

			//calcular la velocidad q lleva para ejecutar el salto
			direccionSalto = direccion ;

		}



		// mientras el este en el aire 
		// congelo h y v
		// sino reviso si esta en piso y descongelo
		if(salto && estaEnElSuelo)
		{
			velocidadX = (float) Math.Round (direccionSalto.x, 1);
			velocidadZ = (float) Math.Round (direccionSalto.z, 1);


			if ( velocidadX != 0 ||  velocidadZ != 0) 
			{
				//print ("SALTO CON VELOCIDAD");

				if (Mathf.Abs (direccionSalto.x) >= Mathf.Abs (direccionSalto.z)) 
				{
					velocidadDeSalto = Mathf.Abs (direccionSalto.x);
				} else 
				{
					velocidadDeSalto = Mathf.Abs (direccionSalto.z);
				}

				// fuerza en vertical
				rb.AddForce (new Vector3 (0, saltoMax, 0), ForceMode.VelocityChange);
				// quitar posibilidad de mover hasta q caiga
				congelarControl = true;
				// Direccion y velocidad a donde se movia :: falta tomar en cuenta el angulo de la camara despues q esta se mueve
				// fuerza en horizontal
				// lo impulsa desde la parte posterior del jugador y su velocidad
				rb.AddForce(transform.forward*velocidadJugador*velocidadDeSalto, ForceMode.Impulse);
				// reiniciar direccionSalto dinamica de salto 
				direccionSalto = new Vector3(0,0,0);
				verificarSaltoConVelocidad = true;
			} 
			else 
			{
				//print ("SALTO SIN VELOCIDAD");
				congelarControl = true;
				moverEnElAire = true;
				rb.AddForce (new Vector3 (0, saltoMax, 0), ForceMode.VelocityChange);
			}

		}

		// moverse en el Aire cuando salta
		if ((h != 0 || v != 0) && moverEnElAire == true) 
		{
			Vector3 direccion2 = new Vector3 (h, 0f, v);
			// Mueve de posicion al jugador
			transform.Translate(direccion2 * Time.deltaTime * 2.0f, camara.transform);
		}


	}		

	void AnimarJugador(float h, float v,bool detenerJ, bool cargaObj)
	{
		bool corriendo;
		bool corriendoConObjeto;
		bool esperaConObjeto;
		bool estaEnEspera;

		if (detenerJ == false && cargaObj == false) 
		{
			corriendo = h != 0 || v != 0;
			estaEnEspera = h == 0 && v == 0;
		} 
		else
		{
			corriendo = false;
			estaEnEspera = false;
		}


		if(detenerJ == false && cargaObj == true)
		{
			corriendoConObjeto =  h != 0 || v != 0;
			esperaConObjeto = h == 0 && v == 0;

		}
		else
		{
			corriendoConObjeto = false;
			esperaConObjeto = false;
		}


		animJugador.SetBool ("EstaCorriendo", corriendo);
		animJugador.SetBool ("EstaCorriendoConObjeto", corriendoConObjeto);
		animJugador.SetBool ("EstaEnEsperaConObjeto", esperaConObjeto);
		animJugador.SetBool ("EstaEnEspera", estaEnEspera);
	}


	// Corrutina en caso de fallo en colision se ejecuta solo despues de un salto con velocidad
	IEnumerator CorrutinaEnElAire(float tiempoEspera) 
	{  

		//Comienza la Corrutina
		yield return new WaitForSeconds(tiempoEspera);
		//Finaliza la Corrutina

		if(estaEnElSuelo == false)
		{
			estaEnElSuelo = true;	
			congelarControl = false;
			moverEnElAire = false;
		}

		activarCorrutinaEnElAire = false;
		verificarSaltoConVelocidad = false;


	}

}
