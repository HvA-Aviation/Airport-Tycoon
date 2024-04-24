using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    private SkillState _skillState;
    
    [SerializeField] private string _title;
    [SerializeField] private string _description;
    [SerializeField] private int _cost;
    [SerializeField] private int _researchTime;

    private float _timer;
    [Header("Objects for UI")]
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private Image _screenImage;
    [SerializeField] private Button _button;
   
    private void OnEnable()
    {        
        switch (_skillState)
        {
            case SkillState.notAvailable:
                _button.interactable = false;
                _screenImage.color = new Color(0,0,0,1);
                break;
            case SkillState.available:
                _button.interactable = true;
                _screenImage.color = new Color(0, 0, 0, .5f);
                break;
            case SkillState.inDevelopment:
                _button.interactable = false;
                Destroy(_screenImage.gameObject);
                break;
            case SkillState.bought:
                _button.interactable = false;
                Destroy(_screenImage.gameObject);
                break;
        }

    }

    private void Awake()
    {
        _titleText.text = _title;
        _descriptionText.text = _description;

        _skillState = SkillState.available;
    }

    public void TimerForInDevelopment()
    {
        if (_skillState != SkillState.inDevelopment) return;

        Debug.Log(_timer);

        _timer += Time.fixedDeltaTime;
        if(_timer >= _researchTime)
        {
            Debug.Log($"Finished research of {_title}");
            RDTreeManager.Instance.SkillsInDevelopment.Remove(this);
            _skillState = SkillState.bought;
        }
    }

    public void OnClick()
    {
        if (_skillState == SkillState.available)
        {
            _skillState = SkillState.inDevelopment;
            RDTreeManager.Instance.SkillsInDevelopment.Add(this);
        }
    }

    public enum SkillState { notAvailable, available, inDevelopment,  bought }
}
