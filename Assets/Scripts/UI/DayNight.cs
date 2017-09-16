using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNight : MonoBehaviour {
	public Light sceneLight;
	public Color color0;
	public Color corlor1;
	private enum DayTime {Day, Night};
	private DayTime currentDayTime;

    //public GameObject sceneLight;
	void Start()
	{
		currentDayTime = DayTime.Day;
		Button btn = this.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}
		
	void TaskOnClick()
	{

		if (currentDayTime == DayTime.Day) {
			// Transform the light into NightTime light

			// Set current daytime to Night
		} 
		else {
			// Transform the light into DayTime light

			// Set current daytime to Day
		}
	}
}
