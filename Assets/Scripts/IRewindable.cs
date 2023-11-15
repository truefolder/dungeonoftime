using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRewindable
{
	public void Record();

	public void Rewind();

	public void RemoveLast();
}
