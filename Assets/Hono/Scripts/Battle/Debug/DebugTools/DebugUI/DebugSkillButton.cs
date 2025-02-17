using System;
using Hono.Scripts.Battle.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hono.Scripts.Battle.DebugTools.DebugUI
{
    public class DebugSkillButton : MonoBehaviour
    {
        public int skillId;
        public bool autoLearn;
        public Button button;
        public TMP_Text text;
        public void Awake()
        {
            button.onClick.AddListener(() =>
            {
                if(skillId == 0) return;
                
                if (World.Current.PlayerControlUnit != null)
                {
                    if(World.Current.PlayerControlUnit.TryGetComponent<CombatComp>(out var combatComp))
                    {
                        if (!combatComp.HasSkill(skillId) && autoLearn)
                        {
                            combatComp.LearnSkill(skillId);
                        }
                        combatComp.UseSkill(skillId);
                    }
                }
            });
        }

        public void Update()
        {
            text.text = skillId.ToString();
        }
    }
}