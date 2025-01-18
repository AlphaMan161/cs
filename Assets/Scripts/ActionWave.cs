// dnSpy decompiler from Assembly-CSharp.dll class: ActionWave
using System;
using UnityEngine;

public class ActionWave
{
	public ActionWave(GameObject actionPrefab, long launchTime, int trajectoryIndex, int count, ActionType actionType, short team)
	{
		this.actionPrefab = actionPrefab;
		this.launchTime = launchTime;
		this.trajectoryIndex = trajectoryIndex;
		this.count = count;
		this.actionType = actionType;
		this.team = team;
	}

	public GameObject actionPrefab;

	public long launchTime;

	public int trajectoryIndex;

	public int count;

	public ActionType actionType;

	public short team;
}
