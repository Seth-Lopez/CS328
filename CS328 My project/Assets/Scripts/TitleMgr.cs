using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleMgr : MonoBehaviour
{
    [SerializeField] GameObject title;
    [SerializeField] GameObject buttons;
    [SerializeField] GameObject levelSelect;
    [SerializeField] GameObject options;
    TMP_Text titlePro;
    // Start is called before the first frame update
    void Start()
    {
        levelSelect.SetActive(false);
        options.SetActive(false);
        title.SetActive(true);
        title.transform.localPosition = new Vector3(-300, 335, 0);
        titlePro = title.GetComponentInChildren<TMP_Text>();
        titlePro.text = "Dungeon Adventurer";
        buttons.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(!buttons.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            titlePro.text = "Dungeon Adventurer";
            title.transform.localPosition = new Vector3(-300, 335, 0);
            levelSelect.SetActive(false);
            options.SetActive(false);
            buttons.SetActive(true);
        }
    }
    public void SrtBtn()
    {
        titlePro.text = "Select Your Level";
        title.transform.localPosition = new Vector3(-255, 335, 0);
        buttons.SetActive(false);
        levelSelect.SetActive(true);
    }
    public void OptBtn()
    {
        titlePro.text = "Options";
        title.transform.localPosition = new Vector3(75, 335, 0);
        buttons.SetActive(false);
        options.SetActive(true);
    }
    public void ExitBtn()
    {
        Application.Quit();
    }
    public void level1()
    {
        SceneManager.LoadScene("Dungeon_2");
    }
    public void level2()
    {
        SceneManager.LoadScene("Dungeon");
    }
    public void level3()
    {
        SceneManager.LoadScene("Dungeon");
    }
}
