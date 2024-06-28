using System;
using Features.Managers;
using UnityEngine;

namespace Features.RandomPassenger.Scripts
{
    public class StaffVisual : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _hair;
        [SerializeField] private SpriteRenderer _clothes;
        [SerializeField] private SpriteRenderer _body;
        [SerializeField] private SpriteRenderer _misc;

        /// <summary>
        /// Updates all the sprites when enabled
        /// </summary>
        private void Start()
        {
            GameManager.Instance.PassengerVisualManager.UpdateRace(this);
        }

        /// <summary>
        /// Set the new sprites
        /// </summary>
        /// <param name="hair">Hair, beard or helmet sprite</param>
        /// <param name="clothes">clothes sprite</param>
        /// <param name="body">Skin color sprite</param>
        /// <param name="misc">Hand held sprite</param>
        public void SetSkinColor(Sprite body)
        {
            _body.sprite = body;
        }
    }
}