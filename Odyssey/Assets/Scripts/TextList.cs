using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextList : MonoBehaviour {

    public struct List
    {
        public int index;
        public Text upgrade;
    }
    private List[] list;
    private int n=0;

	void Start ()
    {
  
	}
	
	void Update ()
    {
		
	}

    void AddUpgrade(int number, Text tex)
    {
        List tempList;
        tempList.index = number;
        tempList.upgrade = tex;
        list[n] = tempList;
        n++;
    }

    List RandomUpgrade()
    { 
        int randInt = Random.Range(0, n);
        List rand = list[randInt];

        for (int i=randInt;i<n;i++)
        {
            list[i] = list[i + 1];
        }
        n--;
        return rand;
    }

}