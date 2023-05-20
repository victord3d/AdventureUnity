using UnityEngine;
using System.Collections;

public class DesaparecerPlataforma : MonoBehaviour {

	public GameObject plataforma;
	public float tiempo;

	void Start () 
	{
		cambiarEstado();
	}
	

	public void cambiarEstado()
	{
		if(plataforma.activeInHierarchy == true)
		{
			plataforma.SetActive(false);
		}
		else if(plataforma.activeInHierarchy == false)
		{
			plataforma.SetActive(true);
		}

		Invoke("cambiarEstado", tiempo);

	}
}
