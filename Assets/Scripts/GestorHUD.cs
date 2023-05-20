using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[ExecuteInEditMode]

public class GestorHUD : MonoBehaviour {

	private InventarioDelJugador inventarioJugador;
	public GameObject textoDiamante;
	public GameObject textoEsfera;

	public GameObject textoVida;
	public Scrollbar barraDeEnergia;

	ControladorDelJugador cJugador;
	private Animator animacionPerderVida;

	bool activarCorrutinaPerderVida;

	void Start () {

		cJugador = GameObject.FindGameObjectWithTag ("Player").GetComponent<ControladorDelJugador> ();
		animacionPerderVida = GameObject.Find("PanelPerderVida").GetComponent<Animator>();

		activarCorrutinaPerderVida = false;

	}
		
	void Awake()
	{
		inventarioJugador = GameObject.FindGameObjectWithTag ("Player").GetComponent<InventarioDelJugador> ();
	}
	

	void Update () {
		
		textoDiamante.GetComponent<Text>().text = inventarioJugador.diamantesAgarrados.ToString ();
		textoEsfera.GetComponent<Text>().text = string.Concat(inventarioJugador.esferasAgarradas.ToString (),"/5");
		textoVida.GetComponent<Text>().text = inventarioJugador.vidas.ToString ();
		barraDeEnergia.size = inventarioJugador.energia;

		if(barraDeEnergia.size == 0 && activarCorrutinaPerderVida == false)
		{
			activarCorrutinaPerderVida = true;
		
			//Pierde una vida
			inventarioJugador.vidas--;

			if(inventarioJugador.vidas > 0)
			{
				//Animacion de perder vida
				StartCoroutine(CorrutinaPerderVida(0.5f));
				StartCoroutine(CorrutinaPerderVidaAnimacion(1.0f));
			}

		}
			
	}


	IEnumerator CorrutinaPerderVida(float tiempoEspera) {  

		print("Entro a la corrutina");

		//Comienza la Corrutina

		cJugador.DetenerJugador = true;

		yield return new WaitForSeconds(tiempoEspera);

		//Finaliza la Corrutina
		cJugador.DetenerJugador = false;
		cJugador.gameObject.transform.position = new Vector3(0,0,0);
		cJugador.gameObject.transform.rotation = new Quaternion (0,0,0,0);
		cJugador.camara.transform.position = new Vector3(0,0,0);
		cJugador.camara.transform.rotation = new Quaternion (14.92f,0,0,0);

		inventarioJugador.energia = 1.0f;

		activarCorrutinaPerderVida = false;

	}

	IEnumerator CorrutinaPerderVidaAnimacion(float tiempoEspera) { 
		//Animacion Perder Vida
		animacionPerderVida.SetBool("PerdioVida",true);
		yield return new WaitForSeconds(tiempoEspera);
		animacionPerderVida.SetBool("PerdioVida",false);

	}

}
