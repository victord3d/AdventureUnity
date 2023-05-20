using UnityEngine;
using System.Collections;

[System.Serializable]
public class Cartel
{
	public int idCartel;
	public GameObject zonaCartel;
	public string[] informacionCartel;

}

public class GestorDeCarteles : MonoBehaviour {

	public Cartel[] carteles;

}
