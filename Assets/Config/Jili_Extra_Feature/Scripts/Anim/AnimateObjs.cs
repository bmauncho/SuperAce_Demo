namespace Config_Assets
{
    using UnityEngine;
    using UnityEngine.Events;
    [System.Serializable]
    public class AnimateObjsEnd : UnityEvent { }
    public class AnimateObjs : MonoBehaviour
    {
        public bool IsLoop = true;
        public float Rate = 0.1f;
        public GameObject[] Objs;
        float Timestamp;
        int Active;
        public AnimateObjsEnd OnEnd;
        void Update()
        {
            if (Timestamp < Time.time)
            {
                for (int i = 0; i < Objs.Length; i++)
                {
                    if (i == Active)
                    {
                        Objs[i].SetActive(true);
                    }
                    else
                    {
                        Objs[i].SetActive(false);
                    }
                }
                Timestamp = Time.time + Rate;
                Active += 1;
                if (Active > Objs.Length - 1)
                {
                    if (IsLoop)
                    {
                        Active = 0;
                    }
                    else
                    {
                        Active = Objs.Length - 1;
                    }
                    OnEnd.Invoke();
                }
            }

        }
        public void ResetAnim()
        {
            Active = 0;
            for (int i = 0; i < Objs.Length; i++)
            {
                if (i == Active)
                {
                    Objs[i].SetActive(true);
                }
                else
                {
                    Objs[i].SetActive(false);
                }
            }
        }
    }
}