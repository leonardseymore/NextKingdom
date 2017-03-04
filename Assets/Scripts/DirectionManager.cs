using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DirectionManager : MonoBehaviour
{
    public Image[] DirectionIndicators;
    Game game;
    float WaitTime = 0.35f;
    int currentIdx = 0;
    float LastWait = 0;
    Image LastDirectionIndicator;
    float FadeTime = 0.125f;
    float FadeOutTime = 0.5f;

    private void Awake()
    {
        game = FindObjectOfType<Game>();
        Game.OnDirectionChanged += OnDirectionChanged;

        foreach(Image dir in DirectionIndicators)
        {
            dir.CrossFadeAlpha(0.1f, FadeTime, true);
        }
    }

    void OnDirectionChanged()
    {

    }

    void Update()
    {
        if (LastWait == 0 || Time.time > LastWait + WaitTime)
        {
            if (LastDirectionIndicator != null)
            {
                LastDirectionIndicator.CrossFadeAlpha(0.1f, FadeOutTime, true);
            }
            LastWait = Time.time;

            currentIdx += game.Direction;
            if (currentIdx < 0)
            {
                currentIdx = DirectionIndicators.Length - 1;
            }
            else if (currentIdx >= DirectionIndicators.Length)
            {
                currentIdx = 0;
            }

            DirectionIndicators[currentIdx].CrossFadeAlpha(1f, FadeTime, true);
            LastDirectionIndicator = DirectionIndicators[currentIdx];
        }
    }
}