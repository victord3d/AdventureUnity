using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CargarYGuardarPartida : MonoBehaviour {

	private InventarioDelJugador inventarioJugador;
	public GameObject panelPause;
	ControladorDelJugador cJugador;
	GestorDeDialogos gDialogos;
	public int cantDuendes;

	bool pauseActivo;

	private Animator animacionGameOver;
	bool activarCorrutinaGameOver;

	void Start () {
	
		inventarioJugador = GameObject.FindGameObjectWithTag ("Player").GetComponent<InventarioDelJugador> ();
		cJugador = GameObject.FindGameObjectWithTag ("Player").GetComponent<ControladorDelJugador> ();
		gDialogos = GameObject.FindGameObjectWithTag ("Player").GetComponent<GestorDeDialogos> ();

		panelPause.SetActive(false);
		pauseActivo = false;

		if(IniciarPartida.cargarPartida)
		{
			inventarioJugador.diamantesAgarrados = PlayerPrefs.GetInt("datosDiamantes");
			inventarioJugador.esferasAgarradas = PlayerPrefs.GetInt("datosEsferas");
			cantDuendes = PlayerPrefs.GetInt("datosUltimoDuende");
			gDialogos.CargarDuendes (cantDuendes);

			inventarioJugador.vidas = PlayerPrefs.GetInt("datosVidas");

		}

		if(IniciarPartida.cargarPartida == false)
		{
			gDialogos.IniciarDuendes ();
		}
			
		animacionGameOver = GameObject.Find("PanelGameOver").GetComponent<Animator>();
		activarCorrutinaGameOver = false;

	}

	void Update () {

		if(Input.GetKeyDown(KeyCode.Escape) && pauseActivo == false)
		{
			PausarPartida ();
			return;
		}


		if(Input.GetKeyDown(KeyCode.Escape) && pauseActivo == true)
		{
			ContinuarPartida ();
			return;
		}
			
		if(inventarioJugador.vidas == 0 && activarCorrutinaGameOver == false)
		{
			activarCorrutinaGameOver = true;

			//Animacion GameOver
			StartCoroutine(CorrutinaGameOver(1.6f));
		}

	}

	public void PausarPartida()
	{
		panelPause.SetActive(true);
		cJugador.DetenerJugador = true;
		pauseActivo = true;

	}

	public void ContinuarPartida()
	{
		panelPause.SetActive(false);
		cJugador.DetenerJugador = false;
		pauseActivo = false;
	}

	public void GuardarPartida()
	{
		int cantActivos=0;
		
		PlayerPrefs.SetInt ("datosDiamantes",inventarioJugador.diamantesAgarrados);
		PlayerPrefs.SetInt ("datosEsferas", inventarioJugador.esferasAgarradas);
		//PlayerPrefs.Save();

		for(int i=0; i<gDialogos.duendes.Length; i++)
		{
			if(gDialogos.duendes[i].estado == "Activo")
			{
				PlayerPrefs.SetInt ("datosIdDuende",gDialogos.duendes[i].idDuende);
				cantActivos++;
			}
		}

		PlayerPrefs.SetInt ("datosUltimoDuende",cantActivos);
		print ("Guardando "+cantActivos);
		PlayerPrefs.SetInt ("datosVidas", inventarioJugador.vidas);
	}


	public void RegresarAIniciarPartida()
	{
		GuardarPartida ();
		SceneManager.LoadScene(0);
	}

	public void BorrarDatos()
	{
		PlayerPrefs.DeleteKey ("datosDiamantes");
		PlayerPrefs.DeleteKey ("datosEsferas");
		PlayerPrefs.DeleteKey ("datosVidas");
	}

	IEnumerator CorrutinaGameOver(float tiempoEspera) {  

		print("Entro a la corrutina GameOver");

		//Comienza la Corrutina

		cJugador.DetenerJugador = true;

		inventarioJugador.vidas = 3;
		inventarioJugador.diamantesAgarrados = 0;

		//Animacion Perder Vida
		animacionGameOver.SetBool("FinJuego",true);

		yield return new WaitForSeconds(tiempoEspera);

		//Finaliza la Corrutina

		animacionGameOver.SetBool("FinJuego",false);

		activarCorrutinaGameOver = false;

		GuardarPartida ();
		RegresarAIniciarPartida();

	}

}
