using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;

public class XMLLoader : MonoBehaviour {
	
	private string XMLFilePath = "../XML/chapter1.xml";
		
	private XmlDocument doc;
	
	private XmlNodeList nodeListText;

	// Use this for initialization
	void Start () {
		doc = new XmlDocument();
		doc.Load(XMLFilePath);
		
		nodeListText = doc.SelectNodes("test");
		
		Debug.Log(nodeListText[0].InnerText);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
