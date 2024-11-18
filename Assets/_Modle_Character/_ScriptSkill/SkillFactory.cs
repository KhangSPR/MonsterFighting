using System.Collections.Generic;
using System;
using UnityEngine;

public static class SkillFactory
{
    private static Dictionary<string, Type> skillDictionary = new Dictionary<string, Type>()
    {
        { "Ball Of Darkness", typeof(BallOfDarkness) },
        {"Magic Vortex", typeof(MagicVortex) },
        {"Venomous Explosion Sphere", typeof(VenomousExplosionSphere) },
        {"Virtual Shield", typeof(VirtualShield) },

        // Thêm các skill khác
    };

    public static ISkill CreateSkill(string skillName)
    {
        if (skillDictionary.TryGetValue(skillName, out Type skillType))
        {
            return (ISkill)Activator.CreateInstance(skillType);
        }
        else
        {
            Debug.LogError("Skill with name " + skillName + " not found in dictionary.");
            return null;
        }
    }
}
