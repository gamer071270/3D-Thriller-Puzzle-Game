using UnityEngine;
using System;
using System.Collections;

[System.Serializable]
public class Appliance
{
    /* 0 - Fridge
       1 - Air Conditioner
       2 - Microwave
       3 - Oven
       4 - Dishwasher
       5 - Kettle
       6 - Washing Machine
       7 - Television
    */
    public int applianceID;
    //public float fuseIncrease = 2f; // Sigorta yükünü artırma hızı
    public bool isRegular;

    private bool isOn;
    private bool fail = false;
    private float puzzleDuration = 180f; // Kaç saniye dayanmalı (örn: 3 dk)
    private float needInterval;
    private float needDuration;
    private float currentTemperature;
    private float tempIncrease;
    private float tempDecrease;
    private float timer;

    public void Initialize()
    {
        if (isRegular)
        {
            switch (applianceID)
            {
                case 0: // Fridge
                    currentTemperature = 4f; // Başlangıç sıcaklığı
                    tempDecrease = 1f; // Açıkken sıcaklık düşüşü
                    tempIncrease = 0.5f; // Kapalıyken sıcaklık artışı
                    break;
                case 1: // Air Conditioner
                    currentTemperature = 25f; // Başlangıç sıcaklığı
                    tempDecrease = 0.1f; // Açıkken sıcaklık düşüşü
                    tempIncrease = 0.1f; // Kapalıyken sıcaklık artışı
                    break;
                default:
                    isRegular = false; // Diğer cihazlar için düzenli ihtiyaç yok
                    break;
            }
        }
        else
        {
            
        }
    }

    public IEnumerator Fridge()
    {
        while (timer < puzzleDuration)
        {
            if (isOn)
            {
                yield return new WaitForSeconds(1f);
                currentTemperature -= tempDecrease;
            }
            else
            {
                yield return new WaitForSeconds(1f);
                currentTemperature += tempIncrease;
            }
            if (currentTemperature <= 4f)
            {
                currentTemperature = 4f;
            }
            else if (currentTemperature >= 20f)
            {
                fail = true;
            }
            else if (currentTemperature >= 18f)
            {
                WarnUser("Fridge is getting warm!");
            }
        }
    }

    public IEnumerator AirConditioner()
    {
        while (timer < puzzleDuration)
        {
            if (isOn)
            {
                yield return new WaitForSeconds(1f);
                currentTemperature -= tempDecrease;
            }
            else
            {
                yield return new WaitForSeconds(1f);
                currentTemperature += tempIncrease;
            }
            if (currentTemperature <= 22f)
            {
                fail = true;
            }
            else if (currentTemperature <= 23f)
            {
                WarnUser("Room is getting cold!");
            }
            else if (currentTemperature >= 28f)
            {
                fail = true;
            }
            else if (currentTemperature >= 27f)
            {
                WarnUser("Room is getting hot!");
            }
        }
    }

    public IEnumerator OneCycle()
    {
        while (timer < puzzleDuration)
        {
            float cycleTimer = 0f;
            bool cycleFail = true;
            needInterval = UnityEngine.Random.Range(10f, 60f); // İhtiyaç aralığı
            needDuration = UnityEngine.Random.Range(5f, 15f);  // İhtiyaç süresi

            while (cycleTimer < needInterval)
            {
                cycleTimer += 1f;
                yield return new WaitForSeconds(1f);
            }
            WarnUser("Open" + applianceID);
            float waitTimer = 0f;
            while (waitTimer < 10f)
            {
                waitTimer += 1f;
                if (isOn)
                {
                    cycleFail = false;
                    break;
                }
                yield return new WaitForSeconds(1f);
            }
            if (cycleFail)
            {
                fail = true;
            }
            else
            {
                DeactivateAppliance();
                while (cycleTimer < needInterval + waitTimer + needDuration)
                {
                    cycleTimer += 1f;
                    yield return new WaitForSeconds(1f);
                }
                WarnUser(applianceID + " is completed.");
            }
        }
    }

    private void WarnUser(string message)
    {
        Debug.Log(message);
        // Cihazın ihtiyacı olduğunu belirtmek için ışık yak
    }

    public void SetIsOn(bool isOn)
    {
        this.isOn = isOn;
    }

    public bool GetIsOn()
    {
        return isOn;
    }

    public bool GetFail()
    {
        return fail;
    }

    private void DeactivateAppliance()
    {
        // Cihaz açıldığında interactable'ı devre dışı bırak, duration boyuna açık kalacak
    }

    public void UpdateTimer(float deltaTime)
    {
        timer += deltaTime;
    }

    public string GetStatus()
    {
        if (isRegular)
        {
            return $"Appliance {applianceID} - Temp: {currentTemperature:F1}°C - On: {isOn}";
        }
        else
        {
            return $"Appliance {applianceID} - On: {isOn}";
        }
    }

}
