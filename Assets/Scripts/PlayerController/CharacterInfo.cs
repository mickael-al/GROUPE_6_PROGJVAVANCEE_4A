using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo
{
    // Player Speed | PlayerEmptyHand | PlayerTypeShot | AbleDash | Dash Speed | Strength | Player Score | Player Transform | 

        public string characterName;
        public float characterSpeed,characterStrength;
        public bool characterEmptyHand, isAbleDash;
        public int characterTypeShot, characterScore;
        public Vector3 characterPosition;
        public float character


        public CharacterInfo(string characterName, float characterSpeed, float characterStrength, bool characterEmptyHand, bool isAbleDash, int characterTypeShot, int characterScore, Vector3 characterPosition, Vector3 characterRotation) {
            this.characterName = characterName;
            this.characterSpeed = characterSpeed;
            this.characterStrength = characterStrength;
            this.characterEmptyHand = characterEmptyHand;
            this.isAbleDash = isAbleDash;
            this.characterTypeShot = characterTypeShot;
            this.characterScore = characterScore;
            this.characterPosition = characterPosition;
            this.characterRotation = characterRotation;
        }
    }       
