using System;
using Hono.Scripts.Battle.Core.Base;
using UnityEngine;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

namespace Hono.Scripts.Battle.Core
{
    public enum IndicatorType
    {
        None,
        Front,
        Direction,
        Circle,
        Rectangle,
        SingleTarget
    }

    public class SkillIndicator : MonoSingleton<SkillIndicator>
    {
        [Header("References")]
        public GameObject directionIndicator;
        public GameObject circleIndicator;
        public GameObject rectangleIndicator;
        public GameObject singleTargetIndicator;
        public GameObject skillCastRange;

        public LayerMask groundLayer;
        public LayerMask targetLayer;

        private Camera _mainCamera;
        private bool _isActive;
        private Unit _user;
        private CombatComp.Skill _skill;
        private IndicatorType _curIndicatorType;

        private void OnEnable()
        {
            HideAllIndicators();
        }

        public void Open(Unit user, CombatComp.Skill skill)
        {
            _user = user;
            _skill = skill;
            _isActive = true;
            
            transform.position = user.UnitTransform.Pos;
            transform.rotation = user.UnitTransform.Rot;
            
            HideAllIndicators();

            //开启技能范围
            skillCastRange.SetActive(true);
            var castRange = Vector3.one * (skill.SkillData.castRange * 2);
            castRange.y = 0.02f;
            skillCastRange.transform.localScale = castRange;

            //指示器
            switch (skill.SkillData.skillTargetType)
            {
                case ESkillTargetSelectType.Target:
                    //singleTargetIndicator.SetActive(true);
                    //singleTargetIndicator.transform.localScale = castRange;
                    _curIndicatorType = IndicatorType.SingleTarget;
                    break;
                case ESkillTargetSelectType.Position:
                    rangeIndicator(skill.SkillData.rangeFilter.BoxData);
                    break;
                case ESkillTargetSelectType.Front:
                    directionIndicator.SetActive(true);
                    directionIndicator.transform.localScale = new Vector3(1f, 1f, 1);
                    _curIndicatorType = IndicatorType.Front;
                    break;
                case ESkillTargetSelectType.Direction:
                    directionIndicator.SetActive(true);
                    directionIndicator.transform.localScale = new Vector3(1f, 1f, 1);
                    _curIndicatorType = IndicatorType.Direction;
                    break;
            }
        }

        private void rangeIndicator(CheckBoxData checkBoxData)
        {
            switch (checkBoxData.ShapeType)
            {
                case ECheckBoxShapeType.Cube:
                    rectangleIndicator.SetActive(true);
                    rectangleIndicator.transform.localScale =
                        new Vector3(checkBoxData.Length, 0.04f, checkBoxData.Width);
                    _curIndicatorType = IndicatorType.Rectangle;
                    break;
                case ECheckBoxShapeType.Sphere:
                    circleIndicator.SetActive(true);
                    circleIndicator.transform.localScale = new Vector3(checkBoxData.Radius, 0.04f, checkBoxData.Radius);
                    _curIndicatorType = IndicatorType.Circle;
                    break;
            }
        }

        private void Update()
        {
            if (_mainCamera == null)
            {
                _mainCamera = Camera.main;
            }
            if (_isActive)
            {
                transform.position = _user.UnitTransform.Pos;
                transform.rotation = _user.UnitTransform.Rot;
                UpdateIndicatorPosition();
                HandleInput();
            }
        }

        private void UpdateIndicatorPosition()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, groundLayer))
            {
                switch (_curIndicatorType)
                {
                    case IndicatorType.Front:
                        directionIndicator.transform.rotation = _user.UnitTransform.Rot;
                        break;
                    case IndicatorType.Direction:
                        Vector3 lookDir = hit.point - _user.UnitTransform.Pos;
                        lookDir.y = 0;
                        if (lookDir != Vector3.zero)
                        {
                            directionIndicator.transform.rotation = Quaternion.LookRotation(lookDir);
                        }
                        break;
                    case IndicatorType.Circle:
                        if (Vector3.Distance(hit.point, _user.UnitTransform.Pos) < _skill.SkillData.castRange)
                        {
                            circleIndicator.transform.position = hit.point;
                        }
                        else
                        {
                            circleIndicator.transform.localPosition = (hit.point - _user.UnitTransform.Pos).normalized *
                                                                      _skill.SkillData.castRange;
                        }
                        break;
                    case IndicatorType.Rectangle:
                        if (Vector3.Distance(hit.point, _user.UnitTransform.Pos) < _skill.SkillData.castRange)
                        {
                            rectangleIndicator.transform.position = hit.point;
                        }
                        else
                        {
                            rectangleIndicator.transform.localPosition = (hit.point - _user.UnitTransform.Pos).normalized *
                                                                         _skill.SkillData.castRange;
                        }
                        break;
                }
            }
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                switch (_curIndicatorType)
                {
                    case IndicatorType.Front:
                    case IndicatorType.Direction:
                        _skill.SelectYAxisAngle = directionIndicator.transform.rotation.eulerAngles.y;
                        _skill.Play();
                        Close();
                        break;
                    case IndicatorType.Circle: {
	                    var selectPos = circleIndicator.transform.position;
	                    selectPos.y = 0;
	                    _skill.SelectWorldPos = selectPos;
	                    var curPos = _user.UnitTransform.Pos;
	                    curPos.y = 0;
	                    _skill.SelectYAxisAngle = Vector3.SignedAngle(_user.UnitTransform.Forward, (selectPos - curPos).normalized,Vector3.up);
	                    World.Query.SearchUnits(_user,_skill.SelectWorldPos,0,_skill.SkillData.rangeFilter,ref _skill.SelectUnitsInArea);
	                    _skill.Play();
	                    Close();
	                    break;
                    }
                    case IndicatorType.Rectangle: {
	                    var selectPos = rectangleIndicator.transform.position;
	                    selectPos.y = 0;
	                    _skill.SelectWorldPos = selectPos;
	                    var curPos = _user.UnitTransform.Pos;
	                    curPos.y = 0;
	                    _skill.SelectYAxisAngle = Vector3.SignedAngle(_user.UnitTransform.Forward, (selectPos - curPos).normalized,Vector3.up);
	                    World.Query.SearchUnits(_user, _skill.SelectWorldPos, 0, _skill.SkillData.rangeFilter, ref _skill.SelectUnitsInArea);
	                    _skill.Play();
	                    Close();
	                    break;
                    }
                    case IndicatorType.SingleTarget:
                        if (TryGetSingleTarget(out int targetUid))
                        {
                            _skill.TargetUid = targetUid;
                            _skill.Play();
                            Close();
                        }
                        else
                        {
                            Debug.Log($"该目标 {targetUid} 不符合要求");
                        }
                        break;
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Close();
            }
        }

        private bool TryGetSingleTarget(out int targetUid)
        {
            targetUid = -1;
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000, targetLayer))
            {
                if (!hit.collider.gameObject.TryGetComponent<UOProxyPhysicsHandler>(out var proxy))
                {
                    return false;
                }

                targetUid = proxy.Unit.Uid;
                return World.Query.ConditionFilter(_user, proxy.Unit, _skill.SkillData.targetFilter);
            }
            
            return false;
        }

        private void HideAllIndicators()
        {
            skillCastRange.SetActive(false);
            directionIndicator.SetActive(false);
            circleIndicator.SetActive(false);
            rectangleIndicator.SetActive(false);
            //singleTargetIndicator.SetActive(false);
        }

        private void Close()
        {
            _user = null;
            _skill = null;
            _isActive = false;
            HideAllIndicators();
        }
    }
}