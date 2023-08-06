using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
	public GameObject credits;

	private void Start()
	{
		credits.SetActive(false);
	}

	public void Clicked()
	{
		credits.SetActive(true);
	}

	public void Close()
	{
		credits.SetActive(false);
	}
}
