using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using DG.Tweening;

[System.Serializable]
public class SettingInfo
{
    public float masterVolume = 1f;
    public float bgmVolume = 1f;
    public float effectVolume = 1f;
}

public class SettingHandler : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer = null;

    private readonly string masterVolume = "MasterVolume";
    private readonly string bgmVolume = "BGMVolume";
    private readonly string effectVolume = "EffectVolume";

    [SerializeField]
    private MainUIHandler mainUIHandler = null;

    [Header("¼¼ÆÃµé")]
    [SerializeField]
    private Text masterText;
    [SerializeField]
    private Slider masterSlider;

    [Space(10f)]
    [SerializeField]
    private Text bgmText;
    [SerializeField]
    private Slider bgmSlider;

    [Space(10f)]
    [SerializeField]
    private Text effectText;
    [SerializeField]
    private Slider effectSlider;

    private List<Text> texts = new List<Text>();
    private List<Slider> sliders = new List<Slider>();

    private int selectedIdx = 0;

    private SettingInfo settingInfo;

    private bool is_Selected = false;
    private Tweener moveTween = null;

    private readonly Color unSelectedCol = new Color(135f / 255f, 135f / 255f, 135f / 255f);

    private void Start()
    {
        texts.Add(masterText);
        texts.Add(bgmText);
        texts.Add(effectText);

        sliders.Add(masterSlider);
        sliders.Add(bgmSlider);
        sliders.Add(effectSlider);

        HighlightSelect();
        GetSettingInfo();
        
    }

    private void Update()
    {
        if (!is_Selected) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopSelect();
            mainUIHandler.SetSelect(true);
        }

        SelectSetting();
        ChangeSelectedVolume();
    }

    private void SelectSetting()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && selectedIdx < 2)
        {
            selectedIdx++;
            HighlightSelect();
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow) && selectedIdx > 0)
        {
            selectedIdx--;
            HighlightSelect();
        }
    }

    private void HighlightSelect()
    {
        for (int i = 0; i < texts.Count; i++)
        {
            texts[i].color = i == selectedIdx ? Color.white : unSelectedCol;
        }
    }

    private void ChangeSelectedVolume()
    {
        bool changed = false;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            sliders[selectedIdx].value = Mathf.Clamp(sliders[selectedIdx].value - (1f / 5f), 0f, 1f);

            changed = true;
            
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            sliders[selectedIdx].value = Mathf.Clamp(sliders[selectedIdx].value + (1f / 5f), 0f, 1f);
            
            changed = true;
        }

        if (changed)
        {
            switch (selectedIdx)
            {
                case 0:
                    SetVolume(masterVolume, sliders[selectedIdx].value);
                    break;

                case 1:

                    SetVolume(bgmVolume, sliders[selectedIdx].value);
                    break;

                case 2:

                    SetVolume(effectVolume, sliders[selectedIdx].value);
                    break;
            }

            SetSettingInfo();
            SaveSettingInfo();
        }
    }


    public void StartSelect()
    {
        moveTween.Kill();
        moveTween = transform.DOLocalMoveX(0f, 0.8f)
                    .SetEase(Ease.OutQuart);

        is_Selected = true;
    }

    public void StopSelect()
    {
        moveTween.Kill();
        moveTween = transform.DOLocalMoveX(1920f, 0.5f)
                    .SetEase(Ease.OutQuart);

        is_Selected = false;
    }


    private void SetVolume(string name, float val)
    {
        mixer.SetFloat(name, Mathf.Log10(val) * 20f);
    }

    private void GetSettingInfo()
    {
        string path = Path.Combine(Application.persistentDataPath, "settingInfo.txt");

        settingInfo = new SettingInfo();

        if (File.Exists(path))
        {
            LoadSettingInfo();
            ApplySettingInfo();
        }
        else
        {
            SaveSettingInfo();
        }
    }

    private void SetSettingInfo()
    {
        settingInfo.masterVolume = sliders[0].value;
        settingInfo.bgmVolume = sliders[1].value;
        settingInfo.effectVolume = sliders[2].value;
    }

    private void ApplySettingInfo()
    {
        sliders[0].value = settingInfo.masterVolume;
        sliders[1].value = settingInfo.bgmVolume;
        sliders[2].value = settingInfo.effectVolume;

        SetVolume(masterVolume, sliders[0].value);
        SetVolume(bgmVolume, sliders[1].value);
        SetVolume(effectVolume, sliders[2].value);
    }

    private void LoadSettingInfo()
    {
        string path = Path.Combine(Application.persistentDataPath, "settingInfo.txt");
        string json = File.ReadAllText(path);

        settingInfo = JsonUtility.FromJson<SettingInfo>(json);
    }

    private void SaveSettingInfo()
    {
        string json = JsonUtility.ToJson(settingInfo, true);
        string path = Path.Combine(Application.persistentDataPath, "settingInfo.txt");

        File.WriteAllText(path, json);
    }
}
