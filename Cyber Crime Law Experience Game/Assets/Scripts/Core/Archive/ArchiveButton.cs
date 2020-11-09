using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArchiveButton : MonoBehaviour
{
    void Start() {
        OnClick(0);
    }
    [SerializeField] private TextMeshProUGUI showDataText;
    public void OnClick(int array){
        string data = ArchiveManager.instance.data[array].Split(':')[0];
        if(ArchiveManager.instance.milestone.milestones.Exists(x => x == data)){
            showDataText.text = ArchiveManager.instance.data[array].Split(':')[1];
            return;
        }
        showDataText.text = "ไม่ทราบข้อมูล เนื่องจากท่านยังไม่ได้เรียนรู้ข้อมูลนี้";
    }
}
