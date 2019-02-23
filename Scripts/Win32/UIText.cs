using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Forms;
public class UIText : MonoBehaviour {

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
        if (Input.GetMouseButtonDown(0))
        {
            MessageBox.Show("Nice");
        }
	}
}
