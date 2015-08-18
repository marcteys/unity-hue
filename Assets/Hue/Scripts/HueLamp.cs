using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;

using MiniJSON;

[ExecuteInEditMode]
public class HueLamp : MonoBehaviour {
	public string devicePath;
	public bool on = true;
	public Color color = Color.white;

	private bool oldOn;
	private Color oldColor;
	
	void Update () {
		if (oldOn != on || oldColor != color) {
			HueBridge bridge = GetComponentInParent<HueBridge>();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://"+ bridge.hostName+  "/api/" + bridge.username + "/lights/" + devicePath + "/state");
            Debug.Log("http" + bridge.hostName + bridge.portNumber + "/api/" + bridge.username + "/lights/" + devicePath + "/state");
			request.Method = "PUT";

			Vector3 hsv = HSVFromRGB (color);

			var state = new Dictionary<string, object> ();
			state ["on"] = on;
			state ["hue"] = (int)(hsv.x / 360.0f * 65535.0f);
			state ["sat"] = (int)(hsv.y * 255.0f);
			state ["bri"] = (int)(hsv.z * 255.0f);
            if ((int)(hsv.z * 255.0f) == 0) state["on"] = false;

			byte[] bytes = System.Text.Encoding.ASCII.GetBytes (Json.Serialize (state));
			request.ContentLength = bytes.Length;
			
			System.IO.Stream s = request.GetRequestStream ();
			s.Write (bytes, 0, bytes.Length);
			s.Close ();

			HttpWebResponse response = (HttpWebResponse)request.GetResponse ();

			response.Close ();
		}

		oldOn = on;
		oldColor = color;
	}

	static Vector3 HSVFromRGB(Color rgb) {
		float max = Mathf.Max(rgb.r, Mathf.Max(rgb.g, rgb.b));
		float min = Mathf.Min(rgb.r, Mathf.Min(rgb.g, rgb.b));
		
		float brightness = rgb.a;
		
		float hue, saturation;
		if (max == min) {
			hue = 0f;
			saturation = 0f;
		} else {
			float c = max - min;
			if (max == rgb.r) {
				hue = (rgb.g - rgb.b) / c;
			} else if (max == rgb.g) {
				hue = (rgb.b - rgb.r) / c + 2f;
			} else {
				hue = (rgb.r - rgb.g) / c + 4f;
			}

			hue *= 60f;
			if (hue < 0f) {
				hue += 360f;
			}
			
			saturation = c / max;
		}

		return new Vector3(hue, saturation, brightness);
	}
}
