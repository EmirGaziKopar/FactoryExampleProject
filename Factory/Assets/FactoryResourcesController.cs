using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FactoryResourcesController : MonoBehaviour
{
    public static int DeliveryPlastic;
    public static int DeliveryCopper;
    public static int DeliverySteel;
    public static bool canProduceFinalProduct;

    public bool motorProduced;
    public bool Steel;
    public bool Copper;
    public bool Plastic;
    public bool Glass;
    public bool Rubber;
    public bool Silicon;

    // Her malzeme için ayrý bir timer ve loading bar
    public Image LoadingSteel;
    public Image LoadingCopper;
    public Image LoadingPlastic;
    public Image LoadingGlass;
    public Image LoadingRubber;
    public Image LoadingSilicon;

    public float TimerSteel;
    public float TimerCopper;
    public float TimerPlastic;
    public float TimerGlass;
    public float TimerRubber;
    public float TimerSilicon;

    public float Speed;
 
    public static int steel = 50;
    public static int copper = 50;
    public static int plastic = 50;
    public static int glass = 50;
    public static int rubber = 50;
    public static int silicon = 50;
    public static int motor = 50;

    public TextMeshProUGUI TextOfCopper;
    public TextMeshProUGUI TextOfSteel;
    public TextMeshProUGUI TextOfPlastic;
    public TextMeshProUGUI TextOfGlass;
    public TextMeshProUGUI TextOfRubber;
    public TextMeshProUGUI TextOfSilicon;
    
    public int requiredSteel = 1;
    public int requiredCopper = 1;
    public int requiredPlastic = 1;
    public int requiredGlass = 1;
    public int requiredRubber = 1;
    public int requiredSilicon = 1;

    public void CanProduceFinalProduct()
    {
        if(steel > 0 && copper > 0 && plastic > 0 && glass > 0 && silicon > 0)
        {
            canProduceFinalProduct = true;
        }
                
    }


    public bool AllResourcesCollected()
    {
        return steel >= requiredSteel &&
               copper >= requiredCopper &&
               plastic >= requiredPlastic &&
               glass >= requiredGlass &&
               rubber >= requiredRubber &&
               silicon >= requiredSilicon;
    }

    // Baþ mühendis motor üretimi gerçekleþtirir.
    public void ProduceMotor()
    {
        if (FactoryResourcesController.canProduceFinalProduct)
        {
            /*
            // Kaynaklarý azalt, motor üretildi.
            steel -= 1;
            copper -= 1;
            plastic -= 1;
            glass -= 1;
            rubber -= 1;
            silicon -= 1;
            motorProduced = true;
            Debug.Log("Motor produced by the chief engineer!");
            */
        }
    }

    public bool HasResourcesForWorker(string workerTag)
    {
        switch (workerTag)
        {
            case "isci1":
                return steel > 0;
            case "isci2":
                return copper > 0;
            case "isci3":
                return plastic > 0;
            case "isci4":
                return glass > 0;
            case "isci5":
                return rubber > 0;
            case "isci6":
                return silicon > 0;
            default:
                return false;
        }
    }

    // Kaynak tüketme metodu
    public void ConsumeResource(string workerTag)
    {
        switch (workerTag)
        {
            case "isci1":
                if (steel > 0) steel -= 1;
                break;
            case "isci2":
                if (copper > 0) copper -= 1;
                break;
            case "isci3":
                if (plastic > 0) plastic -= 1;
                break;
            case "isci4":
                if (glass > 0) glass -= 1;
                break;
            case "isci5":
                if (rubber > 0) rubber -= 1;
                break;
            case "isci6":
                if (silicon > 0) silicon -= 1;
                break;
        }
    }

    // Steel üretimini baþlatmak için
    public void StartSteelProduction()
    {
        Steel = true;
    }

    // Copper üretimini baþlatmak için
    public void StartCopperProduction()
    {
        Copper = true;
    }

    // Plastic üretimini baþlatmak için
    public void StartPlasticProduction()
    {
        Plastic = true;
    }

    // Glass üretimini baþlatmak için
    public void StartGlassProduction()
    {
        Glass = true;
    }

    // Rubber üretimini baþlatmak için
    public void StartRubberProduction()
    {
        Rubber = true;
    }

    // Silicon üretimini baþlatmak için
    public void StartSiliconProduction()
    {
        Silicon = true;
    }

    void Update()
    {
        TextOfSteel.text = steel.ToString();
        TextOfCopper.text = copper.ToString();
        TextOfPlastic.text = plastic.ToString();
        TextOfGlass.text = glass.ToString();
        TextOfSilicon.text = silicon.ToString();
        TextOfRubber.text = rubber.ToString();
        LoadingRubber.fillAmount = TimerRubber * Speed;
        TextOfGlass.text = glass.ToString();
        if (Steel)
        {
            TimerSteel += Time.deltaTime;
            LoadingSteel.fillAmount = TimerSteel * Speed;
            if (LoadingSteel.fillAmount == 1)
            {
                steel += 1;
                TextOfSteel.text = steel.ToString();
                Steel = false;
                LoadingSteel.fillAmount = 0f;
                TimerSteel = 0f;
            }
        }

        // Copper üretim iþlemi
        if (Copper)
        {
            TimerCopper += Time.deltaTime;
            LoadingCopper.fillAmount = TimerCopper * Speed;
            if (LoadingCopper.fillAmount == 1)
            {
                copper += 1;
                TextOfCopper.text = copper.ToString();
                Copper = false;
                LoadingCopper.fillAmount = 0f;
                TimerCopper = 0f;
            }
        }

        // Plastic üretim iþlemi
        if (Plastic)
        {
            TimerPlastic += Time.deltaTime;
            LoadingPlastic.fillAmount = TimerPlastic * Speed;
            if (LoadingPlastic.fillAmount == 1)
            {
                plastic += 1;
                TextOfPlastic.text = plastic.ToString();
                Plastic = false;
                LoadingPlastic.fillAmount = 0f;
                TimerPlastic = 0f;
            }
        }

        // Glass üretim iþlemi
        if (Glass)
        {
            TimerGlass += Time.deltaTime;
            LoadingGlass.fillAmount = TimerGlass * Speed;
            if (LoadingGlass.fillAmount == 1)
            {
                glass += 1;
                TextOfGlass.text = glass.ToString();
                Glass = false;
                LoadingGlass.fillAmount = 0f;
                TimerGlass = 0f;
            }
        }

        // Rubber üretim iþlemi
        if (Rubber)
        {
            TimerRubber += Time.deltaTime;
            LoadingRubber.fillAmount = TimerRubber * Speed;
            if (LoadingRubber.fillAmount == 1)
            {
                rubber += 1;
                TextOfRubber.text = rubber.ToString();
                Rubber = false;
                LoadingRubber.fillAmount = 0f;
                TimerRubber = 0f;
            }
        }

        // Silicon üretim iþlemi
        if (Silicon)
        {
            TimerSilicon += Time.deltaTime;
            LoadingSilicon.fillAmount = TimerSilicon * Speed;
            if (LoadingSilicon.fillAmount == 1)
            {
                silicon += 1;
                TextOfSilicon.text = silicon.ToString();
                Silicon = false;
                LoadingSilicon.fillAmount = 0f;
                TimerSilicon = 0f;
            }
        }
    }
}
