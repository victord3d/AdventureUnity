using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class DialogoDuende : MonoBehaviour {

	GestorDeDialogos gDialogos;
	ControladorDelJugador cJugador;
	GestorDeActividades gActividades;

	public GameObject panel;	// Panel independiente del duende
	public Text texto; 			// texto independiente del duende

	bool activarDialogo;		// Al activar el trigger se prepara para el dialogo
	public string[] auxDialogos;// Obtiene los dialogos de la lista de duendes
	int contadorDialogo;		// Dialogo actual por cada enter

	bool finalizarDialogo;		// Variable para que se ejecute 1 sola vez todo el dialogo

	int idAuxActividad;			// Obtiene el numero de la actividad que esta activando el duende
	int idAuxDuende;			// Obtiene el numero del Duende que esta activo
	bool resuelveActividadAux;  // Verifica si ese duande tiene una actividad asociada

	int idAuxDuendeSig;			// Obtiene el numero del Duende siguiente que se va a activar
	bool repetirAux;			// Verifica si es una actividad a repetir

	GameObject sueloDinamico; 	// Se hace falso en el Start de la clase RecolectorDeObjetosClaves

	//CargarYGuardarPartida
	CargarYGuardarPartida guardarPartida;

	//Icono Enter
	GameObject iconoDialogo;

	//Puente Montana
	GameObject puenteMontana;

	void Awake()
	{
		sueloDinamico = GameObject.Find ("SueloDinamico");
		iconoDialogo = GameObject.Find ("IconoDialogo");
		puenteMontana = GameObject.Find ("MontanaPuente01");
	}
		
	void Start () {

		gDialogos = GameObject.FindGameObjectWithTag ("Player").GetComponent<GestorDeDialogos> ();
		cJugador = GameObject.FindGameObjectWithTag ("Player").GetComponent<ControladorDelJugador> ();

		DesactivarPanel ();
		activarDialogo = false;
		finalizarDialogo = false;
		contadorDialogo = 0;

		gActividades = GameObject.FindGameObjectWithTag ("Player").GetComponent<GestorDeActividades> ();

		idAuxActividad = 0;
		idAuxDuende = 0;

		//CargarYGuardarPartida
		guardarPartida = GameObject.Find("GuardaryCargarScript").GetComponent<CargarYGuardarPartida> ();

		//Icono Enter
		iconoDialogo.SetActive(false);

		//PuenteMontana
		puenteMontana.SetActive(false);

	}
		
	void Update () {

		if(Input.GetKeyDown(KeyCode.Return))
		{
			if(idAuxDuende == 3)
			{
				puenteMontana.SetActive (true);
			}

			if( activarDialogo == true )
			{
				if(contadorDialogo == auxDialogos.Length)
				{
					// Al terminar de hablar con el Duende
					DesactivarPanel ();
					activarDialogo = false;

					print("ResuelveAcvtividad: "+resuelveActividadAux);

					// Si un duende tiene una actividad asociada activa el puzle si es la primera vez o si la va a repetir
					if(resuelveActividadAux == true){
						
						for(int k = 0; k < gActividades.actividades.Length; k++)
						{
							if(gActividades.actividades[k].idActividad == idAuxActividad)
							{
								// Activo zona puzle de tipo 1 y 2 que no reciben Objetos claves
								if(gActividades.actividades[k].tipo == 1 || gActividades.actividades[k].tipo == 2)
								{
									gActividades.actividades[k].activarZonaPuzle = true;

									if(repetirAux == true)
									{
										gActividades.actividades[k].repetir = true;
									}
								}
								// Activo el objeto clave despues de dialogo
								if (gActividades.actividades [k].tipo == 3)
								{
									for(int i=0; i<cJugador.objetosClaves.Length; i++)
									{
										// Objeto de tipo 1 regadera
										if(cJugador.objetosClaves[i].tipo == 1)
										{
											cJugador.objetosClaves [i].disponible = true;
											print ("Esta disponible? "+cJugador.objetosClaves[i].disponible);
											print ("Esta agarrado? "+cJugador.objetosClaves[i].agarrado);
										}
									}

									if(repetirAux == true)
									{
										gActividades.actividades[k].repetir = true;
										gActividades.actividades[k].activarZonaPuzle = true;
									}
								}

								if (gActividades.actividades [k].tipo == 4) 
								{
									for(int i=0; i<cJugador.objetosClaves.Length; i++)
									{
										// Objeto de tipo 2 muestras de terreno
										if(cJugador.objetosClaves[i].tipo == 2)
										{
											cJugador.objetosClaves [i].disponible = true;

										}
									}

									// Repetir Actividad
									if(repetirAux == true)
									{
										gActividades.actividades[k].repetir = true;
										gActividades.actividades[k].activarZonaPuzle = true;
										sueloDinamico.SetActive(true);
									}
								}

								if(gActividades.actividades [k].tipo == 5)
								{
									for(int i=0; i<cJugador.objetosClaves.Length; i++)
									{
										// Objeto de tipo 5 Troncos
										if(cJugador.objetosClaves[i].tipo == 5)
										{
											cJugador.objetosClaves [i].disponible = true;
										}
									}

									if(repetirAux == true)
									{
										gActividades.actividades[k].repetir = true;
										gActividades.actividades[k].activarZonaPuzle = true;
									}
								}

								gActividades.actividades [k].guardar = true;

							}
						}
					}

					// Si un duende no tiene una actividad asociada pasa a estar en estado repetir y activa el siguiente duende
					if(resuelveActividadAux == false)
					{
						for(int f=0; f < gDialogos.duendes.Length; f++)
						{
							if(gDialogos.duendes[f].tipo == "Aprendizaje" || gDialogos.duendes[f].tipo == "Habilidad" )
							{
								if(gDialogos.duendes[f].idDuende == idAuxDuende)
								{
									gDialogos.duendes[f].estado = "Repetir";
									gDialogos.duendes[f].colorGorroDuende = Color.blue;
									gDialogos.duendes[f].gorroDuende.GetComponent<Renderer>().material.color = gDialogos.duendes[f].colorGorroDuende;

								}

								if(gDialogos.duendes[f].idDuende == idAuxDuendeSig)
								{
									gDialogos.duendes[f].estado = "Activo";
									gDialogos.duendes[f].colorGorroDuende = Color.green;
									gDialogos.duendes[f].gorroDuende.GetComponent<Renderer>().material.color = gDialogos.duendes[f].colorGorroDuende;
								}
							}

						}
					}
						


				}

				if( contadorDialogo < auxDialogos.Length)
				{
					ActivarPanel ();
					texto.text = auxDialogos [contadorDialogo];
					contadorDialogo++;
				}


			}

			if(activarDialogo == false && finalizarDialogo == true)
			{
				contadorDialogo = 0;
				finalizarDialogo = false;

				// Guardado automatico
				guardarPartida.GuardarPartida();
			}
				
		}

	}

	void OnTriggerEnter(Collider Otro)
	{

		activarDialogo = true;
		finalizarDialogo = true;

		//Icono Enter
		iconoDialogo.transform.position = new Vector3(transform.parent.position.x,transform.parent.position.y + 0.7f, transform.parent.position.z);
		iconoDialogo.transform.rotation = transform.parent.rotation;
		iconoDialogo.SetActive(true);

		for(int i=0; i < gDialogos.duendes.Length; i++)
		{

			if(gameObject.GetInstanceID() == gDialogos.duendes[i].zonaDuende.GetInstanceID())
			{

				// Carga el dialogo del Duende de acuerdo a su Tipo (Aprendizaje, Habilidad, Acertijo)

				if(gDialogos.duendes[i].tipo == "Aprendizaje")
				{
					if(gDialogos.duendes[i].estado == "Activo"){

						auxDialogos = new string[gDialogos.duendes[i].dialogoVerde.Length];

						for(int j=0; j < gDialogos.duendes[i].dialogoVerde.Length; j++)
						{
							auxDialogos [j] = gDialogos.duendes [i].dialogoVerde[j];

						}

						//Guarda en una variable auxiliar el identificador de la actividad que se debe activar
						idAuxActividad = gDialogos.duendes[i].idActividad;
						idAuxDuende = gDialogos.duendes[i].idDuende;
						idAuxDuendeSig = gDialogos.duendes[i].idDuende + 1;
						resuelveActividadAux = gDialogos.duendes[i].resuelveActividad; // Guardo en una variable si ese Duende tiene una actividad asociada

					}

					if(gDialogos.duendes[i].estado == "Inactivo"){

						auxDialogos = new string[gDialogos.duendes[i].dialogoRojo.Length];

						for(int j=0; j < gDialogos.duendes[i].dialogoRojo.Length; j++)
						{
							auxDialogos [j] = gDialogos.duendes [i].dialogoRojo[j];

						}
					}

					if(gDialogos.duendes[i].estado == "Repetir"){
						auxDialogos = new string[gDialogos.duendes[i].dialogoAzul.Length];

						for(int j=0; j < gDialogos.duendes[i].dialogoAzul.Length; j++)
						{
							auxDialogos [j] = gDialogos.duendes [i].dialogoAzul[j];

						}

						idAuxActividad = gDialogos.duendes[i].idActividad;
						idAuxDuende = gDialogos.duendes[i].idDuende;
						resuelveActividadAux = gDialogos.duendes[i].resuelveActividad;
						repetirAux = true;
					}
				}

				if(gDialogos.duendes[i].tipo == "Habilidad")
				{
					auxDialogos = new string[gDialogos.duendes[i].dialogoHabilidad.Length];

					for(int j=0; j < gDialogos.duendes[i].dialogoHabilidad.Length; j++)
					{
						auxDialogos [j] = gDialogos.duendes [i].dialogoHabilidad[j];

					}

					if(gDialogos.duendes[i].estado == "Activo")
					{
						idAuxDuende = gDialogos.duendes[i].idDuende;
						idAuxDuendeSig = idAuxDuende + 1;
						resuelveActividadAux = gDialogos.duendes[i].resuelveActividad;	
					}

				}

				if(gDialogos.duendes[i].tipo == "Acertijo")
				{
					auxDialogos = new string[gDialogos.duendes[i].dialogoAcertijo.Length];

					for(int j=0; j < gDialogos.duendes[i].dialogoAcertijo.Length; j++)
					{
						auxDialogos [j] = gDialogos.duendes [i].dialogoAcertijo[j];

					}
				}
					
			}
		}

	}

	void OnTriggerExit(Collider Otro)
	{
		activarDialogo = false;
		finalizarDialogo = false;

		idAuxActividad = 0; // Se debe inicializar en 0 para que solo se active la actividad que le corresponde y cuando se presione enter al duende activo
		idAuxDuende = 0;
		resuelveActividadAux = false;

		repetirAux = false;

		//Icono Enter
		iconoDialogo.transform.position = new Vector3(0,0,0);
		iconoDialogo.SetActive(false);



	}

	public void ActivarPanel()
	{
		panel.SetActive (true);
		cJugador.DetenerJugador = true;
	}

	public void DesactivarPanel()
	{
		panel.SetActive (false);
		cJugador.DetenerJugador = false;
	}
}
