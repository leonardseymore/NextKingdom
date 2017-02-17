using BitAura;
using System.Collections;
using UnityEngine;

public partial class Game : MonoBehaviour {

    public void TogglePotion(string potionTypeStr)
    {
        PotionType potion = Utils.ParseEnum<PotionType>(potionTypeStr);
        CurrentPlayer.TogglePotion(potion);
    }

    IEnumerator AnimatePotion(PotionType potionType)
    {
        PlayerAvatar avatar = CurrentPlayer.PlayerAvatar;
        Sprite potionSprite = CardTextures.PotionSpriteLookup[potionType];
        avatar.PotionSprite = potionSprite;
        avatar.ShowPotionImage = true;
        switch (potionType)
        {
            case PotionType.FrekenKraken:
                yield return AnimateFrekenKrakenCR();
                break;
            case PotionType.TemptressShield:
                yield return AnimateTemptressShieldCR();
                break;
            case PotionType.BasicSword:
                yield return AnimateBasicSwordCR();
                break;
        }
        avatar.ShowPotionImage = false;
    }

    IEnumerator AnimateFrekenKrakenCR()
    {
        PlayerAvatar avatar = CurrentPlayer.PlayerAvatar;
        avatar.ShowDefensiveShieldRays = true;
        yield return new WaitForSeconds(2);
        avatar.ShowDefensiveShieldRays = false;
    }

    IEnumerator AnimateTemptressShieldCR()
    {
        PlayerAvatar avatar = CurrentPlayer.PlayerAvatar;
        avatar.ShowDefensiveShieldRays = true;
        yield return new WaitForSeconds(2);
        avatar.ShowDefensiveShieldRays = false;
    }

    IEnumerator AnimateBasicSwordCR()
    {
        PlayerAvatar avatar = CurrentPlayer.PlayerAvatar;
        AudioSource audio = AccumulatedCardsLightingBolt.GetComponent<AudioSource>();
        AccumulatedCardsLightingBolt.Play();
        audio.Play();
        yield return new WaitForSeconds(1f);
        AccumulatedCardsLightingBolt.Play();
        audio.Play();
    }
}
