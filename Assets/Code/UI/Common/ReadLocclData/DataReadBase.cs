using UnityEngine;
using System.Collections;

public class DataReadBase {

	public string path;
	
	protected Hashtable data;
	
	public void init() {
		data = new Hashtable();
	}
	
	public virtual string getRootNodeName() { return "";}
	
	public virtual void appendAttribute(int key, string name, string value) {}

    public virtual bool HasRecord(int key)
    {
        return data.ContainsKey(key);
    }
}
