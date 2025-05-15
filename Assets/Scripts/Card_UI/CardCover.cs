using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardCover : MonoBehaviour
{
    private List<GameObject> coverList = new List<GameObject>();
    private int currentIndex = 0;
    private bool allHidden = false;

    // Start is called before the first frame update
    void Start()
    {
        // 获取所有tag为"Cover"的子物体
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Cover") && child.GetComponent<Image>() != null)
            {
                coverList.Add(child.gameObject);
                child.gameObject.SetActive(false); // 初始时全部隐藏
            }
        }

        // 如果有遮罩，显示第一个
        if (coverList.Count > 0)
        {
            coverList[0].SetActive(true);
            currentIndex = 0;
            allHidden = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 检测鼠标点击
        if (Input.GetMouseButtonDown(0) && coverList.Count > 0)
        {
            if (allHidden)
            {
                // 如果所有遮罩都已隐藏，则显示第一个
                coverList[0].SetActive(true);
                currentIndex = 0;
                allHidden = false;
            }
            else if (currentIndex == coverList.Count - 1)
            {
                // 如果当前是最后一个遮罩，则隐藏它和所有其他遮罩
                coverList[currentIndex].SetActive(false);
                allHidden = true;
            }
            else
            {
                // 隐藏当前遮罩，显示下一个
                coverList[currentIndex].SetActive(false);
                currentIndex++;
                coverList[currentIndex].SetActive(true);
            }
        }
    }
}
