using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeText : MonoBehaviour
{
	public Text text;
	public Slider slider;
	float sliderValue;
	// Use this for initialization
	void Start()
	{
		sliderValue = slider.value;
		text.text = sliderValue.ToString();
	}

	// Update is called once per frame
	void Update()
	{
		sliderValue = slider.value;
		text.text = sliderValue.ToString();
	}
}
