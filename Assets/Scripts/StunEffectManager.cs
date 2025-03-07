using System.Collections.Generic;
using UnityEngine;

public class StunEffectManager : MonoBehaviour
{
    private class StunEffectItem
    {
        public int offset;
        public float rotationSpeed;
        public float phaseShift = 0;
    }

    [SerializeField]
    private int nbIconsToDisplay = 4;

    [SerializeField]
    private GameObject stunIconPrefab;

    [SerializeField]
    private Transform pivot;

    [SerializeField]
    private float distanceWithPivot = 0.5f;

    [SerializeField]
    private int baseSpeed = 4;

    private List<StunEffect> listStunEffects = new List<StunEffect>();

    void Start()
    {
        transform.position = new Vector3(
            pivot.position.x,
            transform.position.y,
            transform.position.z
        );
        List<StunEffectItem> listObj = new List<StunEffectItem>();
        int nbItemsPerSide = (int)Mathf.Ceil((float) nbIconsToDisplay / 2);

        for (int i = 0; i < 2; i++)
        {
            for (var j = 0; j < nbItemsPerSide; j++)
            {
                int offset = i % 2 == 0 ? 1 : -1;
                listObj.Add(
                    new StunEffectItem
                    {
                        offset = offset,
                        rotationSpeed = baseSpeed,
                        phaseShift = offset * (Mathf.PI * ((float) j / nbItemsPerSide)),
                    }
                );
            }
        }

        for (int i = 0; i < nbIconsToDisplay; i++)
        {
            StunEffectItem stunEffectItem = listObj[i];

            Vector3 pos = new Vector3(
                distanceWithPivot * stunEffectItem.offset,
                transform.position.y,
                0
            );

            GameObject go = Instantiate(stunIconPrefab, transform, true);
            go.transform.localPosition = pos;
            go.name += $"_{i}";

            StunEffect stunEffect = go.GetComponent<StunEffect>();
            stunEffect.pivot = pivot;
            stunEffect.phaseShift = stunEffectItem.phaseShift;
            stunEffect.rotationSpeed = stunEffectItem.rotationSpeed;

            listStunEffects.Add(stunEffect);
        }
    }

    public void ToggleVisiblity(bool isVisible)
    {
        listStunEffects.ForEach((item) => item.ToggleVisiblity(isVisible));
    }
}
