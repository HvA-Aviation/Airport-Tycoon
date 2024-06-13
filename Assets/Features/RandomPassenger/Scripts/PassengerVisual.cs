using UnityEngine;

namespace Features.RandomPassenger.Scripts
{
    public class PassengerVisual : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _hair;
        [SerializeField] private SpriteRenderer _clothes;
        [SerializeField] private SpriteRenderer _body;
        [SerializeField] private SpriteRenderer _misc;

        public void SetVisuals(Sprite hair, Sprite clothes, Sprite body, Sprite misc)
        {
            _hair.sprite = hair;
            _clothes.sprite = clothes;
            _body.sprite = body;
            _misc.sprite = misc;
        }
    }
}