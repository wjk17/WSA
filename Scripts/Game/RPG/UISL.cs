using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Esa;
using Esa.UI_;
using UnityEngine.SceneManagement;
using System;
using System.IO;
[Serializable]
public class SaveFile
{
    public float totalGameTime;
    public string name;
    public int lvl = 1;
    public int exp = 0;
    public int hp = 100;
    public int hpMax = 100;
    public int mp = 100;
    public int mpMax = 100;
    public int agility = 10; // 一秒一次动作
}
[Serializable]
public class SaveFileSetting
{
    public int idxSel;
}
public class UISL : Singleton<UISL>
{
    Button_Row buttons;
    public Button btnReturn;
    public int idxSel;
    public static string _filePath = "Save/";
    public static string fileNamePrefix = "file_";
    public static string filePath
    {
        get
        {
            var path = Path.Combine(Application.streamingAssetsPath, _filePath);
            return path;
        }
    }
    public static string settingPath
    {
        get
        {
            return filePath + "setting";
        }
    }
    public static string path(int i)
    {
        return filePath + fileNamePrefix + i.ToString() + ".sf";
    }
    public override void _Start()
    {
        base._Start();
        buttons = GetComponentInChildren<Button_Row>();
        btnReturn.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            UITitleMenu.I.buttons.gameObject.SetActive(true);
        });
    }
    private void LoadList()
    {
        //load savefile list
        DirectoryInfo dir = new DirectoryInfo(filePath);
        FileInfo[] fis = dir.GetFiles("*.sf", SearchOption.TopDirectoryOnly);
        for (int i = 0; i < 5; i++)
        {
            buttons.btns[i].GetComponentInChildren<Text>().text =
                buttons.buttons[i] + "          "
                + "          " + "          "
                + "          " + "          " + "空白";
            buttons.clickable[i] = false;
        }
        foreach (var fi in fis)
        {
            var fileName = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
            var n = fileName.Substring(fileName.Length - 1);
            int i; int.TryParse(n, out i);
            var sf = Serializer.XMLDeSerialize<SaveFile>(path(i));
            var s = (int)sf.totalGameTime;
            var m = s / 60;
            s = s % 60;
            var h = m / 60;
            m = m % 60;
            buttons.btns[i].GetComponentInChildren<Text>().text =
                buttons.buttons[i] + "                  " + sf.name +
                "   测试场景" +
                "   等级 " + sf.lvl.ToString() +
                "   总游戏时间 " + h + "小时" + m + "分钟" + s + "秒";


            buttons.clickable[i] = true;
        }
        for (int i = 0; i < 5; i++)
        {
            var imgs = buttons.btns[i].GetComponentsInChildren<Image>(true);
            foreach (var img in imgs)
            {
                img.enabled = buttons.clickable[i];
            }
        }
    }
    public void NewGameUI()
    {
        gameObject.SetActive(true);
        LoadList();
        buttons.onClick = OnClick_New;
    }
    public void LoadGameUI()
    {
        gameObject.SetActive(true);
        LoadList();
        buttons.onClick = OnClick_Load;
    }
    private void OnClick_New(int i)
    {
        gameObject.SetActive(false);
        UINaming.I.gameObject.SetActive(true);
        UINaming.I.confirm.onClick.AddListener(() =>
        {
            if (UINaming.I.charName.text.Length < UINaming.I.minLength)
            {
                return;
            }
            Serializer.XMLSerialize(new SaveFile() { name = UINaming.I.charName.text }, path(i));
            Serializer.XMLSerialize(new SaveFileSetting() { idxSel = i }, settingPath);
            loading.SetActive(true);
            LoadScene();
        });
    }
    private void LoadScene()
    {
        //StartCoroutine()
        UI.I.StartCoro(LoadSceneDelay());
    }
    IEnumerator LoadSceneDelay()
    {
        btnReturn.enabled = false;
        yield return new WaitForSeconds(delayTime);
        //print(loading);
        SceneManager.LoadSceneAsync("Level_0", LoadSceneMode.Single);
    }
    public float delayTime = 1.5f;
    public GameObject loading;
    private void OnClick_Load(int i)
    {
        if (!buttons.clickable[i]) return;
        buttons.onClick = null;
        loading.SetActive(true);
        Serializer.XMLSerialize(new SaveFileSetting() { idxSel = i }, settingPath);
        LoadScene();
    }
}
