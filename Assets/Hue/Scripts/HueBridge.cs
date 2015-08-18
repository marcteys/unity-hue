using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;

using MiniJSON;

public class HueBridge : MonoBehaviour {
	public string hostName = "127.0.0.1";
	public int portNumber = 8000;
	public string username = "newdeveloper";

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DiscoverLights() {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + hostName + "/api/" + username + "/lights");
		HttpWebResponse response = (HttpWebResponse)request.GetResponse ();
        Debug.Log("http" + hostName + portNumber + "/api/" + username + "/lights");

		System.IO.Stream stream = response.GetResponseStream();
		System.IO.StreamReader streamReader = new System.IO.StreamReader(stream, System.Text.Encoding.UTF8);
		
		var lights = (Dictionary<string, object>)Json.Deserialize (streamReader.ReadToEnd());
		foreach (string key in lights.Keys) {
			var light = (Dictionary<string, object>)lights[key];

			foreach (HueLamp hueLamp in GetComponentsInChildren<HueLamp>()) {
				if (hueLamp.devicePath.Equals(key)) goto Found;
			}
			
			if (light["type"].Equals("Extended color light")) {
				GameObject gameObject = new GameObject();
				gameObject.name = (string)light["name"];
				gameObject.transform.parent = transform;
				gameObject.AddComponent<HueLamp>();
				HueLamp lamp = gameObject.GetComponent<HueLamp>();
				lamp.devicePath = key;
			}

		Found:
			;
		}
	}
}
