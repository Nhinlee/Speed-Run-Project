using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    void Die();
    CharacterState State {get;} 
}
