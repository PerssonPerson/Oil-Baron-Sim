using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Eventtext : MonoBehaviour
{
   TextMeshProUGUI eventText;

    string[] eventArray = new string[] { "Consumers want lower prices, do you want to reduce prices?",
       "A landowner has offered to sell an area rich in oil. Do you want to buy the land?",
       "The stock market has crashed! Do you wish to fire some of your employees to save money?",
   "Bad rumours about your corporation have begun spreading. Do you want to increase advertising?",
   "An oil leak has appeared and oil is pouring into the ocean.\nDo you want to bribe your employees to make sure nobody finds out about the leak?",
   "Oil consumption has increased and your stocks are rising in value. Do you want to increase your employees' wages to compensate for inflation?",
   "Through advertising on social media people's opinion of your company has increased.\nDo you want to spend more money on social media advertising?",
   "A more effective way of producing oil has been developed! Do you want to begin using it?",
   "Demand for oil has rapidly begun increasing. Do you want to accelerate the production rate to satisfy demand?",
   "Your phone is ringing, and it�s from a known swindler. Maybe he has a business proposal for you..?",
   "People are protesting outside your office. Do you want to bribe the police to stop them?",
   "One of your oil rigs is in need of massive repairs. You could either hire experienced workers who can repair without harming marine life,\nor let your own workers repair despite the damage it would cause. Do you want to hire outsourced workers?",
   "One of your oil tankers has sunk, leaving large amounts of oil in the ocean. Do you want to clean it up?",
   "A reporter has asked for a tour on one of your oil rigs. It would give us a better public image, but would lower oil production rate temporarily. Do you accept the request?",
   "A competing oil corporation has caused an extraordinarily large oil spillage. This has caused outrage against the oil industry. Do you wish to make a public statement?",
   "The government has offered your company subsidies in exchange for us lowering our prices. Do you agree to the deal?",
   "An oil extraction method which is much less harmful for nature has been invented. However it is also more expensive. Do you want to adopt the new method?",
   "A news site has just published a hit piece, detailing some of our shady business practices.\nDo you want to blame a few of your own employees and fire them?"
   };

    


   //string event1;



    //string event1A = "Yes";

    // string event1B = "No";

    //bool event1Active;

    //den h�r kan man anv�nda f�r att se till att man inte kan skippa ett event genom att byta dag



    // bool yes;



    //int smallAmnt = 10;

    //bool no;

    // Start is called before the first frame update


    void Start()
    {
        

        

        eventText = GameObject.Find("Eventtext").GetComponent<TextMeshProUGUI>();



        /*eventArray[2] = "Stock market crashes!";

        eventArray[3] = "Bad rumours are spreading about your corporation";

        eventArray[4] = "An oil pipeline is leaking";

        eventArray[5] = "Stocks rise in value and sales are increasing";

        eventArray[6] = "You have gained traction on social media and more people buy oil";

        eventArray[7] = "A more effective way of extracting oil has been invented";*/
    }

   

    // Update is called once per frame
    void Update()
    {
        // visar event texten tills man har svarat
        if (FindObjectOfType<GameManager>().eventActive == true)
        {
            eventText.text = eventArray[FindObjectOfType<GameManager>().eventArrayNumber];
        }
        else 
        {
            eventText.text = "";
        }
        

        /*

                yes = FindObjectOfType<Yes>().yes1;




               


                if(yes == true) 
                {
                    //FindObjectOfType<GameManager>().money = FindObjectOfType<GameManager>().money - smallAmnt;
                    FindObjectOfType<SkipdayLockTest>().eventBlock = false;
                    yes = false;
                    FindObjectOfType<Yes>().yes1 = false;

                }*/

        /* if (event1Active == true)
         {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                eventText.text = event1A;
                event1Active = false;
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                eventText.text = event1B;
                event1Active = false;
            }
        }
        */

    }
}
