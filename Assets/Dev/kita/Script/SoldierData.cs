using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Soldier/SoldierData")]
public class SoldierData : ScriptableObject
{
    public Sprite BreakSprite;//•ºm‚ª‰ó‚ê‚½‚Æ‚«‚Ì‰æ‘œ

    public Sprite SoldierBack;//•ºm‚Ì— ‚Ì‰æ‘œ

    public List<Soldier> SoldierList;//•ºm‚Ìƒf[ƒ^‚ğŠi”[‚·‚éƒŠƒXƒg
}
