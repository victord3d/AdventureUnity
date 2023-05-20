using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InfoCartel : MonoBehaviour {

	GestorDeCarteles gCarteles;
	ControladorDelJugador cJugador;
	public GameObject panelCartel;		// Panel independiente del cartel
	public Text textoCartel; 			// Texto independiente del cartel

	bool activarDialogo;				// Al activar el trigger se prepara para el dialogo
	public string[] auxDialogos;		// Obtiene los dialogos de la lista de carteles
	int contadorDialogo;				// Dialogo actual por cada enter
	bool finalizarDialogo;				// Variable para que se ejecute 1 sola vez todo el dialogo

	//Icono Enter
	GameObject iconoDialogo;

	void Awake()
	{
		iconoDialogo = GameObject.Find ("IconoDialogo");
	}
		
	void Start () {

		gCarteles = GameObject.FindGameObjectWithTag ("Player").GetComponent<GestorDeCarteles> ();
		cJugador = GameObject.FindGameObjectWithTag ("Player").GetComponent<ControladorDelJugador> ();

		activarDialogo = false;
		contadorDialogo = 0;
		finalizarDialogo = false;

		DesactivarPanel ();

		//Icono Enter
		iconoDialogo.SetActive(false);

	}
		
	void Update () {

		if(Input.GetKeyDown(KeyCode.Return))
		{
			if( activarDialogo == true )
			{
				if(contadorDialogo == auxDialogos.Length)
				{
					// Al terminar de hablar con el Duende
					DesactivarPanel ();
					activarDialogo = false;
				}

				if( contadorDialogo < auxDialogos.Length)
				{
					ActivarPanel ();
					textoCartel.text = auxDialogos [contadorDialogo];
					contadorDialogo++;
				}
			}

			if(activarDialogo == false && finalizarDialogo == true)
			{
				contadorDialogo = 0;
				finalizarDialogo = false;
			}


		}

	}

	void OnTriggerEnter(Collider Otro)
	{
		activarDialogo = true;
		finalizarDialogo = true;

		//Icono Enter
		iconoDialogo.transform.position = new Vector3(transform.parent.position.x,transform.parent.position.y + 2.12f, transform.parent.position.z);
		iconoDialogo.transform.rotation = transform.parent.rotation;
		iconoDialogo.SetActive(true);

		for(int i=0; i<gCarteles.carteles.Length; i++)
		{
			if(gameObject.GetInstanceID() == gCarteles.carteles[i].zonaCartel.GetInstanceID())
			{
				auxDialogos = new string[gCarteles.carteles[i].informacionCartel.Length];

				for(int j=0; j<gCarteles.carteles[i].informacionCartel.Length; j++)
				{
					auxDialogos [j] = gCarteles.carteles[i].informacionCartel[j];
				}
			}
		}

	}

	void OnTriggerExit(Collider Otro)
	{
		activarDialogo = false;
		finalizarDialogo = false;

		//Icono Enter
		iconoDialogo.transform.position = new Vector3(0,0,0);
		iconoDialogo.SetActive(false);
	}

	public void ActivarPanel()
	{
		panelCartel.SetActive (true);
		cJugador.DetenerJugador = true;
	}

	public void DesactivarPanel()
	{
		panelCartel.SetActive (false);
		cJugador.DetenerJugador = false;
	}

}
