using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager {

    private int currentTime;
    private int maxTime;

	public TimeManager(int maxTime) {
        currentTime = 0;
        this.maxTime = maxTime;
    }

    public void resetTime() {
        currentTime = 0;
    }

    public void incrementTime() {
        currentTime++;
        if(currentTime == maxTime) {
            /* TO DO - Call Story Manager to call end of day */
        }
    }

    public int getTime() {
        return currentTime;
    }

}
