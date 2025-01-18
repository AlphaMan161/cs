// dnSpy decompiler from Assembly-CSharp.dll class: ServerState
using System;
using System.Collections;
using System.Collections.Generic;

public struct ServerState
{
	public int peerCount;

	public byte cpuUsage;

	public short memory;

	public short lag;

	public short disconnectConnect;

	public float reportTime;

	public short ping;

	public Dictionary<string, Hashtable> glServerLoads;
}
