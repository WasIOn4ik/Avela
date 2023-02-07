using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Spes.UI
{
    public class PlayerUpheadUI : MonoBehaviour
    {
        [SerializeField] protected TMP_Text playerName;
        [SerializeField] protected RectTransform hpFill;
        [SerializeField] protected RectTransform hpValue;
        protected IDamageable owner;
        protected Coroutine coroutine;

        protected float cachedFillHP;
        protected float cachedCurrentHP;
        protected float cachedMaxHP;

        protected float fillUpdateSpeed = 1f;

        public void Init(IDamageable newOwner, float updateSpeed)
        {
            SetOwner(newOwner);
            fillUpdateSpeed = updateSpeed * newOwner.GetInfo().maxHP / 100;
        }

        protected void SetOwner(IDamageable newOwner)
        {
            owner = newOwner;

            var info = owner.GetInfo();
            cachedCurrentHP = info.currentHP;
            cachedFillHP = info.currentHP;
        }

        public void UpdateUI()
        {
            playerName.text = owner.GetInfo().title;
            UpdateHP();
        }

        protected void UpdateHP()
        {
            var info = owner.GetInfo();
            float value = info.currentHP / info.maxHP;

            //Если был нанесен урон
            if (cachedCurrentHP > info.currentHP)
            {
                if (coroutine == null)
                {
                    cachedFillHP = info.currentHP;
                    coroutine = StartCoroutine(UpdateFill());
                }
            }
            //Иначе это лечение

            cachedCurrentHP = info.currentHP;
            cachedMaxHP = info.maxHP;
            hpValue.localScale = new Vector3(value, 1f, 1f);
        }

        protected IEnumerator UpdateFill()
        {
            yield return null;
            while (cachedFillHP > cachedCurrentHP)
            {
                cachedFillHP -= fillUpdateSpeed;
                hpFill.localScale = new Vector3(cachedFillHP / cachedMaxHP, 1f, 1f);
                yield return null;
            }
        }
    }
}
