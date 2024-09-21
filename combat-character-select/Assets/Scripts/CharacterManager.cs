using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering.Universal;
using Unity.Mathematics;
using static UnityEngine.Rendering.DebugUI.Table;
using System;

public class CharacterManager : MonoBehaviour
{

    [SerializeField] private SOCharacterData characterData;
    [SerializeField] private Transform socketP1;
    [SerializeField] private Transform socketP2;

    private List<CharacterModel> mP1ModelCollection = new();
    private List<CharacterModel> mP2ModelCollection = new();

    private CharacterModel mActiveP1Model, mActiveP2Model;


    public void OnCharacterChange(SelectionMenuController.SelectorHandler selector)
    {
        //Get the CharacterData object by index
        var cDataSO = characterData.GetByIndex(selector.Row, selector.Column);

        //Get the target prefab
        var targetPrefab = selector.Index == 0 ? cDataSO.p1Prefab : cDataSO.p2Prefab;

        //Ignore if prefab not available
        if (targetPrefab == null) return;

        //Get the targe model collection
        var targetCollection = selector.Index == 0 ? mP1ModelCollection : mP2ModelCollection;
        //Get the target socket
        var targetSocket = selector.Index == 0 ? socketP1 : socketP2;

        //Deactivate any active model
        if (selector.Index == 0)
        { if (mActiveP1Model != null) mActiveP1Model.gameObject.SetActive(false); }
        else
        { if (mActiveP2Model != null) mActiveP2Model.gameObject.SetActive(false); }

        //If character not available in target model collection
        if (!targetCollection.Any(item => item.CharacterKey == targetPrefab.CharacterKey))
        {
            //Instantiate
            var character = Instantiate(targetPrefab, targetSocket);
            //Reset transform
            character.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            //Add to target collection
            targetCollection.Add(character);

            //Set as active model
            if (selector.Index == 0)
                mActiveP1Model = character;
            else
                mActiveP2Model = character;
        }
        else
        {
            //If Player 1
            if (selector.Index == 0)
            {
                // Get from model collection as active model
                mActiveP1Model = targetCollection.FirstOrDefault(item => item.CharacterKey == targetPrefab.CharacterKey);

                // Set the active model to active
                if (mActiveP1Model != null)
                    mActiveP1Model.gameObject.SetActive(true);
            }
            else
            {
                // Get from model collection as active model
                mActiveP2Model = targetCollection.FirstOrDefault(item => item.CharacterKey == targetPrefab.CharacterKey);

                // Set the active model to active
                if (mActiveP2Model != null)
                    mActiveP2Model.gameObject.SetActive(true);
            }
        }
    }
}
