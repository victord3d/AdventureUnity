using UnityEngine;
using System.Collections;

public class MovimientoCircular : MonoBehaviour {

	float cTiempo;

	public float velocidad;
	public float ancho;
	public float largo;
	public float velocidadDeRotacion;

	void Start () {
		cTiempo = 0;
	}
		
	void Update () {
			
		cTiempo += Time.deltaTime*velocidad;
		float x = Mathf.Cos (cTiempo)*ancho;
		float y = transform.localPosition.y;
		float z = Mathf.Sin (cTiempo)*largo;

		transform.localPosition = new Vector3 (x,y,z);
	
		transform.Rotate(Vector3.up,(-1)*velocidadDeRotacion*Time.deltaTime);

	}
}
