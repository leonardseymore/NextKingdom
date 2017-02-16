using BitAura;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class Game : MonoBehaviour {

    

    
    
    IEnumerator HelpMe()
    {
        Card card = GetBestCardToPlay(CurrentPlayerCards);
        if (card == null)
        {

        }
        yield return null;
    }
}
