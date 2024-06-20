using System;
using Features.Managers;
using UnityEngine;

namespace Features.RandomPassenger.Scripts
{
    public class PassengerVisual : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _hair;
        [SerializeField] private SpriteRenderer _clothes;
        [SerializeField] private SpriteRenderer _body;
        [SerializeField] private SpriteRenderer _misc;

        /// <summary>
        /// Updates all the sprites when enabled
        /// </summary>
        private void OnEnable()
        {
            GameManager.Instance.PassengerVisualManager.UpdateSkin(this);
        }

        /// <summary>
        /// Set the new sprites
        /// </summary>
        /// <param name="hair">Hair, beard or helmet sprite</param>
        /// <param name="clothes">clothes sprite</param>
        /// <param name="body">Skin color sprite</param>
        /// <param name="misc">Hand held sprite</param>
        public void SetVisuals(Sprite hair, Sprite clothes, Sprite body, Sprite misc)
        {
            _hair.sprite = hair;
            _clothes.sprite = clothes;
            _body.sprite = body;
            _misc.sprite = misc;
        }
    }
}