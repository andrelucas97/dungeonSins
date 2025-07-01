using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityDatabase", menuName = "CardsGame/AbilityDatabase")]
public class AbilityDatabase : ScriptableObject
{
    [SerializeField] private List<AbilityData> abilities;

    private Dictionary<CharacterAbility, AbilityData> lookup;

    public AbilityData GetAbilityData(CharacterAbility id)
    {
        if (lookup == null)
        {
            lookup = new Dictionary<CharacterAbility, AbilityData>();
            foreach (var ability in abilities)
            {
                if (!lookup.ContainsKey(ability.AbilityID))
                    lookup.Add(ability.AbilityID, ability);
            }
        }

        lookup.TryGetValue(id, out var data);
        return data;
    }

    public List<AbilityData> GetAbilityDataList(List<CharacterAbility> ids)
    {
        List<AbilityData> results = new();
        foreach (var id in ids)
        {
            var data = GetAbilityData(id);
            if (data != null)
                results.Add(data);
        }
        return results;
    }
}
