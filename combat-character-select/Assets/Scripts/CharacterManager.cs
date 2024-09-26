using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    #region Inspector Fields
    [SerializeField] private SOCharacterData characterData;
    [SerializeField] private Transform socketP1;
    [SerializeField] private Transform socketP2;
    #endregion

    #region Private Fields
    private List<CharacterModel> mP1ModelCollection = new();
    private List<CharacterModel> mP2ModelCollection = new();
    private CharacterModel mActiveP1Model, mActiveP2Model;
    #endregion

    #region Static Fields
    private static readonly string[] mLastIdleClips = new string[2];
    private static readonly string[] mIdleClips = { "idle-mk1", "idle-mk2", "idle-mk3" };
    #endregion

    //Get a non repeating random clip from idle clips
    public static string GetNonRepeatingRandomClip(int playerIndex)
    {
        //Throw error if wrong player index
        if (playerIndex > 1)
            throw new System.Exception("Player index cannot be greater than 2");

        //Resultant clip
        string clip;

        do
        {
            //Get random index for clip
            int randomIndex = Random.Range(0, mIdleClips.Length);
            //Get the clip
            clip = mIdleClips[randomIndex];
        }
        //Loop until the new clip is not same as the last one for player index
        while (clip == mLastIdleClips[playerIndex]);

        //Update last clip string
        mLastIdleClips[playerIndex] = clip;

        //Return the resultant clip
        return clip;
    }

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
            //Set index
            character.PlayerIndex = selector.Index;

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
